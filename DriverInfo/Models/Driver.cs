namespace DriverInfo.Models
{
    public class Driver
    {
        public int DriverID { get; set; }
        public string DriverName { get; set; }
        public string CarReg { get; set; }
        public int ResponsibleEmployeeID { get; set; } // Foreign key
        public decimal TotalAmountOut { get; set; }
        public decimal TotalAmountIn { get; set; }
        public decimal Balance => TotalAmountIn - TotalAmountOut; // Ny property för balans

        // Navigation properties
        public virtual Employee ResponsibleEmployee { get; set; } // Navigation property
        public virtual ICollection<Event> Events { get; set; }
    }
}
