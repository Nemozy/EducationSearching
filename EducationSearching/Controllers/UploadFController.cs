using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EducationSearching.Models;
using EducationSearching.ClassContent;

namespace EducationSearching.Controllers
{
    public class UploadFController : Controller
    {
        //
        // GET: /Upload/
        DataContext _DB = new DataContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UploadFile()
        {
            ViewData["Success"] = "";
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UploadFile(string Name)
        {
            try
            {
                //1
                string SPfilePath = Server.MapPath("~/Upload/");
                string filepath = SPfilePath + System.IO.Path.GetFileName(Request.Files["file1"].FileName);
                Request.Files["file1"].SaveAs(filepath);
                string ContentType = Request.Files["file1"].ContentType;
                int fileLen = Request.Files["file1"].ContentLength;
                byte[] fileBytes = new byte[fileLen];
                Request.Files["file1"].InputStream.Read(fileBytes, 0, fileBytes.Length);
                byte[] Data = fileBytes;

                _DB.Database.ExecuteSqlCommand("insert into Files (FileData, MimeType, Name, AboutFile) VALUES ({0}, {1}, {2}, {3})", Data, Request.Files["file1"].ContentType, Request.Files["file1"].FileName, Request.Params["AboutFile1"]);
                int IdMax1 = (from a in _DB.Files select a.Id).Max();

                //2
                SPfilePath = Server.MapPath("~/Upload/");
                filepath = SPfilePath + System.IO.Path.GetFileName(Request.Files["file2"].FileName);
                Request.Files["file2"].SaveAs(filepath);
                ContentType = Request.Files["file2"].ContentType;
                fileLen = Request.Files["file2"].ContentLength;
                fileBytes = new byte[fileLen];
                Request.Files["file2"].InputStream.Read(fileBytes, 0, fileBytes.Length);
                Data = fileBytes;

                _DB.Database.ExecuteSqlCommand("insert into Files (FileData, MimeType, Name, AboutFile) VALUES ({0}, {1}, {2}, {3})", Data, Request.Files["file2"].ContentType, Request.Files["file2"].FileName, Request.Params["AboutFile2"]);
                int IdMax2 = (from a in _DB.Files select a.Id).Max();

                //3
                SPfilePath = Server.MapPath("~/Upload/");
                filepath = SPfilePath + System.IO.Path.GetFileName(Request.Files["file3"].FileName);
                Request.Files["file3"].SaveAs(filepath);
                ContentType = Request.Files["file3"].ContentType;
                fileLen = Request.Files["file3"].ContentLength;
                fileBytes = new byte[fileLen];
                Request.Files["file3"].InputStream.Read(fileBytes, 0, fileBytes.Length);
                Data = fileBytes;

                _DB.Database.ExecuteSqlCommand("insert into Files (FileData, MimeType, Name, AboutFile) VALUES ({0}, {1}, {2}, {3})", Data, Request.Files["file3"].ContentType, Request.Files["file3"].FileName, Request.Params["AboutFile3"]);
                int IdMax3 = (from a in _DB.Files select a.Id).Max();

                UsersContext db = new UsersContext();
                EducationSearching.Models.UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == User.Identity.Name.ToLower());

                _DB.Database.ExecuteSqlCommand("insert into fileContaner (whoUserId, data, programId, scanId, docsId) VALUES ({0}, {1}, {2}, {3}, {4})", user.UserId, DateTime.Now, IdMax1, IdMax2, IdMax3);
                ViewData["Success"] = "Success";
            }
            catch (Exception e)
            {
                ViewData["Success"] = "Upload Failed";
            }
            
            return View();
        }

        public ActionResult Downloads()
        {
            DataContext db = new DataContext();
            DataContext db2 = new DataContext();
            Dictionary<string, EducationSearching.ClassContent.API_DB.fileLoad> items = new Dictionary<string, EducationSearching.ClassContent.API_DB.fileLoad>();
            foreach (var file in db.fileContaner)
            {
                EducationSearching.ClassContent.API_DB.fileLoad n = new EducationSearching.ClassContent.API_DB.fileLoad();

                EducationSearching.Models.Files filea = db2.Files.FirstOrDefault(u => u.Id == file.programId);
                n.program = filea.Name;

                n.aboutprogram = filea.AboutFile;

                filea = db2.Files.FirstOrDefault(u => u.Id == file.scanId);
                n.scan = filea.Name;

                n.aboutscan = filea.AboutFile;

                filea = db2.Files.FirstOrDefault(u => u.Id == file.docsId);
                n.docs = filea.Name;

                n.aboutdocs = filea.AboutFile;

                n.Id = file.Id;

                UsersContext dbs = new UsersContext();
                EducationSearching.Models.UserProfile user = dbs.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == User.Identity.Name.ToLower());

                n.whoUser = user.UserName;

                n.data = file.data;

                n.docsId = file.docsId;
                n.programId = file.programId;
                n.scanId = file.scanId;


                items.Add(file.Id.ToString(), n);
            }
            return View(items);
        }

        public ActionResult DownloadFile(string Id)
        {
            DataContext db = new DataContext();

            int IdCur = Convert.ToInt32(Id);
            EducationSearching.Models.Files File = db.Files.FirstOrDefault(u => u.Id == IdCur);
            FileContentResult tr = new FileContentResult(File.FileData, File.MimeType);
            tr.FileDownloadName = File.Name;
            return tr;
        }

        public ActionResult DeleteFiles(string Id)
        {
            DataContext db = new DataContext();
            int IdCur = Convert.ToInt32(Id);
            EducationSearching.Models.fileContaner fd = db.fileContaner.FirstOrDefault(u => u.Id == IdCur);
            _DB.Database.ExecuteSqlCommand("DELETE FROM Files WHERE Id=" + fd.docsId.ToString());
            _DB.Database.ExecuteSqlCommand("DELETE FROM Files WHERE Id=" + fd.programId.ToString());
            _DB.Database.ExecuteSqlCommand("DELETE FROM Files WHERE Id=" + fd.scanId.ToString());

            _DB.Database.ExecuteSqlCommand("DELETE FROM fileContaner WHERE Id=" + Id);

            return RedirectToAction("Downloads", "UploadF");
        }
    }
}
