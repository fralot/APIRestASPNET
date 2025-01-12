using APIRest.Models;
using System.Threading.Tasks;

namespace APIRest.Services
{
    public interface IUserService
    {
        Task<bool> ValidateUserAsync(string email, string password);
        Task<string> GetUserRoleAsync(string email, string password);
        Task<PasswordsModel> GetUserByEmailAsync(string email);
        Task AddUserAsync(string email, string password, string salt, string role);
    }
}