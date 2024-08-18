using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ECommerce.Models;

namespace ECommerce.Controllers
{
    public class CartItemsController : Controller
    {
        private ECommerceEntities db = new ECommerceEntities();

        // GET: CartItems
        public ActionResult Index()
        {
            var userId = (int) Session["currentUserID"];
            var cartItems = db.CartItems.Include(c => c.Cart).Include(c => c.Product).Where(item => item.Cart.UserId == userId);
            return View(cartItems.ToList());
        }


        [HttpPost]
        public ActionResult AddToCart(int ProductId, int Quantity)
        {
            if (Session["currentUserID"] == null)
            {
                return RedirectToAction("Login", "Users");
            }
            else
            {
                var userId = (int)Session["currentUserID"];
                var cart = db.Carts.FirstOrDefault(c => c.UserId == userId);
                if (cart == null)
                {
                    cart = new Cart { UserId = userId };
                    db.Carts.Add(cart);
                    db.SaveChanges();
                }

                var cartItem = db.CartItems.FirstOrDefault(ci => ci.CartId == cart.CartId && ci.ProductId == ProductId);
                if (cartItem == null)
                {
                    cartItem = new CartItem
                    {
                        CartId = cart.CartId,
                        ProductId = ProductId,
                        Quantity = Quantity
                    };
                    db.CartItems.Add(cartItem);
                }
                else
                {
                    cartItem.Quantity += Quantity;
                }

                db.SaveChanges();

                return RedirectToAction("Index", "CartItems");
            }
        }



        // In CartItemsController.cs

        [HttpPost]
        public ActionResult RemoveItem(int cartItemId)
        {
            using (var db = new ECommerceEntities())
            {
                var cartItem = db.CartItems.FirstOrDefault(ci => ci.CartItemId == cartItemId);

                if (cartItem != null)
                {
                    db.CartItems.Remove(cartItem);
                    db.SaveChanges();
                }

                return RedirectToAction("Index", "CartItems");
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }

}
