using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;

namespace LibraryConnectingDB
{
    public class ConnectingBD : IConnectDB
    {
        public void AddMealToBD(Repast repast)
        {
            var db = new ConnectBD();
            db.Meal.Add(repast);
            db.SaveChanges();
        }
        public void AddUserToBD(User user)
        {
            var db = new ConnectBD();
            db.Users.Add(user);
            db.SaveChanges();
        }
        public void RemoveMealfromDB(DateTime dateTime, string Login, string namefood)
        {
            var db = new ConnectBD();
            db.Meal.RemoveRange(db.Meal.Where(x => x.day == dateTime && x.loginUser == Login && x.name == namefood));
            db.SaveChanges();
        }
        public List<Repast> TakeMealFromBD(string Login, DateTime dateTime, string ration)
        {
            var db = new ConnectBD();
            List<Repast> TakeMeal = db.Meal.Where(x => x.loginUser == Login && x.day == dateTime && x.ration == ration).ToList();
            return TakeMeal;
        }
        public User FirstOrDefault(string Login)
        {
            var db = new ConnectBD();
            User user = db.Users.FirstOrDefault(x => x.login == Login);
            return user;
        }
        public User FirstOrDefaultLoginAndPassword(string Login, string password)
        {
            var db = new ConnectBD();
            User user = db.Users.FirstOrDefault(item => item.login == Login && item.password == password);
            return user;
        }
        public Repast? FirstOrDefault(string Login, DateTime dateTime)
        {
            var db = new ConnectBD();            
            return db.Meal.FirstOrDefault(x => x.loginUser == Login && x.day == dateTime.Date);
        }
        public void DataUpload(string Login, string name, byte growth, byte weight, byte activity, byte goal, byte male, DateTime dateTime, byte age, short rsk)
        {
            var db = new ConnectBD();
            User user = db.Users.FirstOrDefault(x => x.login == Login);
            user.name = name;
            user.growth = growth;
            user.weight = weight;
            user.activity = activity;
            user.goal = goal;
            user.male = male;
            user.Birth = dateTime;
            user.age = age;
            user.rsk = rsk;
            db.SaveChanges();
        }

    }
    public class User
    {
        [Key]
        public Int16 id { get; set; }
        public string? login { get; set; }
        public string? name { get; set; }
        public string? password { get; set; }
        public byte male { get; set; }
        public DateTime Birth { get; set; }
        public byte age { get; set; }
        public byte growth { get; set; }
        public byte weight { get; set; }
        public byte activity { get; set; }
        public byte goal { get; set; }
        public Int16 rsk { get; set; }

    }
    public class Repast// : IEnumerator<Repast>
    {
        [Key]
        public Int16 code { get; set; }
        public DateTime? day { get; set; }
        public string? ration { get; set; }
        public string? name { get; set; }
        public double kkal { get; set; }
        public double ugl { get; set; }
        public double fats { get; set; }
        public double bel { get; set; }
        private static Int16 gramm = 100;
        public string? loginUser { get; set; }
        public Int16 gram
        {
            get
            {
                return gramm;
            }
            set
            {
                if (value != 0)
                    gramm = value;
            }
        }

        //public Repast Current => throw new NotImplementedException();

        //object IEnumerator.Current => throw new NotImplementedException();

        //public DbEnumerator GetEnumerator()
        // {  
        //    return new DbEnumerator((System.Data.IDataReader)this);
        // }
        public void Recalculate()
        {
            kkal = Math.Round(kkal * gramm / 100, 2);
            bel = Math.Round(bel * gramm / 100, 2);
            ugl = Math.Round(ugl * gramm / 100, 2);
            fats = Math.Round(fats * gramm / 100, 2);
        }

        public Repast Copy()
        {
            return new Repast(name, gramm.ToString(), kkal.ToString(), bel.ToString(), fats.ToString(), ugl.ToString());
        }
        public override string ToString()
        {
            return name;
        }
        public string ToStringFull()
        {
            return name + " " + gram + " г. " + " - " + kkal + " ккал., " + bel + " г. бел., " + fats + " г. жир., " + ugl + " г. угл.";
        }

        public bool MoveNext()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Repast(string _name, string _gram, string _kkal, string _bel, string _fat, string _ugl)
        {
            name = _name;
            kkal = double.Parse(_kkal);
            bel = double.Parse(_bel);
            fats = double.Parse(_fat);
            ugl = double.Parse(_ugl);
            gramm = Int16.Parse(_gram);
        }
        public Repast() { }
    }

    public class ConnectBD : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Repast> Meal { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("workstation id=Calories.mssql.somee.com;packet size=4096;user id=AnroMel_SQLLogin_1;pwd=yza3c59w31;data source=Calories.mssql.somee.com;persist security info=False;initial catalog=Calories; TrustServerCertificate = true");
        }
    }
}
