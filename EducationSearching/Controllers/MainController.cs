using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EducationSearching.Filters;
using EducationSearching.Models;

namespace EducationSearching.Controllers
{
    //[Authorize(Roles = "User")]
    public class MainController : Controller
    {
        //
        // GET: /Main/
        //[Authorize(Roles = "User")]
        public ActionResult Index()
        {
            UsersContext db = new UsersContext();
            EducationSearching.Models.UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == User.Identity.Name.ToLower());
            if(user== null)
            {
                ViewBag.UserRole = "Anonymous";
                return View();
            }
            EducationSearching.Models.webpages_UsersInRoles userInRoles = db.userInRoles.Find(user.UserId);
            if (userInRoles == null)
            {
                ViewBag.UserRole = "Anonymous";
                return View();
            }
            EducationSearching.Models.webpages_Roles roles = db.Roles.Find(userInRoles.RoleId);
            if (userInRoles == null)
            {
                ViewBag.UserRole = "Anonymous";
                return View();
            }

            ViewBag.UserRole = roles.RoleName;

            /*if (string.Equals(roles.RoleName, "Administrator"))
            {
                return RedirectToAction("Index", "Home");
            }*/
            //ViewBag.Message = "Главная. Вы зашли...";

            return View();
        }
    }
}
