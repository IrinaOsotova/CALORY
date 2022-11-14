
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CALORY
{
    public class User
    {
        [Key]
        public Int16 id { get; set; }
        public string login { get; set; }
        public string name { get; set; }
        public string password { get; set; }
        public byte male { get; set; }
        public DateTime Birth { get; set; }
        public byte age { get; set; }
        public byte growth { get; set; }
        public byte weight { get; set; }
        public byte activity { get; set; }
        public byte goal { get; set; }
        public Int16 rsk { get; set; }
        
    }


    public class Meal
    {
        [Key]
        public Int16 code { get; set; }
        public DateTime? data { get; set; }
        public string? ration { get; set; }
        public string? food { get; set; }
        public Int16 gram { get; set; }
        public Int16 kkal { get; set; }
        public string? loginUser { get; set; }

    }
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Meal> Meal { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseMySQL("Server=localhost;Database=calorie;Uid=root;Pwd=root;");
            optionsBuilder.UseSqlServer("workstation id=Calories.mssql.somee.com;packet size=4096;user id=AnroMel_SQLLogin_1;pwd=yza3c59w31;data source=Calories.mssql.somee.com;persist security info=False;initial catalog=Calories");
        }
    }
}

