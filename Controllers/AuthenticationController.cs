using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OnlineRetailStoreV01.Models;
using OnlineRetailStoreV01.Service;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OnlineRetailStoreV01.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IUserService _userService;

        public AuthenticationController(IUserService userService)
        {
            _userService = userService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult LoginIndex()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register(User model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var isRegistered = _userService.RegisterAsync(model);
            bool isRes=isRegistered.Result;

            if (!isRes)
                return BadRequest(new { message = "Failed to register user" });

            //return Ok(new { message = "User registered successfully" });
            return Redirect("Admin/Index");
            return RedirectToAction("Index", "Admin");
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(string email,string passwword)
        {
            var isAuthenticated = _userService.Authenticate(email, passwword);

            if (!Convert.ToBoolean(isAuthenticated))
                return Unauthorized();

            
            // Get the user object based on email
            var user = await _userService.GetUserByEmailAsync(email);

            var token = GenerateJwtToken(user.UserId, user.UserType.ToString());

            string redirectTo = "";
            switch (user.UserType)
            {
                case UserType.Admin:
                    return RedirectToAction("Index", "Admin");
                    break;
                case UserType.Vendor:
                    return RedirectToAction("Index", "Vendor");
                    break;
                case UserType.Courier:
                    redirectTo = "/courier/dashboard";
                    break;
                case UserType.Customer:
                    redirectTo = "/customer/dashboard";
                    break;
                default:
                    return Unauthorized();
            }

            return Redirect(redirectTo);
        }

        // Other methods...

        private string GenerateJwtToken(int userId, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = GetTokenKeyForRole(role);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private byte[] GetTokenKeyForRole(string role)
        {
            switch (role)
            {
                case "Admin":
                    return Encoding.ASCII.GetBytes("admin-secret-key");
                case "Vendor":
                    return Encoding.ASCII.GetBytes("vendor-secret-key");
                case "Courier":
                    return Encoding.ASCII.GetBytes("courier-secret-key");
                case "Customer":
                    return Encoding.ASCII.GetBytes("customer-secret-key");
                default:
                    throw new ArgumentException("Invalid role");
            }
        }
    }
}
