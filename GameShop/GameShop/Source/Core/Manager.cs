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

        public override string Read() {
            string text = "\n Manager";
            text = text + "\n StaffId     = "+staffid.ToString();
            text = text + "\n UserName    = "+username.ToString();
            text = text + "\n FirstName   = "+firstname.ToString();
            text = text + "\n Surname     = "+surname.ToString();
            text = text + "\n Email       = "+email.ToString();
            text = text + "\n Address     = "+address.ToString();
            text = text + "\n PhoneNo     = "+phoneno.ToString();
            text = text + "\n DateOfBirth = "+dateofbirth.ToString();
            return text + "\n";
        }
    }
}