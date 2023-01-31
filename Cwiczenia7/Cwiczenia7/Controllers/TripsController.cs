using Cwiczenia7.Models;
using Cwiczenia7.Models.DTO;
using Cwiczenia7.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cwiczenia7.Controllers
{
    [Route("api/trips")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        private readonly IDbService _service;

        public TripsController(IDbService service)
        {
            _service = service;
        }

        [HttpGet()]
        public async Task<IActionResult> GetTrips()
        {
            var trips = await _service.GetTrips();
            return Ok(trips);

        }

        [HttpDelete("/api/clients/{idClient}")]
        public async Task<IActionResult> RemoveClient(int idClient) 
        {
            bool response = await _service.RemoveClient(idClient);
            if (response)
            {
                return Ok();           
            }
            return BadRequest();
        }
        [HttpPost("{idTrip}/clients")]
        public async Task<IActionResult> AssignClientToTrip(int idTrip, UpdateClientDTO updateClient)
        {
            bool response = await _service.AssignClientToTrip(idTrip, updateClient);
            if (response)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
