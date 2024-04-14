using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineRetailStoreV01.Models;
using System.Net;

namespace OnlineRetailStoreV01
{
    public class FileName
    {
    }

}
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using OnlineRetailStoreV01.Models;

namespace OnlineRetailStoreV01.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Account/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(user.UserId.ToString(), false);

                    // Redirect based on user type
                    switch (user.UserType)
                    {
                        case UserType.Admin:
                            return RedirectToAction("AdminView", "Admin");
                        case UserType.Customer:
                            return RedirectToAction("CustomerView", "Customer");
                        case UserType.Courier:
                            return RedirectToAction("CourierView", "Courier");
                        case UserType.Vendor:
                            return RedirectToAction("VendorView", "Vendor");
                        default:
                            ModelState.AddModelError("", "Invalid user type.");
                            return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }
            return View(model);
        }

        // GET: /Account/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UserRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Create new user
                var user = new User
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    Password = model.Password,
                    UserType = model.UserType
                };

                // Add user to database
                db.Users.Add(user);
                db.SaveChanges();

                // Redirect to login page
                return RedirectToAction("Login");
            }
            return View(model);
        }

        // GET: /Account/Logout
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }
    }
}

using System;
using System.Linq;
using System.Web.Mvc;
using OnlineRetailStoreV01.Models;

namespace OnlineRetailStoreV01.Controllers
{
    public class CartController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "Customer")]
        public ActionResult Index()
        {
            // Retrieve the user's cart items
            var userId = (int)Session["UserId"];
            var cartItems = db.CartItems.Where(c => c.UserId == userId).ToList();
            return View(cartItems);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToCart(int productId, int quantity)
        {
            // Add the specified quantity of the product to the user's cart
            var userId = (int)Session["UserId"];
            var existingCartItem = db.CartItems.FirstOrDefault(c => c.UserId == userId && c.ProductId == productId);

            if (existingCartItem != null)
            {
                existingCartItem.Quantity += quantity;
            }
            else
            {
                db.CartItems.Add(new CartItem { UserId = userId, ProductId = productId, Quantity = quantity });
            }

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveFromCart(int id)
        {
            // Remove a product from the user's cart
            var cartItem = db.CartItems.Find(id);
            db.CartItems.Remove(cartItem);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PlaceOrder()
        {
            // Place an order for the user
            var userId = (int)Session["UserId"];
            var cartItems = db.CartItems.Where(c => c.UserId == userId).ToList();

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                Items = cartItems
            };

            db.Orders.Add(order);

            foreach (var item in cartItems)
            {
                item.OrderId = order.Id;

                // Decrease the product quantity in the database
                var product = db.Products.Find(item.ProductId);
                if (product != null)
                {
                    product.Quantity -= item.Quantity;
                }
            }

            db.SaveChanges();

            // Clear the user's cart
            db.CartItems.RemoveRange(cartItems);
            db.SaveChanges();

            return RedirectToAction("Index", "Order");
        }
    }
}



using System.Linq;
using System.Web.Mvc;
using OnlineRetailStoreV01.Models;

namespace OnlineRetailStoreV01.Controllers
{
    public class OrderController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "Customer")]
        public ActionResult Index()
        {
            // Retrieve the user's orders
            var userId = (int)Session["UserId"];
            var orders = db.Orders.Where(o => o.UserId == userId).ToList();
            return View(orders);
        }

        public ActionResult Details(int id)
        {
            // View details of a specific order
            var order = db.Orders.Find(id);
            return View(order);
        }
    }
}


using System.Linq;
using System.Web.Mvc;
using OnlineRetailStoreV01.Models;

namespace OnlineRetailStoreV01.Controllers
{
    public class CourierController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "Courier")]
        public ActionResult Index()
        {
            // Retrieve orders assigned to the courier
            var courierId = (int)Session["UserId"];
            var orders = db.Orders.Where(o => o.CourierId == courierId).ToList();
            return View(orders);
        }

        [Authorize(Roles = "Courier")]
        public ActionResult UpdateStatus(int id, OrderStatus status)
        {
            // Update the status of the order
            var order = db.Orders.Find(id);
            if (order != null)
            {
                order.Status = status;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}



using System;
using System.Collections.Generic;

namespace OnlineRetailStoreV01.Models
{
    public enum OrderStatus
    {
        Pending,
        InProgress,
        Delivered,
        Cancelled
    }

    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public int CourierId { get; set; }
        public OrderStatus Status { get; set; }
        public virtual ICollection<CartItem> Items { get; set; }
    }
}


using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using OnlineRetailStoreV01.Models;

namespace OnlineRetailStoreV01.Controllers
{
    public class ProductController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "Vendor")]
        public ActionResult Index()
        {
            var vendorId = (int)Session["UserId"];
            var products = db.Products.Where(p => p.VendorId == vendorId).ToList();
            return View(products);
        }

        [Authorize(Roles = "Vendor")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                var vendorId = (int)Session["UserId"];
                product.VendorId = vendorId;
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        [Authorize(Roles = "Vendor")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        [Authorize(Roles = "Vendor")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}



namespace OnlineRetailStoreV01.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int VendorId { get; set; }
        public virtual Vendor Vendor { get; set; }
    }
}



[HttpPost]
[ValidateAntiForgeryToken]
public ActionResult PlaceOrder()
{
    var userId = (int)Session["UserId"];
    var cartItems = db.CartItems.Where(c => c.UserId == userId).ToList();

    var order = new Order
    {
        UserId = userId,
        OrderDate = DateTime.Now,
        Items = cartItems
    };

    db.Orders.Add(order);

    foreach (var item in cartItems)
    {
        item.OrderId = order.Id;

        // Decrease the product quantity in the database
        var product = db.Products.Find(item.ProductId);
        if (product != null)
        {
            product.Quantity -= item.Quantity;
        }
    }

    db.SaveChanges();

    // Clear the user's cart
    db.CartItems.RemoveRange(cartItems);
    db.SaveChanges();

    return RedirectToAction("Index", "Order");
}



using System.Linq;
using System.Web.Mvc;
using OnlineRetailStoreV01.Models;

namespace OnlineRetailStoreV01.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ManageVendors()
        {
            var vendors = db.Vendors.ToList();
            return View(vendors);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ManageCouriers()
        {
            var couriers = db.Couriers.ToList();
            return View(couriers);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ManageProducts()
        {
            var products = db.Products.ToList();
            return View(products);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ApproveVendor(int id)
        {
            var vendor = db.Vendors.Find(id);
            if (vendor != null)
            {
                vendor.IsApproved = true;
                db.SaveChanges();
            }
            return RedirectToAction("ManageVendors");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ApproveCourier(int id)
        {
            var courier = db.Couriers.Find(id);
            if (courier != null)
            {
                courier.IsApproved = true;
                db.SaveChanges();
            }
            return RedirectToAction("ManageCouriers");
        }
    }
}

[HttpPost]
[ValidateAntiForgeryToken]
public ActionResult PlaceOrder()
{
    var userId = (int)Session["UserId"];
    var cartItems = db.CartItems.Where(c => c.UserId == userId).ToList();

    var order = new Order
    {
        UserId = userId,
        OrderDate = DateTime.Now,
        Items = cartItems
    };

    db.Orders.Add(order);

    foreach (var item in cartItems)
    {
        item.OrderId = order.Id;

        // Decrease the product quantity in the database
        var product = db.Products.Find(item.ProductId);
        if (product != null)
        {
            product.Quantity -= item.Quantity;
        }
    }

    db.SaveChanges();

    // Clear the user's cart
    db.CartItems.RemoveRange(cartItems);
    db.SaveChanges();

    return RedirectToAction("Index", "Order");
}

