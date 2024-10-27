namespace DriverInfo.Models
{
    public class Event
    {
        public int EventID { get; set; }
        public int? DriverID { get; set; }  // FK to Driver
        public int? EmployeeID { get; set; }  // FK to Employee
        public DateTime EventDate { get; set; } = DateTime.Now;
        public string Description { get; set; }
        public decimal AmountOut { get; set; }
        public decimal AmountIn { get; set; }

        // Navigation properties
        public virtual Driver? Driver { get; set; }
        public virtual Employee? Employee { get; set; }

    }
}
