using Smart_Delivery___Fleet_Management_System.Enums;

namespace Smart_Delivery___Fleet_Management_System.Models
{
    public class Driver
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public DriverStatus Status { get; set; }
        public double CurrentLat { get; set; }
        public double CurrentLng { get; set; }
        public double Rating { get; set; }
        public DateTime CreatedAt {  get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<DriverLocation> Locations { get; set; }
    }
}
