using Microsoft.EntityFrameworkCore;
using OnlineRetailStoreV01.Data;
using OnlineRetailStoreV01.Models;

namespace OnlineRetailStoreV01.Repository
{
    public class UserRepository:IUserRepository
    {
        private readonly AppDbContext _db;

        public UserRepository(AppDbContext db) 
        {
            _db = db;
        }

        public async Task AddUserAsync(User user)
        {
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();

        }

        public async Task DeleteUserAsync(int userId)
        {
            var user=await _db.Users.FindAsync(userId);
            if (user != null)
            {
                _db.Users.Remove(user);
                await _db.SaveChangesAsync();   
            }
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _db.Users.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _db.Users.FindAsync(userId);
        }

        public async Task UpdateUserAsync(User user)
        {
            //_db.Entry(user).State=EntityState.Modified;
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }
    }
}
