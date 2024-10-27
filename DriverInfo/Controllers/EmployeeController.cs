using DriverInfo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DriverInfo.Controllers
{

    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository; // Ändrat till interface

        public EmployeeController(IEmployeeRepository employeeService)
        {
            _employeeRepository = employeeService;
        }

        public async Task<IActionResult> Index()
        {
            var employees = await _employeeRepository.GetAllEmployeesAsync();
            return View(employees);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                employee.Email = employee.Email.ToLower();

                await _employeeRepository.AddEmployeeAsync(employee);
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                employee.Email = employee.Email.ToLower();

                await _employeeRepository.UpdateEmployeeAsync(employee);
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);  // Hämta anställd
            if (employee != null)
            {
                await _employeeRepository.DeleteEmployeeAsync(id);  // Ta bort anställd på riktigt

                var userEmail = HttpContext.Session.GetString("UserEmail") ?? "UnknownUser";
                Console.WriteLine($"Employee {employee.Name} (ID: {employee.EmployeeID}) was deleted by {userEmail}");  // Loggning
            }
            else
            {
                ModelState.AddModelError("", "Anställd hittades inte.");
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }
    }
}