namespace Smart_Delivery___Fleet_Management_System.DTOs.Order
{
    public class CreateOrderDto
    {
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string PickupAddress { get; set; }
        public string DropoffAddress { get; set; }
    }
}
