using OnlineRetailStoreV01.Models;

namespace OnlineRetailStoreV01.Repository
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(int userId);
        Task<IEnumerable<User>> GetAllUsersAsync(); 
        Task AddUserAsync(User user);
        Task DeleteUserAsync(int userId);
        Task UpdateUserAsync(int userId,User updatedUser);
    }
}
