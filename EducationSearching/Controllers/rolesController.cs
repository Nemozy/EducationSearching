using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EducationSearching.Models;

namespace EducationSearching.Controllers
{
    public class rolesController : Controller
    {
        private UsersContext db = new UsersContext();

        //
        // GET: /roles/

        public ActionResult Index()
        {
            return View(db.Roles.ToList());
        }

        //
        // GET: /roles/Details/5

        public ActionResult Details(int id = 0)
        {
            EducationSearching.Models.webpages_Roles roles = db.Roles.Find(id);
            if (roles == null)
            {
                return HttpNotFound();
            }
            return View(roles);
        }

        //
        // GET: /roles/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /roles/Create

        [HttpPost]
        public ActionResult Create(EducationSearching.Models.webpages_Roles roles)
        {
            if (ModelState.IsValid)
            {
                db.Roles.Add(roles);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(roles);
        }

        //
        // GET: /roles/Edit/5

        public ActionResult Edit(int id = 0)
        {
            EducationSearching.Models.webpages_Roles roles = db.Roles.Find(id);
            if (roles == null)
            {
                return HttpNotFound();
            }
            return View(roles);
        }

        //
        // POST: /roles/Edit/5

        [HttpPost]
        public ActionResult Edit(webpages_Roles roles)
        {
            if (ModelState.IsValid)
            {
                db.Entry(roles).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(roles);
        }

        //
        // GET: /roles/Delete/5

        public ActionResult Delete(int id = 0)
        {
            EducationSearching.Models.webpages_Roles roles = db.Roles.Find(id);
            if (roles == null)
            {
                return HttpNotFound();
            }
            return View(roles);
        }

        //
        // POST: /roles/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            EducationSearching.Models.webpages_Roles roles = db.Roles.Find(id);
            db.Roles.Remove(roles);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}