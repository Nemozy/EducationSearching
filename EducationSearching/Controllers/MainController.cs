using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EducationSearching.Controllers
{
    [Authorize(Roles = "User")]
    public class MainController : Controller
    {
        //
        // GET: /Main/
        [Authorize(Roles = "User")]
        public ActionResult Index()
        {
            ViewBag.Message = "Главная. Вы зашли...";

            return View();
        }
        
    }
}
