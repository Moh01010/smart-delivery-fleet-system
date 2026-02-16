using Smart_Delivery___Fleet_Management_System.Enums;

namespace Smart_Delivery___Fleet_Management_System.DTOs.Dispatcher
{
    public class LiveDriverDto
    {
        public int DriverId { get; set; }
        public string Name { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public DriverStatus Status { get; set; }
        public int ActiveOrders { get; set; }
    }
}
