﻿using Microsoft.AspNetCore.Mvc;
using OnlineRetailStoreV01.Data;
using OnlineRetailStoreV01.Models;
using OnlineRetailStoreV01.Service;

namespace OnlineRetailStoreV01.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUserService _userService;

        public AdminController(IUserService userService,AppDbContext db)
        {
            _userService = userService;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _userService.GetAllUsersAsync());
        }

        
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            user.Password=_userService.HashPassword(user.Password);
            await _userService.AddUserAsync(user);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var existingUser=await _userService.GetUserByIdAsync(id);
            if(existingUser != null)
            {
                return View(existingUser);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,User user)
        {
            if (id!= user.UserId || !ModelState.IsValid)
            {
                return View(user);
            }
            var existingUser=await _userService.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound();
            }
            if (!_userService.VerifyPassWord(_userService.HashPassword(user.Password), existingUser.Password))
            {
                user.Password = _userService.HashPassword(user.Password);
            }
            await _userService.UpdateUserAsync(id,user);
            return RedirectToAction("Index");
        }

        
        public async Task<IActionResult> Delete(int id)
        {
            var existingUser = await _userService.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound();
            }
            return View(existingUser);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _userService.DeleteUserAsync(id);
            return RedirectToAction("Index");
        }

        
        public async Task<IActionResult> Details(int id)
        {
            var existingUser = await _userService.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound();
            }
            return View(existingUser);
        }
    }
}
