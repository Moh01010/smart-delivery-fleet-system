using Smart_Delivery___Fleet_Management_System.Enums;

namespace Smart_Delivery___Fleet_Management_System.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Phone { get; set; }
        public string PasswordHash { get; set; }

        public UserRole Role { get; set; }
    }
}
