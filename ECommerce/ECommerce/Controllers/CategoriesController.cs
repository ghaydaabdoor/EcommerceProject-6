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
    public class CategoriesController : Controller
    {
        private ECommerceEntities db = new ECommerceEntities();

        // GET: Categories
        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }

        public ActionResult About()
        {
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }

        // GET: Categories/Delete/5

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
