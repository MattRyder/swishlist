using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Swishlist.Models;
using Microsoft.AspNet.Identity;
using System.Web.Security;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Swishlist.Controllers
{
    public class WishlistItemsController : Controller
    {
        private const string PARAMS_WHITELIST = "ID,Name,ImageUrl,Description,URL,Reference";
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: WishlistItems
        public ActionResult Index()
        {
            return View(db.WishlistItems.ToList());
        }

        // GET: WishlistItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WishlistItem wishlistItem = db.WishlistItems.Find(id);
            if (wishlistItem == null)
            {
                return HttpNotFound();
            }
            return View(wishlistItem);
        }

        public ActionResult Reserve(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            WishlistItem wishlistItem = db.WishlistItems.Find(id);

            if (wishlistItem == null)
            {
                return HttpNotFound();
            }

            if(wishlistItem.ReservingUser != null)
            {
                return HttpNotFound("This item has already been reserved by another friend");
            }

            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);

            string uID = User.Identity.GetUserId();
            wishlistItem.ReservingUser = userManager.FindById(User.Identity.GetUserId());
            db.SaveChanges();

            new EmailSender().SendItemRemovedEmail(wishlistItem);

            return RedirectToAction("Details", "Wishlists", new { id = wishlistItem.Wishlist.ID });
        }

        // GET: WishlistItems/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WishlistItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = PARAMS_WHITELIST)] WishlistItem wishlistItem)
        {
            if (ModelState.IsValid)
            {
                db.WishlistItems.Add(wishlistItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(wishlistItem);
        }

        // GET: WishlistItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WishlistItem wishlistItem = db.WishlistItems.Find(id);
            if (wishlistItem == null)
            {
                return HttpNotFound();
            }
            return View(wishlistItem);
        }

        // POST: WishlistItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = PARAMS_WHITELIST)] WishlistItem wishlistItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(wishlistItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(wishlistItem);
        }

        // GET: WishlistItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WishlistItem wishlistItem = db.WishlistItems.Find(id);
            if (wishlistItem == null)
            {
                return HttpNotFound();
            }
            return View(wishlistItem);
        }

        // POST: WishlistItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            WishlistItem wishlistItem = db.WishlistItems.Find(id);
            db.WishlistItems.Remove(wishlistItem);
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
