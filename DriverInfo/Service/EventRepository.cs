using DriverInfo.Data;
using DriverInfo.Models;
using Microsoft.EntityFrameworkCore;

namespace DriverInfo.Service
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetAllEventsAsync();
        Task<Event> GetEventByIdAsync(int id);
        Task AddEventAsync(Event eventItem);
        Task UpdateEventAsync(Event eventItem);
        Task DeleteEventAsync(int id);
        Task<IEnumerable<Event>> GetEventsByDriverIdAsync(int driverId);
        Task<IEnumerable<Event>> GetRecentEventsAsync(DateTime since);
        Task<IEnumerable<Event>> GetEventsByEmployeeIdAsync(int employeeId);
        Task<IEnumerable<Event>> GetEventsInRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Event>> GetEventsByEmployeeIdAndDateRangeAsync(int employeeId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<Event>> GetEventsByResponsibleEmployeeIdAsync(int employeeId);
    }
    public class EventRepository : IEventRepository
    {
        private readonly AppDbContext _context;

        public EventRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Event>> GetAllEventsAsync()
        {
            return await _context.Events
                .Include(e => e.Driver) // Om du vill inkludera relaterad information om förare
                .ThenInclude(d => d.ResponsibleEmployee)
                .Include(e => e.Employee) // Om du vill inkludera relaterad information om anställd
                .ToListAsync();
        }

        public async Task<Event> GetEventByIdAsync(int id)
        {
            return await _context.Events
                .Include(e => e.Driver)
                .Include(e => e.Employee)
                .FirstOrDefaultAsync(e => e.EventID == id);
        }

        public async Task AddEventAsync(Event eventItem)
        {
            _context.Events.Add(eventItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEventAsync(Event eventItem)
        {
            _context.Events.Update(eventItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEventAsync(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);
            if (eventItem != null)
            {
                _context.Events.Remove(eventItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Event>> GetEventsByDriverIdAsync(int driverId)
        {
            return await _context.Events
                .Where(e => e.DriverID == driverId) // Filtrera händelser baserat på förare
                .Include(e => e.Driver) // Inkludera information om föraren
                .Include(e => e.Employee) // Inkludera information om anställd
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetRecentEventsAsync(DateTime since)
        {
            return await _context.Events
                .Where(e => e.EventDate >= since)
                .Include(e => e.Driver)
                .Include(e => e.Employee)
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetEventsByEmployeeIdAsync(int employeeId)
        {
            return await _context.Events
                .Where(e => e.EmployeeID == employeeId)
                .Include(e => e.Driver)
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetEventsInRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Events
                .Where(e => e.EventDate >= startDate && e.EventDate <= endDate)
                .Include(e => e.Driver)
                .Include(e => e.Employee)
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetEventsByEmployeeIdAndDateRangeAsync(int employeeId, DateTime startDate, DateTime endDate)
        {
            return await _context.Events
                .Where(e => e.EmployeeID == employeeId && e.EventDate >= startDate && e.EventDate <= endDate)
                .Include(e => e.Driver)
                .ToListAsync();
        }
        public async Task<IEnumerable<Event>> GetEventsByResponsibleEmployeeIdAsync(int employeeId)
        {
            // Först hämta alla förare som den angivna anställda är ansvarig för
            var drivers = await _context.Drivers
                .Where(d => d.ResponsibleEmployeeID == employeeId)
                .ToListAsync();

            // Hämta alla händelser kopplade till dessa förare
            var events = await _context.Events
                .Where(e => drivers.Select(d => d.DriverID).Contains(e.DriverID.Value)) // Filtrera händelser baserat på förare
                .Include(e => e.Driver) // Inkludera förare information
                .ThenInclude(d => d.ResponsibleEmployee)
                .Include(e => e.Employee) // Inkludera anställd information
                .ToListAsync();

            return events;
        }
    }
}
