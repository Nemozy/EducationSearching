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
            System.Web.Security.Roles.Providers.Clear();

            System.Configuration.Provider.ProviderBase pr;
            pr.
            <add name="AspNetSqlProfileProvider"
 connectionStringName="ASPNETApplicationServices"
 applicationName="/"
 type="System.Web.Profile.SqlProfileProvider, System.Web, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"/>;


            System.Web.Security.Roles.Providers.Add();
            System.Web.Security.Roles.CreateRole("Member");
            System.Web.Security.Roles.CreateRole("Administrator");
            Response.Write("Done");

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
