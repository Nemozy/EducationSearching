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
            ViewBag.UserFIO = user.FIOShort;
            if (user == null)
            {
                ViewBag.UserRole = "Anonymous";
                return true;
            }
            webpages_UsersInRoles userInRoles = db.webpages_UsersInRoles.FirstOrDefault(u => u.UserId == user.UserId);
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

        public ActionResult Index_Users()
        {
            if (!redirectNonAdmin())
            {
                return RedirectToAction("Index", "Main");
            }
            return View(db.UserProfile.ToList());
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

        public ActionResult Details_Users(int id = 0)
        {
            if (!redirectNonAdmin())
            {
                return RedirectToAction("Index", "Main");
            }

            UserProfile userProfile = db.UserProfile.First(u => u.UserId == id);
            webpages_UsersInRoles usersInRoles = db.webpages_UsersInRoles.First(u => u.UserId == id);
            webpages_Roles roles = db.webpages_Roles.First(u => u.RoleId == usersInRoles.RoleId);
            ViewBag.usersInRoles = usersInRoles;
            if (userProfile == null)
            {
                return HttpNotFound();
            }

            UserProfileR userProfileR = new UserProfileR();
            userProfileR.usersInRoles = new Models.webpages_UsersInRoles();
            userProfileR.usersInRoles.RoleId = usersInRoles.RoleId;
            userProfileR.usersInRoles.UserId = usersInRoles.UserId;
            userProfileR.FIOShort = userProfile.FIOShort;
            userProfileR.UserId = userProfile.UserId;
            userProfileR.UserName = userProfile.UserName;
            userProfileR.roles = new Models.webpages_Roles();
            userProfileR.roles.RoleId = roles.RoleId;
            userProfileR.roles.RoleName = roles.RoleName;
            return View(userProfileR);
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

        public ActionResult Edit_Users(int id = 0)
        {
            if (!redirectNonAdmin())
            {
                return RedirectToAction("Index", "Main");
            }

            ViewBag.dbRoles = db.webpages_Roles;

            UserProfile userProfile = db.UserProfile.First(u => u.UserId == id);
            webpages_UsersInRoles usersInRoles = db.webpages_UsersInRoles.First(u => u.UserId == id);
            webpages_Roles roles = db.webpages_Roles.First(u => u.RoleId == usersInRoles.RoleId);
            if (userProfile == null)
            {
                return HttpNotFound();
            }
            UserProfileR userProfileR = new UserProfileR();
            userProfileR.usersInRoles = new Models.webpages_UsersInRoles();
            userProfileR.FIOShort = userProfile.FIOShort;
            userProfileR.UserId = userProfile.UserId;
            userProfileR.UserName = userProfile.UserName;
            userProfileR.roles = new Models.webpages_Roles();

            userProfileR.usersInRoles.RoleId = usersInRoles.RoleId;
            userProfileR.usersInRoles.UserId = usersInRoles.UserId;
            userProfileR.roles.RoleId = roles.RoleId;
            userProfileR.roles.RoleName = roles.RoleName;
            ViewBag.RoleId = roles.RoleId;
            return View(userProfileR);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit_Users(UserProfileR userProfileR, string role)
        {
            if (!redirectNonAdmin())
            {
                return RedirectToAction("Index", "Main");
            }

            if (ModelState.IsValid)
            {
                //db.Entry(predmetdb).State = EntityState.Modified;

                UserProfile userProfileEdit = db.UserProfile.First(u => u.UserId == userProfileR.UserId);
                userProfileEdit.FIOShort = userProfileR.FIOShort;
                userProfileEdit.UserName = userProfileR.UserName;
                db.SubmitChanges();

                webpages_UsersInRoles usersInRolesNew = new webpages_UsersInRoles();
                webpages_UsersInRoles usersInRolesEdit = db.webpages_UsersInRoles.First(u => u.UserId == userProfileR.UserId);
                usersInRolesNew.RoleId = Convert.ToInt32(role);
                usersInRolesNew.UserId = userProfileR.UserId;

                db.webpages_UsersInRoles.DeleteOnSubmit(usersInRolesEdit);
                db.SubmitChanges();

                db.webpages_UsersInRoles.InsertOnSubmit(usersInRolesNew);
                db.SubmitChanges();

                return RedirectToAction("Index_Users");
            }
            return View(userProfileR);
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

        public ActionResult Delete_Users(int id = 0)
        {
            if (!redirectNonAdmin())
            {
                return RedirectToAction("Index", "Main");
            }

            UserProfile userProfile = db.UserProfile.First(u => u.UserId == id);
            if (userProfile == null)
            {
                return HttpNotFound();
            }
            return View(userProfile);
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

        [HttpPost, ActionName("Delete_Users")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed_Users(int id)
        {
            if (!redirectNonAdmin())
            {
                return RedirectToAction("Index", "Main");
            }

            webpages_UsersInRoles usersInRoles = db.webpages_UsersInRoles.First(u => u.UserId == id);
            db.webpages_UsersInRoles.DeleteOnSubmit(usersInRoles);
            db.SubmitChanges();

            UserProfile userProfile = db.UserProfile.First(u => u.UserId == id);
            db.UserProfile.DeleteOnSubmit(userProfile);
            db.SubmitChanges();
            return RedirectToAction("Index_Users");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}