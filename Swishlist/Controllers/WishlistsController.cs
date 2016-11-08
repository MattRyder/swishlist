using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Swishlist.Models;
using Swishlist.Models.Extensions;
using HtmlAgilityPack;
using Vereyon.Web;
using Microsoft.AspNet.Identity;

namespace Swishlist.Controllers
{
    public class WishlistsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private const string PARAMS_WHITELIST = "ID,Name,Description,WishlistToken";

        // GET: Wishlists
        [Authorize]
        public ActionResult Index()
        {
            string userID = User.Identity.GetUserId<string>();
            return View(db.Wishlists.Where(w => w.UserID.Equals(userID)).ToList());
        }

        // GET: Wishlists/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wishlist wishlist = db.Wishlists.Find(id);

            if(wishlist.UserID.Equals(User.Identity.GetUserId()))
            {
                FlashMessage.Danger("You can't view your own Wishlist!");
                return RedirectToAction("Index", "Home");
            }


            AmazonRegistry registry = new AmazonRegistry(wishlist);
            if(registry.Parse())
            {
                db.SaveChanges();
            }

            if (wishlist == null)
            {
                return HttpNotFound();
            }
            return View(wishlist);
        }

        // GET: Wishlists/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Wishlists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = PARAMS_WHITELIST)] Wishlist wishlist)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser currentUser = User.Identity.GetCurrentUser();

                if (currentUser != null)
                {
                    wishlist.UserID = User.Identity.GetUserId();
                    db.Wishlists.Add(wishlist);
                    db.SaveChanges();

                    FlashMessage.Confirmation("Successfully registered your Wishlist");
                } else
                {
                    FlashMessage.Danger("Not Logged In", "You must be logged in before creating a Wishlist");
                }

                return RedirectToAction("Index");
            }

            return View(wishlist);
        }

        // GET: Wishlists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wishlist wishlist = db.Wishlists.Find(id);
            if (wishlist == null)
            {
                return HttpNotFound();
            }
            return View(wishlist);
        }

        // POST: Wishlists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = PARAMS_WHITELIST)] Wishlist wishlist)
        {
            if (ModelState.IsValid)
            {
                db.Entry(wishlist).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(wishlist);
        }

        // GET: Wishlists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wishlist wishlist = db.Wishlists.Find(id);
            if (wishlist == null)
            {
                return HttpNotFound();
            }
            return View(wishlist);
        }

        // POST: Wishlists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Wishlist wishlist = db.Wishlists.Find(id);
            db.Wishlists.Remove(wishlist);
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
