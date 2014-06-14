using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EducationSearching.Filters;
using EducationSearching.Models;

namespace EducationSearching.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        public ActionResult Index()
        {
            DBDataContext db = new DBDataContext();
            UserProfile user = db.UserProfile.FirstOrDefault(u => u.UserName.ToLower() == User.Identity.Name.ToLower());
            ViewBag.UserFIO = user.FIOShort;
            if (user == null)
            {
                ViewBag.UserRole = "Anonymous";
                return View();
            }
            userInRoles userInRoles = db.userInRoles.FirstOrDefault(u => u.UserId == user.UserId);
            if (userInRoles == null)
            {
                ViewBag.UserRole = "Anonymous";
                return View();
            }
            webpages_Roles roles = db.webpages_Roles.FirstOrDefault(u => u.RoleId == userInRoles.RoleId);
            if (userInRoles == null)
            {
                ViewBag.UserRole = "Anonymous";
                return View();
            }

            if (!string.Equals(roles.RoleName, "Administrator"))
            {
                return RedirectToAction("Index", "Main");
            }
            return View();
        }

    }
}
