using OnlineRetailStoreV01.Controllers;
using OnlineRetailStoreV01.Models;

namespace OnlineRetailStoreV01.Service
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int userId);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(int userId,User updatedUser);
        Task DeleteUserAsync(int userId);
        bool VerifyPassWord(string password, string hashedPassword);
        string HashPassword(string password);
        Task<bool> Authenticate(string email, string password);
        Task<User> GetUserByEmailAsync(string email);
        Task<bool> RegisterAsync(User model);

    }
}
