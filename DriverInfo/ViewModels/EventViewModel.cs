using DriverInfo.Models;

namespace DriverInfo.ViewModels
{
    public class EventViewModel
    {
        public int EventID { get; set; }
        public DateTime EventDate { get; set; }
        public string Description { get; set; }
        public decimal AmountOut { get; set; }
        public decimal AmountIn { get; set; }
        public string DriverName { get; set; }  // Förarnamn från Driver
        public string ResponsibleEmployeeName { get; set; }  // Anställdes namn från Employee
        public bool IsRecent { get; set; }
    }
}
