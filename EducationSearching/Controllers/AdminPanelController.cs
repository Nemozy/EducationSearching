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
    public class AdminPanelController : Controller
    {
        //
        // GET: /Main/
        //[Authorize(Roles = "User")]
        public ActionResult Index()
        {
            DataContext db = new DataContext();
            IEnumerable<EducationSearching.Models.PredmetDB> predmets = db.PredmetDB.AsEnumerable<EducationSearching.Models.PredmetDB>();
            ViewBag.Predmets = predmets;


            return View();
        }
        
    }
}
