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
        private DBDataContext db = new DBDataContext();

        //
        // GET: /AdminPanel/
        private bool redirectNonAdmin()
        {
            bool outRes = true;
            UserProfile user = db.UserProfile.FirstOrDefault(u => u.UserName.ToLower() == User.Identity.Name.ToLower());
            if (user == null)
            {
                ViewBag.UserRole = "Anonymous";
                return true;
            }
            userInRoles userInRoles = db.userInRoles.FirstOrDefault(u => u.UserId == user.UserId);
            if (userInRoles == null)
            {
                ViewBag.UserRole = "Anonymous";
                return true;
            }
            webpages_Roles roles = db.webpages_Roles.FirstOrDefault(u => u.RoleId == userInRoles.RoleId);
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

            PredmetDB predmetdb = db.PredmetDB.First(u => u.Id == id);
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
                db.PredmetDB.InsertOnSubmit(predmetdb);
                db.SubmitChanges();
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

            PredmetDB predmetdb = db.PredmetDB.First(u => u.Id == id);
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
                //db.Entry(predmetdb).State = EntityState.Modified;

                PredmetDB predmetdbEdit = db.PredmetDB.First(u => u.Id == predmetdb.Id);
                predmetdbEdit.Disciplina = predmetdb.Disciplina;
                db.SubmitChanges();
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

            PredmetDB predmetdb = db.PredmetDB.First(u => u.Id == id);
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

            PredmetDB predmetdb = db.PredmetDB.First(u => u.Id == id);
            db.PredmetDB.DeleteOnSubmit(predmetdb);
            db.SubmitChanges();
            return RedirectToAction("Index_Predmet");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}