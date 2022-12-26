using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryConnectDB
{
    public  interface IConnectBD
    {
        void AddMealToBD(Repast repast);
        void AddUserToBD(User user);
        void RemoveMealfromDB(DateTime dateTime, string Login, string namefood);
        List<Repast> TakeMealFromBD(string Login, DateTime dateTime, string ration);
        User FirstOrDefault(string Login);
        User FirstOrDefaultLoginAndPassword(string Login, string password);
        Repast FirstOrDefault(string Login, DateTime dateTime);
        void DataUpload(string Login, string name, byte growth, byte weight, byte activity, byte goal, byte male, DateTime dateTime, byte age, short rsk);

    }
}
