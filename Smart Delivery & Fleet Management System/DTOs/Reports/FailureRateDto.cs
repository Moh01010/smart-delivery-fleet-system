namespace Smart_Delivery___Fleet_Management_System.DTOs.Reports
{
    public class FailureRateDto
    {
        public int TotalOrders { get; set; }
        public int FailedOrders { get; set; }
        public double FailurePercentage { get; set; }
    }
}
