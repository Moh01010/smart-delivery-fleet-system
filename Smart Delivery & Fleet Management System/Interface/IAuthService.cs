namespace Smart_Delivery___Fleet_Management_System.Interface
{
    public interface IAuthService
    {
        Task<string> LoginAsync(string phone, string password);
    }
}
