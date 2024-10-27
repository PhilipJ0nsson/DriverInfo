using DriverInfo.Models;
using DriverInfo.Service;
using DriverInfo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DriverInfo.Controllers
{
    public class EventController : Controller
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDriverRepository _driverRepository;

        public EventController(IEventRepository eventRepository, IEmployeeRepository employeeRepository, IDriverRepository driverRepository)
        {
            _eventRepository = eventRepository;
            _employeeRepository = employeeRepository;
            _driverRepository = driverRepository;
        }

        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate, int? driverId, int? employeeId)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            var events = new List<Event>();

            if (userRole == "Admin")
            {
                // För admin, hämta alla händelser
                events = (List<Event>)await _eventRepository.GetAllEventsAsync();

                // Hämta alla förare och anställda för admin
                ViewBag.Drivers = await _driverRepository.GetAllDriversAsync();
                ViewBag.Employees = await _employeeRepository.GetAllEmployeesAsync();
            }
            else if (userRole == "Employee")
            {
                var employeeIdFromSession = HttpContext.Session.GetInt32("UserId");
                if (employeeIdFromSession.HasValue)
                {
                    events = (List<Event>)await _eventRepository.GetEventsByResponsibleEmployeeIdAsync(employeeIdFromSession.Value);

                    // Hämta förare som är kopplade till den anställde
                    ViewBag.Drivers = await _driverRepository.GetDriversByResponsibleEmployeeIdAsync(employeeIdFromSession.Value);
                    ViewBag.Employees = null; // Ingen lista över anställda för enskilda anställda
                }
            }

            // Filtrera händelser baserat på datum om angivna
            if (startDate.HasValue && endDate.HasValue)
            {
                // Set endDate to the end of the day for proper filtering
                var endOfDay = endDate.Value.Date.AddDays(1).AddTicks(-1); // This gives you 23:59:59.9999999
                events = events.Where(e => e.EventDate >= startDate.Value && e.EventDate <= endOfDay).ToList();
            }

            // Filtrera baserat på förare
            if (driverId.HasValue)
            {
                events = events.Where(e => e.Driver != null && e.Driver.DriverID == driverId.Value).ToList();
            }

            // Filtrera baserat på ansvarig anställd (endast för admin)
            if (userRole == "Admin" && employeeId.HasValue)
            {
                events = events.Where(e => e.Driver?.ResponsibleEmployee?.EmployeeID == employeeId.Value).ToList();
            }

            // Sortera händelserna efter datum
            events = events.OrderByDescending(e => e.EventDate).ToList();

            // Beräkna total Amount In och Amount Out för de filtrerade resultaten (gäller endast för admin)
            if (userRole == "Admin")
            {
                ViewBag.TotalAmountIn = events.Sum(e => e.AmountIn);
                ViewBag.TotalAmountOut = events.Sum(e => e.AmountOut);
                ViewBag.Balance = ViewBag.TotalAmountIn - ViewBag.TotalAmountOut;
            }

            // Omvandla till ViewModel för att inkludera förare- och anställdinfo
            var eventViewModels = events.Select(e => new EventViewModel
            {
                EventID = e.EventID,
                EventDate = e.EventDate,
                Description = e.Description,
                AmountOut = e.AmountOut,
                AmountIn = e.AmountIn,
                DriverName = e.Driver?.DriverName ?? "N/A",
                ResponsibleEmployeeName = e.Driver?.ResponsibleEmployee?.Name ?? "N/A",
                IsRecent = userRole == "Admin" ? (e.EventDate >= DateTime.Now.AddHours(-24)) : (e.EventDate >= DateTime.Now.AddHours(-12))
            }).ToList();

            return View(eventViewModels);
        }
        // Visa formulär för att skapa en ny händelse
        public IActionResult Create()
        {
            return View();
        }

        // Skapa en ny händelse
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event eventItem)
        {
            if (ModelState.IsValid)
            {
                await _eventRepository.AddEventAsync(eventItem);
                return RedirectToAction(nameof(Index));
            }
            return View(eventItem);
        }

        // Visa detaljer om en specifik händelse
        public async Task<IActionResult> Details(int id)
        {
            var eventItem = await _eventRepository.GetEventByIdAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }
            return View(eventItem);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var eventItem = await _eventRepository.GetEventByIdAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }

            // Ensure the driver's name is retrieved correctly
            if (eventItem.Driver != null)
            {
                ViewBag.DriverName = eventItem.Driver.DriverName; // Assuming DriverName is a property of the Driver object
            }
            else
            {
                ViewBag.DriverName = "Unknown Driver"; // Fallback if no driver is found
            }

            return View(eventItem);
        }

        // Redigera en händelse
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Event eventItem)
        {
            if (id != eventItem.EventID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _eventRepository.UpdateEventAsync(eventItem);
                return RedirectToAction(nameof(Index));
            }
            return View(eventItem);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var eventItem = await _eventRepository.GetEventByIdAsync(id);  // Hämta händelse
            if (eventItem == null)
            {
                return NotFound();  // Om händelsen inte finns
            }

            await _eventRepository.DeleteEventAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}