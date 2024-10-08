﻿using System;
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
    public class ProductsController : Controller
    {
        private ECommerceEntities db = new ECommerceEntities();

        // GET: Products
        public ActionResult Index(int? id)
        {
            var products = db.Products.Include(p => p.Category);

            if (id.HasValue)
            {
                products = products.Where(p => p.CategoryId == id.Value);
            }

            var categories = db.Categories.ToList();

            ViewBag.Categories = categories;
            ViewBag.ProductCount = products.Count();

            return View(products.ToList());
        }


        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
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
