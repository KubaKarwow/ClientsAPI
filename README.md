# ClientsAPI

Using this database scheme, implemented with "database first"

Technologies used: Entity Framework, Swagger
![image](https://github.com/user-attachments/assets/4b3e9246-0648-4b88-baf9-a49a6cc73db5)

This API allows to use these endpoints:

-Trips List Endpoint

This functionality provides an API endpoint to retrieve a list of trips, sorted by start date in descending order. The endpoint supports optional pagination through query string parameters.

Endpoint: /api/trips
Method: GET

-Delete Client Endpoint

This functionality provides an API endpoint to delete a client. The endpoint includes a check to ensure the client has no assigned trips before deletion.

Endpoint: /api/clients/{idClient}
Method: DELETE

-Assign Client to Trip Endpoint
This functionality provides an API endpoint to assign a client to a trip. The endpoint includes validation checks to ensure the client and trip details meet specific criteria.

Endpoint: /api/trips/{idTrip}/clients
Method: POST

Description: This endpoint assigns a client to a specified trip. The server performs several checks to validate the request, including checking for the existence of the client and trip, and ensuring the trip is in the future. The registration date is set to the current time when the request is processed.

