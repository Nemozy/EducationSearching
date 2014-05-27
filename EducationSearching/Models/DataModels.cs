using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace EducationSearching.Models
{
    public class DataContext : System.Data.Entity.DbContext
    {
        public DataContext()
            : base("DefaultConnection")//("DefaultConnection")
        {
        }
        public DbSet<PredmetDB> PredmetDB { get; set; } // добавляем таблицу ролей
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
}
