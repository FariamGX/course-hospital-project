using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopterCW
{
    public class CheckUser
    {
        public string login { get; set; }

        public bool IsAdmin { get; }


        public string Status => IsAdmin ? "Admin" : "User";               //лямбда вираз

        public CheckUser(string login, bool isAdmin)
        {
            login = login.Trim();
            IsAdmin = isAdmin;
        }
    }
}
