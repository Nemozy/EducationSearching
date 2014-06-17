using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EducationSearching.Filters;
using EducationSearching.Models;
using PagedList;
using WebMatrix.WebData;
using System.Net;

namespace EducationSearching.Controllers
{
    //[Authorize(Roles = "User")]
    public class MainController : Controller
    {
        //
        // GET: /Main/
        //[Authorize(Roles = "User")]
        public ActionResult Index(string searchString, string spisokYear, string spisokSemestr, int? page)
        {
            DBDataContext db = new DBDataContext();
            UserProfile user = ПолучитьТекущегоПользователяИУстановитьРоль();

            IQueryable<PredmetForUser> predmets = null;
            if (spisokYear != null && spisokSemestr != null)
            {
                if (string.Equals(ViewBag.UserRole, "Administrator"))
                {
                    predmets = db.PredmetForUser.Where(b => b.spisokYear == spisokYear && b.spisokSemestr == spisokSemestr);
                }
                else//иначе только самого пользователя
                {
                    predmets = db.PredmetForUser.Where(b => b.FIOUserID == user.UserId &&
                        b.spisokYear == spisokYear && b.spisokSemestr == spisokSemestr);
                }

                //сортируем от большего к меньшему
                predmets = predmets.OrderByDescending(b => b.predmetName);
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(predmets.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult AddPredmet()
        {
            ViewBag.Message = "Добавление предмета.";
            ОпределениеСпискаПреподователей();
            ОпределениеСпискаПредметов();
            return View();
        }

        private void ОпределениеСпискаПреподователей()
        {
            DBDataContext db = new DBDataContext();
            IQueryable<UserProfile> users = db.UserProfile.Where(s => (s.FIOShort != "" || s.FIOShort != null));
            List<string> userSrt = new List<string>();
            foreach (UserProfile us in users)
            {
                userSrt.Add(us.UserId + "|" + us.FIOShort);
            }
            ViewBag.userSrt = userSrt;
        }

        private void ОпределениеСпискаПредметов()
        {
            DBDataContext db = new DBDataContext();
            IQueryable<PredmetDB> predmets = db.PredmetDB.Where(s => (s.Disciplina != "" || s.Disciplina != null));
            List<string> predmetSrt = new List<string>();
            foreach (PredmetDB us in predmets)
            {
                predmetSrt.Add(us.Id + "|" + us.Disciplina);
            }
            ViewBag.predmetSrt = predmetSrt;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPredmet(string spisokYear, string spisokSemestr, string FIOUserID, string predmetId, string potok, string kolvoGrupp, 
            string kolvoPodGrupp, string kolvoStudent, string lekcii, string praktikaPlan, string praktikaVsego, string labPlan, string labVsego, 
            string konsultTek, string konsultEkz, string exzamen, string zachet, string rykvoPract, string rykvoDJPpr, string chasSRS, string chasReit, 
            string rykvoAspirant, string vsego, string assistent)
        {
            ОпределениеСпискаПреподователей();
            ОпределениеСпискаПредметов();
            if (spisokYear == null || spisokSemestr == null || FIOUserID == null || predmetId == null)
            {
                ViewBag.MessageErr = "Не заполнены поля курса и преподователя!";
                return View();
            }

            DBDataContext db = new DBDataContext();
            PredmetForUser newPredmet = new PredmetForUser();
            newPredmet.spisokYear = spisokYear;
            newPredmet.spisokSemestr = spisokSemestr;
            newPredmet.FIOUserID = Convert.ToInt32(FIOUserID);
            UserProfile user = db.UserProfile.FirstOrDefault(u => u.UserId == newPredmet.FIOUserID);
            newPredmet.FIOShort = user.FIOShort;
            newPredmet.predmetId = Convert.ToInt32(predmetId);
            PredmetDB predmetDB = db.PredmetDB.FirstOrDefault(u => u.Id == newPredmet.predmetId);
            newPredmet.predmetName = predmetDB.Disciplina;
            newPredmet.potok = potok;

            newPredmet.kolvoGrupp = kolvoGrupp;
            newPredmet.kolvoPodGrupp = kolvoPodGrupp;
            newPredmet.kolvoStudent = kolvoStudent;
            newPredmet.lekcii = lekcii;
            newPredmet.praktikaPlan = praktikaPlan;
            newPredmet.praktikaVsego = praktikaVsego;
            newPredmet.labPlan = labPlan;
            newPredmet.labVsego = labVsego;
            newPredmet.konsultTek = konsultTek;
            newPredmet.konsultEkz = konsultEkz;
            newPredmet.exzamen = exzamen;
            newPredmet.zachet = zachet;
            newPredmet.rykvoPract = rykvoPract;
            newPredmet.rykvoDJPpr = rykvoDJPpr;
            newPredmet.chasSRS = chasSRS;
            newPredmet.chasReit = chasReit;
            newPredmet.rykvoAspirant = rykvoAspirant;
            newPredmet.vsego = vsego;
            newPredmet.assistent = assistent;
            db.PredmetForUser.InsertOnSubmit(newPredmet);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }

        private UserProfile ПолучитьТекущегоПользователяИУстановитьРоль()
        {
            DBDataContext db = new DBDataContext();
            UserProfile user = db.UserProfile.FirstOrDefault(u => u.UserName.ToLower() == User.Identity.Name.ToLower());
            ViewBag.UserFIO = user.FIOShort;
            ViewBag.UserRole = "Anonymous";
            if (user != null)
            {
                webpages_UsersInRoles userInRoles = db.webpages_UsersInRoles.FirstOrDefault(u => u.UserId == user.UserId);
                if (userInRoles != null)
                {
                    webpages_Roles roles = db.webpages_Roles.FirstOrDefault(u => u.RoleId == userInRoles.RoleId);
                    if (roles != null)
                    {
                        ViewBag.UserRole = roles.RoleName;
                    }
                }
            }
            return user;
        }

        // GET: Review/5
        public ActionResult Review(int? id, bool? saveChangesError = false)
        {
            DBDataContext db = new DBDataContext();
            ПолучитьТекущегоПользователяИУстановитьРоль();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Данная строка была удалена ранее!";
            }

            PredmetForUser predmetForUser = db.PredmetForUser.FirstOrDefault(u => u.Id == id);
            if (predmetForUser == null)
            {
                return HttpNotFound();
            }
            return View(predmetForUser);
        }

        public ActionResult Remove(int id)
        {
            try
            {
                DBDataContext db = new DBDataContext();
                int response = db.ExecuteCommand(@"DELETE FROM PredmetForUser WHERE PredmetForUser.Id={0}", id);
                if (response == 1)
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
                //return RedirectToAction("Remove", new { id = id, saveChangesError = true });
            }
            return RedirectToAction("Index");
        }

        public ActionResult EditRecord(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ПолучитьТекущегоПользователяИУстановитьРоль();
            ОпределениеСпискаПреподователей();
            ОпределениеСпискаПредметов();

            DBDataContext db = new DBDataContext();
            PredmetForUser predmetForUser = db.PredmetForUser.FirstOrDefault(u => u.Id == id);
            if (predmetForUser == null)
            {
                return HttpNotFound();
            }
            return View(predmetForUser);
        }

        // POST: /EditRecord/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRecord([Bind(Include = "Id, spisokYear,  spisokSemestr,  FIOUserID,  predmetId,  potok,  kolvoGrupp,  kolvoPodGrupp,  kolvoStudent,  lekcii,  praktikaPlan,  praktikaVsego,  labPlan,  labVsego, konsultTek,  konsultEkz,  exzamen,  zachet,  rykvoPract,  rykvoDJPpr,  chasSRS,  chasReit, rykvoAspirant,  vsego,  assistent")]PredmetForUser predmetForUser)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ОпределениеСпискаПреподователей();
                    ОпределениеСпискаПредметов();
                    if (predmetForUser.spisokYear == null || predmetForUser.spisokSemestr == null || predmetForUser.FIOUserID == null || predmetForUser.predmetId == null)
                    {
                        ViewBag.MessageErr = "Не заполнены поля курса и преподователя!";
                        return View(predmetForUser);
                    }

                    DBDataContext db = new DBDataContext();
                    PredmetForUser newPredmet = db.PredmetForUser.FirstOrDefault(u => u.Id == predmetForUser.Id);
                    newPredmet.spisokYear = predmetForUser.spisokYear;
                    newPredmet.spisokSemestr = predmetForUser.spisokSemestr;
                    newPredmet.FIOUserID = Convert.ToInt32(predmetForUser.FIOUserID);
                    UserProfile user = db.UserProfile.FirstOrDefault(u => u.UserId == newPredmet.FIOUserID);
                    newPredmet.FIOShort = user.FIOShort;
                    newPredmet.predmetId = Convert.ToInt32(predmetForUser.predmetId);
                    PredmetDB predmetDB = db.PredmetDB.FirstOrDefault(u => u.Id == newPredmet.predmetId);
                    newPredmet.predmetName = predmetDB.Disciplina;
                    newPredmet.potok = predmetForUser.potok;

                    newPredmet.kolvoGrupp = predmetForUser.kolvoGrupp;
                    newPredmet.kolvoPodGrupp = predmetForUser.kolvoPodGrupp;
                    newPredmet.kolvoStudent = predmetForUser.kolvoStudent;
                    newPredmet.lekcii = predmetForUser.lekcii;
                    newPredmet.praktikaPlan = predmetForUser.praktikaPlan;
                    newPredmet.praktikaVsego = predmetForUser.praktikaVsego;
                    newPredmet.labPlan = predmetForUser.labPlan;
                    newPredmet.labVsego = predmetForUser.labVsego;
                    newPredmet.konsultTek = predmetForUser.konsultTek;
                    newPredmet.konsultEkz = predmetForUser.konsultEkz;
                    newPredmet.exzamen = predmetForUser.exzamen;
                    newPredmet.zachet = predmetForUser.zachet;
                    newPredmet.rykvoPract = predmetForUser.rykvoPract;
                    newPredmet.rykvoDJPpr = predmetForUser.rykvoDJPpr;
                    newPredmet.chasSRS = predmetForUser.chasSRS;
                    newPredmet.chasReit = predmetForUser.chasReit;
                    newPredmet.rykvoAspirant = predmetForUser.rykvoAspirant;
                    newPredmet.vsego = predmetForUser.vsego;
                    newPredmet.assistent = predmetForUser.assistent;
                    db.SubmitChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Все поля из выподающих списков должны быть заполнены!");
            }
            return View(predmetForUser);
        }

    }
}