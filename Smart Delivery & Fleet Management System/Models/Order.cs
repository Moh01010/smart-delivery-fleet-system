using Smart_Delivery___Fleet_Management_System.Enums;

namespace Smart_Delivery___Fleet_Management_System.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }

        public string PickupAddress { get; set; }
        public double PickupLat { get; set; }
        public double PickupLng { get; set; }
        public string DropoffAddress { get; set; }
        public double DropoffLat { get; set; }
        public double DropoffLng { get; set; }
        public OrderStatus Status { get; set; }
        public int? AssignedDriverId { get; set; }
        public Driver AssignedDriver { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<OrderStatusHistory> StatusHistory { get; set; }
    }
}
