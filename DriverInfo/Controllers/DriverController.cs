using DriverInfo.Data;
using DriverInfo.Models;
using DriverInfo.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DriverInfo.Controllers
{
    public class DriverController : Controller
    {
        private readonly IDriverRepository _driverRepository;
        private readonly IEmployeeRepository _employeeRepository; // Inkludera EmployeeRepository
        private readonly IEventRepository _eventRepository; // Lägg till EventRepository

        public DriverController(IDriverRepository driverRepository, IEmployeeRepository employeeRepository, IEventRepository eventRepository) // Lägg till EmployeeRepository här
        {
            _driverRepository = driverRepository;
            _employeeRepository = employeeRepository; // Spara EmployeeRepository
            _eventRepository = eventRepository; // Spara EventRepository
        }

        public async Task<IActionResult> Index()
        {
            // Hämta ID för inloggad anställd
            var employeeId = HttpContext.Session.GetInt32("UserId");
            var userRole = HttpContext.Session.GetString("UserRole"); // Hämta användarrollen

            IEnumerable<Driver> drivers;

            if (userRole == "Admin")
            {
                // För admin, hämta alla förare
                drivers = await _driverRepository.GetAllDriversAsync();
            }
            else if (userRole == "Employee" && employeeId.HasValue)
            {
                // För anställda, hämta endast förare som den inloggade anställda är ansvarig för
                drivers = await _driverRepository.GetDriversByResponsibleEmployeeIdAsync(employeeId.Value);
            }
            else
            {
                // Om ingen anställd är inloggad, returnera en tom lista eller hantera som önskat
                drivers = Enumerable.Empty<Driver>();
            }

            return View(drivers);
        }

        public async Task<IActionResult> Create()
        {
            var employees = await _employeeRepository.GetAllEmployeesAsync(); // Hämta anställda asynkront
            ViewBag.Employees = new SelectList(employees, "EmployeeID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Driver driver)
        {
            if (ModelState.IsValid)
            {
                await _driverRepository.AddDriverAsync(driver);
                return RedirectToAction(nameof(Index));
            }
            return View(driver);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var driver = await _driverRepository.GetDriverByIdAsync(id);
            if (driver == null)
            {
                return NotFound();
            }

            var employees = await _employeeRepository.GetAllEmployeesAsync(); // Använd EmployeeRepository
            ViewBag.Employees = new SelectList(employees, "EmployeeID", "Name", driver.ResponsibleEmployeeID);
            return View(driver);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Driver driver)
        {
            if (id != driver.DriverID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _driverRepository.UpdateDriverAsync(driver);
                return RedirectToAction(nameof(Index));
            }

            var employees = await _employeeRepository.GetAllEmployeesAsync(); // Använd EmployeeRepository
            ViewBag.Employees = new SelectList(employees, "EmployeeID", "Name", driver.ResponsibleEmployeeID);
            return View(driver);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var driver = await _driverRepository.GetDriverByIdAsync(id);
            if (driver == null)
            {
                return NotFound();
            }

            await _driverRepository.DeleteDriverAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            // Hämta föraren med relaterad information som ansvarig anställd och events
            var driver = await _driverRepository.GetDriverByIdAsync(id);

            if (driver == null)
            {
                return NotFound(); // Om ingen förare hittas
            }

            // Se till att driver.Events inte är null
            if (driver.Events != null && driver.Events.Any())
            {
                // Summera beloppen från event om det finns några
                driver.TotalAmountIn = driver.Events.Sum(e => e.AmountIn);
                driver.TotalAmountOut = driver.Events.Sum(e => e.AmountOut);
            }
            else
            {
                // Om inga events finns, sätt totalerna till 0
                driver.TotalAmountIn = 0;
                driver.TotalAmountOut = 0;
            }
            driver.Events = driver.Events.OrderByDescending(e => e.EventDate).ToList();

            // Beräkna balansen
            decimal balance = driver.TotalAmountIn - driver.TotalAmountOut;
            ViewBag.Balance = balance; // Skicka balansen till vyn

            return View(driver); // Skicka föraren som modell till vyn
        }
        public async Task<IActionResult> AddEvent(int driverId)
        {
            var driver = await _driverRepository.GetDriverByIdAsync(driverId);
            if (driver == null)
            {
                return NotFound();
            }

            // Pass the driver name to the view using ViewBag
            ViewBag.DriverName = driver.DriverName;

            return View(new Event { DriverID = driverId }); // Pass the new Event model to the view
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEvent(Event eventItem)
        {
            if (ModelState.IsValid)
            {
                await _eventRepository.AddEventAsync(eventItem);
                return RedirectToAction(nameof(Details), new { id = eventItem.DriverID }); // Redirect to driver details
            }

            // Hämta förarnamnet om vi behöver återvisa vyn efter valideringsfel
            var driver = await _driverRepository.GetDriverByIdAsync(eventItem.DriverID.Value);
            ViewBag.DriverName = driver?.DriverName;

            // Return the model with validation errors
            return View(eventItem);
        }
    }
}

