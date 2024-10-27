using DriverInfo.Models;
using Microsoft.AspNetCore.Mvc;

namespace DriverInfo.Controllers
{
    public class AccountController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;

        public AccountController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginEmployee()); // Skapa en ny instans av LoginModel
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginEmployee model) // Gör metoden asynkron
        {
            if (ModelState.IsValid)
            {
                model.Email = model.Email.ToLower();

                // Hämta alla anställda asynkront
                var employees = await _employeeRepository.GetAllEmployeesAsync();

                // Autentisering av användaren här
                var employee = employees
                    .FirstOrDefault(e => e.Email == model.Email && e.Password == model.Password);

                if (employee != null)
                {
                    // Sätt session eller cookie
                    HttpContext.Session.SetString("UserEmail", employee.Email);
                    HttpContext.Session.SetString("UserRole", employee.Role);
                    HttpContext.Session.SetInt32("UserId", employee.EmployeeID); // Lägg till denna rad
                    return RedirectToAction("Index", "Event"); // Om autentisering lyckas
                }
            }
            ModelState.AddModelError("", "Invalid login attempt.");
            return View(model); // Återställs till inloggningsvyn med meddelande
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Rensa sessionen
            return RedirectToAction("Login");
        }
    }
}
