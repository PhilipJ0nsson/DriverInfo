using DriverInfo.Data;
using DriverInfo.Models;
using DriverInfo.Service;
using Microsoft.EntityFrameworkCore;

namespace DriverInfo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Lägg till session
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true; // För att sessionen ska fungera
            });

            //Add DB provider
            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection")));


            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            builder.Services.AddScoped<IEventRepository, EventRepository>();
            builder.Services.AddScoped<IDriverRepository, DriverRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}");

            app.Run();
        }
        public void Configure(IApplicationBuilder app, IServiceProvider serviceProvider)
        {
            // Övrig konfiguration för appen
            // ...

            // Skapa en administratörsanvändare i Azure SQL-databasen om den inte finns
            CreateAdminUser(serviceProvider);
        }

        private void CreateAdminUser(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                // Säkerställ att databasen är migrerad till senaste versionen
                context.Database.Migrate();

                // Kontrollera om det finns en administratör redan
                if (!context.Employees.Any(e => e.Role == "Admin"))
                {
                    var adminUser = new Employee
                    {
                        Name = "Admin",
                        Email = "admin@example.com",
                        Password = "Admin@123", // OBS: Hasha lösenordet i produktion
                        Role = "Admin"
                    };

                    context.Employees.Add(adminUser);
                    context.SaveChanges();
                }
            }
        }
    }
}
