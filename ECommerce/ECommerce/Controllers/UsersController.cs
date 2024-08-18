using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using ECommerce.Models;

namespace ECommerce.Controllers
{
    public class UsersController : Controller
    {
        private ECommerceEntities db = new ECommerceEntities();

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }








        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,Username,UserPassword,Email,FirstName,LastName,CreatedAt")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Login","Users");
            }

            return View(user);
        }



        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            using (var db = new ECommerceEntities())
            {
                var user = db.Users.FirstOrDefault(u => u.Email == email);

                Session["currentUserID"] = user.UserId;
                Session["currentEmail"] = user.Email;
                Session["currentFirstName"] = user.FirstName;
                Session["currentLastName"]=user.LastName;
                Session["currentUserName"]=user.Username;

                if (user == null)
                {
                    ViewBag.EmailError = "The email address doesn't exist.";
                    return View();
                }
                else if (user.UserPassword != password)
                {
                    ViewBag.PasswordError = "Incorrect password.";
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Products"); 
                }
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Categories"); 

        }



        // GET: User/Edit
        public ActionResult Edit()
        {
            int currentUserId = (int)Session["currentUserID"];
            var user = db.Users.Find(currentUserId);

            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        // POST: User/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User model, string currentPassword, string newPassword, string confirmPassword)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = db.Users.Find(model.UserId);

            if (user == null)
            {
                return HttpNotFound();
            }

            user.Username = model.Username;
            user.Email = model.Email;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.city = model.city;
            user.street = model.street;

            if (!string.IsNullOrEmpty(currentPassword) || !string.IsNullOrEmpty(newPassword) || !string.IsNullOrEmpty(confirmPassword))
            {
                if (!VerifyPassword(currentPassword, user.UserPassword))
                {
                    ModelState.AddModelError("", "Current password is incorrect.");
                    return View(model);
                }

                if (newPassword != confirmPassword)
                {
                    ModelState.AddModelError("", "New password and confirm password do not match.");
                    return View(model);
                }

                user.UserPassword = HashPassword(newPassword);
            }

            db.SaveChanges();
            TempData["SuccessMessage"] = "Profile updated successfully!";
            return RedirectToAction("Edit");
        }

        private bool VerifyPassword(string inputPassword, string storedPasswordHash)
        {
            return inputPassword == storedPasswordHash; 
        }

        private string HashPassword(string password)
        {
            return password; 
        }
    























// GET: Users/Delete/5
public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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
