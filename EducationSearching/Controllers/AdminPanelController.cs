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
    public class AdminPanelController : Controller
    {
        private DataContext db = new DataContext();

        //
        // GET: /AdminPanel/
        private bool redirectNonAdmin()
        {
            bool outRes = true;
            UsersContext db = new UsersContext();
            EducationSearching.Models.UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == User.Identity.Name.ToLower());
            if (user == null)
            {
                ViewBag.UserRole = "Anonymous";
                return true;
            }
            EducationSearching.Models.webpages_UsersInRoles userInRoles = db.userInRoles.Find(user.UserId);
            if (userInRoles == null)
            {
                ViewBag.UserRole = "Anonymous";
                return true;
            }
            EducationSearching.Models.webpages_Roles roles = db.Roles.Find(userInRoles.RoleId);
            if (userInRoles == null)
            {
                ViewBag.UserRole = "Anonymous";
                return true;
            }

            if (!string.Equals(roles.RoleName, "Administrator"))
            {
                outRes = false;
            }

            return outRes;
        }

        public ActionResult Index_Predmet()
        {
            if (!redirectNonAdmin())
            {
                return RedirectToAction("Index", "Main");
            }
            return View(db.PredmetDB.ToList());
        }

        public ActionResult Index()
        {
            if (!redirectNonAdmin())
            {
                return RedirectToAction("Index", "Main");
            }
            return View();
        }

        //
        // GET: /AdminPanel/Details/5

        public ActionResult Details_Predmet(int id = 0)
        {
            if (!redirectNonAdmin())
            {
                return RedirectToAction("Index", "Main");
            }

            PredmetDB predmetdb = db.PredmetDB.Find(id);
            if (predmetdb == null)
            {
                return HttpNotFound();
            }
            return View(predmetdb);
        }

        //
        // GET: /AdminPanel/Create

        public ActionResult Create_Predmet()
        {
            if (!redirectNonAdmin())
            {
                return RedirectToAction("Index", "Main");
            }

            return View();
        }

        //
        // POST: /AdminPanel/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create_Predmet(PredmetDB predmetdb)
        {
            if (!redirectNonAdmin())
            {
                return RedirectToAction("Index", "Main");
            }

            if (ModelState.IsValid)
            {
                db.PredmetDB.Add(predmetdb);
                db.SaveChanges();
                return RedirectToAction("Index_Predmet");
            }

            return View(predmetdb);
        }

        //
        // GET: /AdminPanel/Edit/5

        public ActionResult Edit_Predmet(int id = 0)
        {
            if (!redirectNonAdmin())
            {
                return RedirectToAction("Index", "Main");
            }

            PredmetDB predmetdb = db.PredmetDB.Find(id);
            if (predmetdb == null)
            {
                return HttpNotFound();
            }
            return View(predmetdb);
        }

        //
        // POST: /AdminPanel/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit_Predmet(PredmetDB predmetdb)
        {
            if (!redirectNonAdmin())
            {
                return RedirectToAction("Index", "Main");
            }

            if (ModelState.IsValid)
            {
                db.Entry(predmetdb).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index_Predmet");
            }
            return View(predmetdb);
        }

        //
        // GET: /AdminPanel/Delete/5

        public ActionResult Delete_Predmet(int id = 0)
        {
            if (!redirectNonAdmin())
            {
                return RedirectToAction("Index", "Main");
            }

            PredmetDB predmetdb = db.PredmetDB.Find(id);
            if (predmetdb == null)
            {
                return HttpNotFound();
            }
            return View(predmetdb);
        }

        //
        // POST: /AdminPanel/Delete/5

        [HttpPost, ActionName("Delete_Predmet")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed_Predmet(int id)
        {
            if (!redirectNonAdmin())
            {
                return RedirectToAction("Index", "Main");
            }

            PredmetDB predmetdb = db.PredmetDB.Find(id);
            db.PredmetDB.Remove(predmetdb);
            db.SaveChanges();
            return RedirectToAction("Index_Predmet");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}