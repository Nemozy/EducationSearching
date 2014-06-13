using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EducationSearching.Filters;
using EducationSearching.Models;
using System.Web.Mvc;

namespace EducationSearching.ClassContent
{
    public class API_DB
    {
        static public void WriteArchive(String pathProg, String pathScan, String pathDocs)
        {
            DBDataContext db = new DBDataContext();
            db.ExecuteCommand("insert into fileContaner (whoUserId, data, program, scan, docs) VALUES ({0}, {1}, {2}, {3}, {4})", 1);
        }
        

        public struct fileLoad
        {
            public int Id { get; set; }
            public string whoUser { get; set; }
            public DateTime? data { get; set; }
            public string program { get; set; }
            public string scan { get; set; }
            public string docs { get; set; }
            public string aboutprogram { get; set; }
            public string aboutscan { get; set; }
            public string aboutdocs { get; set; }

            public int? programId { get; set; }
            public int? scanId { get; set; }
            public int? docsId { get; set; }
        }
    }
}