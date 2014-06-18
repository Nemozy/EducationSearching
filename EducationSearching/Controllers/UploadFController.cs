using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EducationSearching.Models;
using EducationSearching.ClassContent;
using System.Net;

namespace EducationSearching.Controllers
{
    public class UploadFController : Controller
    {
        //
        // GET: /Upload/
        DBDataContext _DB = new DBDataContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UploadFile()
        {
            ОпределениеСпискаПредметов();
            ViewData["Success"] = "";
            return View();
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

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UploadFile(string Name, string predmetId)
        {
            ОпределениеСпискаПредметов();
            if (predmetId != null)//значит не выбран предмет!
            {
                try
                {
                    //1
                    string SPfilePath = Server.MapPath("~/Upload/");
                    string filepath = SPfilePath + System.IO.Path.GetFileName(Request.Files["file1"].FileName);
                    string ContentType = "";
                    int fileLen;
                    byte[] fileBytes;
                    byte[] Data;

                    int IdMax1 = 0;
                    if (filepath.Contains("."))
                    {
                        Request.Files["file1"].SaveAs(filepath);
                        ContentType = Request.Files["file1"].ContentType;
                        fileLen = Request.Files["file1"].ContentLength;
                        fileBytes = new byte[fileLen];
                        Request.Files["file1"].InputStream.Read(fileBytes, 0, fileBytes.Length);
                        Data = fileBytes;
                        string str1 = Request.Params["AboutFile1"].ToString();
                        _DB.ExecuteCommand("insert into Files (FileData, MimeType, Name, AboutFile) VALUES ({0}, {1}, {2}, N'" + str1 + "')", Data, Request.Files["file1"].ContentType, Request.Files["file1"].FileName);
                        IdMax1 = (from a in _DB.Files select a.Id).Max();
                    }
                    //2
                    SPfilePath = Server.MapPath("~/Upload/");
                    filepath = SPfilePath + System.IO.Path.GetFileName(Request.Files["file2"].FileName);
                    int IdMax2 = 0;
                    if (filepath.Contains("."))
                    {
                        Request.Files["file2"].SaveAs(filepath);
                        ContentType = Request.Files["file2"].ContentType;
                        fileLen = Request.Files["file2"].ContentLength;
                        fileBytes = new byte[fileLen];
                        Request.Files["file2"].InputStream.Read(fileBytes, 0, fileBytes.Length);
                        Data = fileBytes;

                        _DB.ExecuteCommand("insert into Files (FileData, MimeType, Name, AboutFile) VALUES ({0}, {1}, {2}, N'" + Request.Params["AboutFile2"].ToString() + "')", Data, Request.Files["file2"].ContentType, Request.Files["file2"].FileName);
                        IdMax2 = (from a in _DB.Files select a.Id).Max();
                    }

                    //3
                    SPfilePath = Server.MapPath("~/Upload/");
                    filepath = SPfilePath + System.IO.Path.GetFileName(Request.Files["file3"].FileName);
                    int IdMax3 = 0;
                    if (filepath.Contains("."))
                    {
                        Request.Files["file3"].SaveAs(filepath);
                        ContentType = Request.Files["file3"].ContentType;
                        fileLen = Request.Files["file3"].ContentLength;
                        fileBytes = new byte[fileLen];
                        Request.Files["file3"].InputStream.Read(fileBytes, 0, fileBytes.Length);
                        Data = fileBytes;

                        _DB.ExecuteCommand("insert into Files (FileData, MimeType, Name, AboutFile) VALUES ({0}, {1}, {2}, N'" + Request.Params["AboutFile3"].ToString() + "')", Data, Request.Files["file3"].ContentType, Request.Files["file3"].FileName);
                        IdMax3 = (from a in _DB.Files select a.Id).Max();
                    }

                    DBDataContext db = new DBDataContext();
                    UserProfile user = db.UserProfile.FirstOrDefault(u => u.UserName.ToLower() == User.Identity.Name.ToLower());

                    _DB.ExecuteCommand("insert into fileContaner (whoUserId, data, programId, scanId, docsId, predmetId) VALUES ({0}, {1}, {2}, {3}, {4}, {5})", user.UserId, DateTime.Now, IdMax1, IdMax2, IdMax3, predmetId);
                    ViewData["Success"] = "Success";
                }
                catch (Exception e)
                {
                    ViewData["Success"] = "Upload Failed";
                }
            }
            return View();
        }

        public ActionResult Downloads(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DBDataContext db = new DBDataContext();
            Dictionary<string, API_DB.fileLoad> items = new Dictionary<string, API_DB.fileLoad>();
            foreach (var file in db.fileContaner.Where(s => s.predmetId == id))
            {
                API_DB.fileLoad n = new API_DB.fileLoad();

                //EducationSearching.Files filea = db.Files.FirstOrDefault(u => u.Id == file.programId);
                EducationSearching.Files filea = new EducationSearching.Files();
                filea = db.Files.FirstOrDefault(u => u.Id == file.programId);
                if(filea == null)
                {
                    filea = db.Files.FirstOrDefault(u => u.Id == file.docsId);
                    if(filea == null)
                    {
                        filea = db.Files.FirstOrDefault(u => u.Id == file.scanId);
                    }
                }

                if (filea != null)
                {
                    n.program = filea.Name;

                    n.aboutprogram = filea.AboutFile;

                    if (file.scanId != 0)
                    {
                        filea = db.Files.FirstOrDefault(u => u.Id == file.scanId);
                        n.scan = filea.Name;
                        n.aboutscan = filea.AboutFile;
                    }


                    if (file.docsId != 0)
                    {
                        filea = db.Files.FirstOrDefault(u => u.Id == file.docsId);
                        n.docs = filea.Name;
                        n.aboutdocs = filea.AboutFile;
                    }



                    n.Id = file.Id;

                    DBDataContext dbs = new DBDataContext();
                    UserProfile user = dbs.UserProfile.FirstOrDefault(u => u.UserName.ToLower() == User.Identity.Name.ToLower());

                    n.whoUser = user.UserName;

                    n.data = file.data;

                    n.docsId = file.docsId;
                    n.programId = file.programId;
                    n.scanId = file.scanId;


                    items.Add(file.Id.ToString(), n);
                }
            }
            return View(items);
        }

        public ActionResult DownloadFile(string Id)
        {
            DBDataContext db = new DBDataContext();

            int IdCur = Convert.ToInt32(Id);
            EducationSearching.Files fFile = db.Files.FirstOrDefault(u => u.Id == IdCur);
            FileContentResult tr = new FileContentResult(fFile.FileData.ToArray(), fFile.MimeType);
            tr.FileDownloadName = fFile.Name;
            return tr;
        }

        public ActionResult DeleteFiles(string Id)
        {
            DBDataContext db = new DBDataContext();
            int IdCur = Convert.ToInt32(Id);
            fileContaner fd = db.fileContaner.FirstOrDefault(u => u.Id == IdCur);
            int? predmetId = fd.predmetId;
            db.ExecuteCommand("DELETE FROM Files WHERE Id=" + fd.docsId.ToString());
            db.ExecuteCommand("DELETE FROM Files WHERE Id=" + fd.programId.ToString());
            db.ExecuteCommand("DELETE FROM Files WHERE Id=" + fd.scanId.ToString());

            db.ExecuteCommand("DELETE FROM fileContaner WHERE Id=" + Id);
            return RedirectToAction("Downloads", new { id = predmetId });
        }
    }
}
