using APBD9.Data;
using APBD9.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APBD9.Controllers;

[ApiController]
[Route("api/clients")]
public class ClientsController : ControllerBase
{
    private readonly ApbdContext _context;

    public ClientsController(ApbdContext _context)
    {
        this._context = _context;
    }
    [HttpDelete("{idClient:int}")]
    public async Task<IActionResult> DeleteClient(int idClient)
    {
        var clientTrips = await _context.ClientTrips.Where(ct => ct.IdClient==idClient).CountAsync();
        if (clientTrips > 0)
        {
            return Conflict("This client has trips already assigned to him");
        }

        var client = new Client
        {
            IdClient = idClient
        };

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();
        return Ok("Deleted a client ^^");
    }
}