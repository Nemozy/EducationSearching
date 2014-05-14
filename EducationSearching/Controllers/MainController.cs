using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EducationSearching.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class MainController : Controller
    {
        //
        // GET: /Main/
       // [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            ViewBag.Message = "Главная.";

            return View();
        }
        
    }
}
