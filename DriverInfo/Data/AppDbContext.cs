using DriverInfo.Models;
using Microsoft.EntityFrameworkCore;

namespace DriverInfo.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Event> Events { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    // Skapa initiala data här
        //    modelBuilder.Entity<Employee>().HasData(
        //        new Employee
        //        {
        //            EmployeeID = 1,
        //            Name = "Admin User",
        //            Email = "admin@example.com",
        //            Password = "SecurePassword123", // Tänk på att hasha lösenord i verklig användning
        //            Role = "Admin"
        //        },
        //        new Employee
        //        {
        //            EmployeeID = 2,
        //            Name = "Regular Employee",
        //            Email = "employee@example.com",
        //            Password = "SecurePassword456", // Tänk på att hasha lösenord i verklig användning
        //            Role = "Employee"
        //        }
        //    );

        //    modelBuilder.Entity<Driver>().HasData(
        //        new Driver
        //        {
        //            DriverID = 1,
        //            DriverName = "John Doe",
        //            CarReg = "ABC123",
        //            NoteDescription = "Initial note",
        //            ResponsibleEmployee = "Admin User",
        //            AmountOut = 0,
        //            AmountIn = 0,
        //            TotalAmountOut = 0,
        //            TotalAmountIn = 0
        //        }
        //    );

        //    modelBuilder.Entity<Event>().HasData(
        //        new Event
        //        {
        //            EventID = 1,
        //            DriverID = 1, // FK till Driver
        //            EventDate = DateTime.Now,
        //            Description = "Driver John Doe started his shift.",
        //            ResponsibleEmployee = "Admin User"
        //        }
        //    );
        //}

    }
}
