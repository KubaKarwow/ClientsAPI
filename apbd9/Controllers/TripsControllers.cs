using APBD9.Data;
using APBD9.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APBD9.Controllers;

[ApiController]
[Route("api/trips")]
public class TripsControllers: ControllerBase
{
    private readonly ApbdContext _context;

    public TripsControllers(ApbdContext _context)
    {
        this._context = _context;
    }

   
    [HttpGet]
    public async Task<IActionResult> GetTrips([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var totalTrips = await _context.Trips.CountAsync();

        var trips = await _context.Trips
            .OrderByDescending(e => e.DateFrom)
            .Skip(page - 1)
            .Take(pageSize)
            .Select(e => new
            {
                e.Name,
                e.Description,
                e.DateFrom,
                e.DateTo,
                e.MaxPeople,
                Countries = _context.Countries
                    .Where(c => c.IdTrips.Any(t => t.IdTrip == e.IdTrip))
                    .Select(c => new { c.Name })
                    .ToList(),
                Clients = _context.ClientTrips
                    .Where(ct => ct.IdTrip == e.IdTrip)
                    .Select(ct => new { ct.IdClientNavigation.FirstName, ct.IdClientNavigation.LastName })
                    .ToList()
            })
            .ToListAsync();

        
        var response = new
        {
            pageNum = page,
            pageSize = pageSize,
            allPages = totalTrips,
            trips = trips
        };

        return Ok(response);
    }

    [HttpPost("{idTrip:int}/clients")]
    public async Task<IActionResult> assignClientToTrip(int idTrip, [FromBody] ClientToTripDTO clientToTripDto)
    {
        var countAsync = await _context.Clients.Where(c => c.Pesel==clientToTripDto.Pesel).CountAsync();
        if (countAsync > 0)
        {
            return BadRequest("Client with this pesel already exists");
        }

        var trip = await _context.Trips.FirstOrDefaultAsync(t => t.IdTrip == idTrip);
        if (trip == null)
        {
            return NotFound("No such trip");
        }
        if (trip.DateFrom.Date < DateTime.Now)
        {
            return BadRequest("This trip already took place");
        }

        var client = new Client
        {
            FirstName = clientToTripDto.FirstName,
            LastName = clientToTripDto.LastName,
            Email = clientToTripDto.Email,
            Telephone = clientToTripDto.Telephone,
            Pesel = clientToTripDto.Pesel
        };
        _context.Clients.Add(client);
        await _context.SaveChangesAsync();

        var clientTrip = new ClientTrip
        {
            IdClient = client.IdClient,
            IdTrip = idTrip,
            RegisteredAt = DateTime.Now,
            PaymentDate = DateTime.Parse(clientToTripDto.PaymentDate)
        };

        _context.ClientTrips.Add(clientTrip);
        await _context.SaveChangesAsync();

        return Ok("Everything went well, client and client to trip added B)");
    }
    

}