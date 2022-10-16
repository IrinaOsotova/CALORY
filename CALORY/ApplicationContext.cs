using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CALORY
{
    public class User
    {
        public int id { get; set; }
        public string name { get; set; }
        public string login { get; set; }
        public string password { get; set; }

    }
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseMySQL("Server=localhost;Database=calorie;Uid=root;Pwd=root;");
            optionsBuilder.UseSqlServer("workstation id=Calories.mssql.somee.com;packet size=4096;user id=AnroMel_SQLLogin_1;pwd=yza3c59w31;data source=Calories.mssql.somee.com;persist security info=False;initial catalog=Calories");
        }
    }
}
