using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicBase;

namespace DynamicObjectTransactions
{
    class Program
    {
        static void Main(string[] args)
        {
            User usr = new User();
            usr.UserId = 12;
            usr.UserName = "Nahk'o";
            usr.FirstName = "N'edim";
            usr.LastName = "Şahin";
            usr.IsActive = true;
            usr.UserType = 14;
            usr.CreationDate = null;

            string script = usr.InsertQuery();
            Console.WriteLine(script);
            Console.ReadKey();
        }
    }

    class User
    {
        dynamic user;
        public User()
        {
            user = new DynamicBaseObject();
            user.SetTable("Users");
            user.SetVirtualKey("UserId");
            user.SetSchema("adbo");
        }

        public int UserId
        {
            get { return user.UserId; }
            set { user.UserId = value; }
        }


        public string UserName
        {
            get { return user.UserName; }
            set { user.UserName = value; }
        }


        public string FirstName
        {
            get { return user.FirstName; }
            set { user.FirstName = value; }
        }

        private string lastName;

        public string LastName
        {
            get { return user.LastName; }
            set { user.LastName = value; }
        }

        public DateTime? CreationDate
        {
            get { return user.CreationDate; }
            set { user.CreationDate = value; }
        }


        public int UserType
        {
            get { return user.UserType; }
            set { user.UserType = value; }
        }

        public bool IsActive
        {
            get { return user.IsActive; }
            set { user.IsActive = value; }
        }

        public string InsertQuery() { return user.InsertQuery(); }
    }
}
