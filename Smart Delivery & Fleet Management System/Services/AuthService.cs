using Smart_Delivery___Fleet_Management_System.Interface;

namespace Smart_Delivery___Fleet_Management_System.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly JwtService _jwt;
        private readonly PasswordHasher _hasher;

        public AuthService(IUserRepository userRepo, JwtService jwt, PasswordHasher hasher)
        {
            _userRepo = userRepo;
            _jwt = jwt;
            _hasher = hasher;
        }

        public async Task<string> LoginAsync(string phone, string password)
        {
            var user = await _userRepo.GetByPhoneAsync(phone);

            if (user == null)
                return null;

            if (!_hasher.Verify(password, user.PasswordHash))
                return null;


            return _jwt.GenerateToken(user);
        }
    }

}
