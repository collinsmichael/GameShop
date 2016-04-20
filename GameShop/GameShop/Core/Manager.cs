using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop
{
    [Serializable]
    public class Manager : User
    {
        public Manager(string username, string password, string firstname, string surname, string email, string address, string phoneno, string dateofbirth)
        : base(username, password, firstname, surname, email, address, phoneno, dateofbirth)
        { }


        public Manager()
        : base()
        {
        }

    }
}
