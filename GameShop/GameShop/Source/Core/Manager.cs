// ========================================================================= //
// File Name : Manager.cs                                                    //
// File Date : 12 April 2016                                                 //
// Author(s) : Michael Collins, Louise McKeown, Alan Rowlands                //
// File Info : The manager class.                                            //
// ========================================================================= //

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop {
    [Serializable]
    public class Manager : Staff {
        public Manager(string StaffId, string username, string password, string firstname, string surname,
                       string email, string address, string phoneno, string dateofbirth)
        : base(StaffId, username, password, firstname, surname, email, address, phoneno, dateofbirth)
        { }


        public Manager()
        : base()
        { }
    }
}