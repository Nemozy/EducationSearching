using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EducationSearching.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            DBDataContext db = new DBDataContext();
            UserProfile user = db.UserProfile.FirstOrDefault(u => u.UserName.ToLower() == User.Identity.Name.ToLower());
            
            if (user == null)
            {
                ViewBag.UserRole = "Anonymous";
            }
            else
            {
                ViewBag.UserFIO = user.FIOShort;
                webpages_UsersInRoles userInRoles = db.webpages_UsersInRoles.FirstOrDefault(u => u.UserId == user.UserId);
                if (userInRoles == null)
                {
                    ViewBag.UserRole = "Anonymous";
                }
                else
                {
                    webpages_Roles roles = db.webpages_Roles.FirstOrDefault(u => u.RoleId == userInRoles.RoleId);
                    if (roles == null)
                    {
                        ViewBag.UserRole = "Anonymous";
                    }
                    if (!string.Equals(roles.RoleName, "Administrator"))
                    {
                        ViewBag.UserRole = "Administrator";
                    }
                }
            }

            if (User.Identity.IsAuthenticated && (!(string.IsNullOrEmpty(User.Identity.Name))))
            {
                return RedirectToAction("Index", "Main");
            }
            ViewBag.Message = "";
            
            
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Страница контактов.";

            return View();
        }


    }
}
