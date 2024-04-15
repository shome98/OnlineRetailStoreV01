using OnlineRetailStoreV01.Models;
using OnlineRetailStoreV01.Repository;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;

namespace OnlineRetailStoreV01.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository=userRepository;
        }
        public async Task AddUserAsync(User user)
        {
            try
            {
                if (user != null)
                {
                    
                    await _userRepository.AddUserAsync(user);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while adding a new user!!!\n Please try again later. ", ex);
            }
        }

        public async Task DeleteUserAsync(int userId)
        {
            try
            {
                await _userRepository.DeleteUserAsync(userId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while deleting user!!!\n Please try again later. ", ex);
            }


        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            try
            {
                return await _userRepository.GetAllUsersAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while fetching all the users!!!\n Please try again later. ", ex);
            }

        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            try 
            {
                return await _userRepository.GetUserByIdAsync(userId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while fetching the user!!!\n Please try again later. ", ex);
            }

        }

        public async Task UpdateUserAsync(int userId, User updatedUser)
        {
            try
            {
                await _userRepository.UpdateUserAsync(userId, updatedUser);
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while updating the user!!!\n Please try again later. ", ex);
            }
        }
        
        public  string HashPassword(string password)
        {
            using(var sha256=SHA256.Create())
            {
                var hashedBytes=sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public bool VerifyPassWord(string password,string hashedPassword)
        {
            string hashedInput=HashPassword(password);
            return string.Equals(hashedInput, hashedPassword);
        }
        public async Task<User> GetUserByEmailAsync(string email)
        {
            try
            {
                return await _userRepository.GetUserByEmailAsync(email);
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while updating the user!!!\n Please try again later. ", ex);
            }
        }
        public async Task<bool> Authenticate(string email,string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if(user!=null && VerifyPassWord(user.Password,password))
            {
                return true;
            }
            return false;
        }
        public async Task<bool> RegisterAsync(User model)
        {
            try
            {
                var user = new User
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    Password = HashPassword(model.Password), // Hash the password before storing
                    UserType = model.UserType
                };

                await  _userRepository.AddUserAsync(user); 

                return true; 
            }
            catch (Exception ex)
            { 
                return false; 
            }
            return false ;
        }
    }
}
