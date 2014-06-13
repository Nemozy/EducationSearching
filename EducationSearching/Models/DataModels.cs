using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Web;

namespace EducationSearching.Models
{
    public class DataContext : System.Data.Entity.DbContext
    {
        public DataContext()
            : base("DefaultConnection")//("DefaultConnection")
        {
        }
        public DbSet<PredmetDB> PredmetDB { get; set; } // добавляем таблицу ролей
        public DbSet<Files> Files { get; set; }
        public DbSet<fileContaner> fileContaner { get; set; }
    }

    // описание таблицы предметов
    [Table("PredmetDB")]
    public class PredmetDB
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Disciplina { get; set; }
    }

    [Table("Files")]
    public class Files
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        public string Name { get; set; }
        [Required(ErrorMessage = "Выберите файл")]
        public byte[] FileData { get; set; }
        public string AboutFile { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string MimeType { get; set; }
    }

    [Table("fileContaner")]
    public class fileContaner
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int whoUserId { get; set; }
        public DateTime data { get; set; }
        public int programId { get; set; }
        public int scanId { get; set; }
        public int docsId { get; set; }
    }
}
