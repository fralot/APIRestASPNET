//using APIRest.Helpers;
//using APIRest.Models;

//namespace APIRest.Service
//{
//    public class AuthenticationService : IAuthenticationService
//    {
//        private readonly List<UserModel> _users = new List<UserModel>();

//        public Task<AuthenticationResponse> RegisterAsync(RegisterModel register)
//        {
//            if (_users.Any(u => u.Email == register.Email))
//                return Task.FromResult(new AuthenticationResponse { Success = false, Message = "Email already exists" });

//            var hashedPassword = EncryptionHelper.HashPassword(register.Password);
//            _users.Add(new UserModel { Email = register.Email, PasswordHash = hashedPassword, Role = register.Role });

//            return Task.FromResult(new AuthenticationResponse { Success = true, Message = "User registered successfully" });
//        }

//        public Task<AuthenticationResponse> LoginAsync(LoginModel login)
//        {
//            var user = _users.FirstOrDefault(u => u.Email == login.Email);
//            if (user == null || !EncryptionHelper.VerifyPassword(login.Password, user.PasswordHash))
//                return Task.FromResult(new AuthenticationResponse { Success = false, Message = "Invalid credentials" });

//            return Task.FromResult(new AuthenticationResponse { Success = true, Role = user.Role });
//        }
//    }

//}
