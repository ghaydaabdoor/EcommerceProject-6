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














        // GET: CartItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CartItem cartItem = db.CartItems.Find(id);
            if (cartItem == null)
            {
                return HttpNotFound();
            }
            return View(cartItem);
        }

        // GET: CartItems/Create
        public ActionResult Create()
        {
            ViewBag.CartId = new SelectList(db.Carts, "CartId", "CartId");
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName");
            return View();
        }

        // POST: CartItems/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CartItemId,CartId,ProductId,Quantity")] CartItem cartItem)
        {
            if (ModelState.IsValid)
            {
                db.CartItems.Add(cartItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CartId = new SelectList(db.Carts, "CartId", "CartId", cartItem.CartId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", cartItem.ProductId);
            return View(cartItem);
        }

        // GET: CartItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CartItem cartItem = db.CartItems.Find(id);
            if (cartItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.CartId = new SelectList(db.Carts, "CartId", "CartId", cartItem.CartId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", cartItem.ProductId);
            return View(cartItem);
        }

        // POST: CartItems/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CartItemId,CartId,ProductId,Quantity")] CartItem cartItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cartItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CartId = new SelectList(db.Carts, "CartId", "CartId", cartItem.CartId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", cartItem.ProductId);
            return View(cartItem);
        }

        // GET: CartItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CartItem cartItem = db.CartItems.Find(id);
            if (cartItem == null)
            {
                return HttpNotFound();
            }
            return View(cartItem);
        }

        // POST: CartItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CartItem cartItem = db.CartItems.Find(id);
            db.CartItems.Remove(cartItem);
            db.SaveChanges();
            return RedirectToAction("Index");
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
