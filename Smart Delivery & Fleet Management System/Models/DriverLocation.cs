namespace Smart_Delivery___Fleet_Management_System.Models
{
    public class DriverLocation
    {
        public int Id { get; set; }

        public int DriverId { get; set; }
        public Driver Driver { get; set; }

        public double Lat { get; set; }
        public double Lng { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
