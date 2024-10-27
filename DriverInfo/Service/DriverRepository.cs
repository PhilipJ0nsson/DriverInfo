using DriverInfo.Data;
using DriverInfo.Models;
using Microsoft.EntityFrameworkCore;

namespace DriverInfo.Service
{
    public interface IDriverRepository
    {
        Task<IEnumerable<Driver>> GetAllDriversAsync();
        Task<Driver> GetDriverByIdAsync(int id);
        Task AddDriverAsync(Driver driver);
        Task UpdateDriverAsync(Driver driver);
        Task DeleteDriverAsync(int id);
        Task<IEnumerable<Driver>> GetDriversByResponsibleEmployeeIdAsync(int employeeId);
    }
    public class DriverRepository : IDriverRepository
    {
        private readonly AppDbContext _context;

        public DriverRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Driver>> GetAllDriversAsync()
        {
            return await _context.Drivers.ToListAsync();
        }

        public async Task<Driver> GetDriverByIdAsync(int id)
        {
            return await _context.Drivers
                .Include(d => d.ResponsibleEmployee) // Inkludera ansvarig anställd
                .Include(d => d.Events) // Inkludera relaterade händelser
                .FirstOrDefaultAsync(d => d.DriverID == id);
        }

        public async Task AddDriverAsync(Driver driver)
        {
            await _context.Drivers.AddAsync(driver); // Använd AddAsync här
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDriverAsync(Driver driver)
        {
            _context.Drivers.Update(driver);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDriverAsync(int id)
        {
            var driver = await _context.Drivers.FindAsync(id);
            if (driver != null)
            {
                _context.Drivers.Remove(driver);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<Driver>> GetDriversByResponsibleEmployeeIdAsync(int employeeId)
        {
            return await _context.Drivers
                .Where(d => d.ResponsibleEmployeeID == employeeId)
                .ToListAsync();
        }
    }
}
