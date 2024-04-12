﻿using OnlineRetailStoreV01.Models;
using OnlineRetailStoreV01.Repository;
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
                user.Password =HashPassword(user.Password);
                await _userRepository.AddUserAsync(user);
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
            catch(Exception ex)
{
                throw new Exception("Error occurred while deleting the user!!!\n Please try again later. ", ex);
            }

        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _userRepository.GetUserByIdAsync(userId);
        }

        public async Task UpdateUserAsync(int userId, User updatedUser)
        {
            try
            {
                await _userRepository.UpdateUserAsync(userId, updatedUser);
            }
            catch(Exception ex) 
            {
                throw new Exception("Error occurred while updating the user!!!\n Please try again later. ", ex);
            }
        }
        
        private string HashPassword(string password)
        {
            using(var sha256=SHA256.Create())
            {
                var hashedBytes=sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private bool VerifyPassWord(string password,string hashedPassword)
        {
            string hashedInput=HashPassword(password);
            return string.Equals(hashedInput, hashedPassword);
        }
        public async Task<bool> Authenticate(int userId,string password)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if(user!=null && VerifyPassWord(user.Password,password))
            {
                return true;
            }
            return false;
        }
    }
}
