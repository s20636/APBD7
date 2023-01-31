using Cwiczenia7.Models;
using Cwiczenia7.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace Cwiczenia7.Services
{
    public class DbService : IDbService
    {
        private readonly S20636Context _context;

        public DbService(S20636Context context)
        { 
            _context = context;
        }

        public async Task<IEnumerable<TripDTO>> GetTrips()
        {
            return await _context.Trips
                .Include(e => e.ClientTrips)
                .Include(e => e.IdCountries)
                .Select(e => new TripDTO 
            { 
                    Name = e.Name,
                    Description = e.Description,
                    DateFrom = e.DateFrom,
                    DateTo = e.DateTo,
                    MaxPeople = e.MaxPeople,
                    Countries = e.IdCountries.Select(c => new CountryDTO { Name = c.Name}).ToList(),
                    Clients = e.ClientTrips.Select(e => new ClientDTO { FirstName = e.IdClientNavigation.FirstName, LastName = e.IdClientNavigation.LastName}).ToList()         
            })
                .OrderByDescending(e => e.DateFrom)
                .ToListAsync();
                    
                    
                   
        }

        public async Task<bool> RemoveClient(int id)
        {

            var client = await _context.Clients.Include(c => c.ClientTrips).Where(c => c.IdClient == id).FirstOrDefaultAsync();
            if (client == null || client.ClientTrips.Any())
            { 
                return false;
            }
            _context.Attach(client);
            _context.Remove(client);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> AssignClientToTrip(int idTrip, UpdateClientDTO updateClient)
        {
            var client = await _context.Clients.Where(c => c.Pesel == updateClient.Pesel).FirstOrDefaultAsync();
            if (client == null)
            {
                client = new Client()
                {
                    FirstName = updateClient.FirstName,
                    LastName = updateClient.LastName,
                    Pesel = updateClient.Pesel,
                    Email = updateClient.Email,
                    Telephone = updateClient.Telephone
                };
                _context.Add(client);
                await _context.SaveChangesAsync();
                client = await _context.Clients.Where(c => c.Pesel == updateClient.Pesel).FirstAsync();
            }
            var trip = await _context.Trips.Where(t => t.IdTrip == idTrip).FirstOrDefaultAsync();
            if (trip == null)
            {
                return false;
            }
            var clientTrip = await _context.ClientTrips.Where(t => t.IdTrip == idTrip && t.IdClient == client.IdClient).FirstOrDefaultAsync();
            if (clientTrip != null)
            {
                return false;
            }
            clientTrip = new ClientTrip()
            {
                IdClient = client.IdClient,
                IdTrip = idTrip,
                RegisteredAt = DateTime.Now,
                PaymentDate = updateClient.PaymentDate
            };
            _context.ClientTrips.Add(clientTrip);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
