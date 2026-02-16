using Smart_Delivery___Fleet_Management_System.Enums;

namespace Smart_Delivery___Fleet_Management_System.DTOs.Dispatcher
{
    public class ActiveOrderDto
    {
        public int Id { get; set; }
        public string PickupAddress { get; set; }
        public string DropoffAddress { get; set; }
        public OrderStatus Status { get; set; }
        public int? AssignedDriverId { get; set; }
    }
}
