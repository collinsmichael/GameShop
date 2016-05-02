// ========================================================================= //
// File Name : Member.cs                                                     //
// File Date : 12 April 2016                                                 //
// Author(s) : Michael Collins, Louise McKeown, Alan Rowlands                //
// File Info : The member class.                                             //
// ========================================================================= //

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop {
    [Serializable]
    public class Member : User {
        public Member(string username, string firstname, string surname, string email,
                      string address, string phoneno, string dateofbirth)
        : base(username, firstname, surname, email, address, phoneno, dateofbirth)
        { }


        public Member()
        : base()
        { }
    }
}
