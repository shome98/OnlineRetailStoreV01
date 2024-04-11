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
    }
}
