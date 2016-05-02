// ========================================================================= //
// File Name : Staff.cs                                                      //
// File Date : 12 April 2016                                                 //
// Author(s) : Michael Collins, Louise McKeown, Alan Rowlands                //
// File Info : The staff class.                                              //
// ========================================================================= //

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameShop {
    [Serializable]
    public class Staff:User {
        public string staffid;
        public string password;
         
        public Staff(string StaffId, string username, string password, string firstname,
                     string surname, string email, string address, string phoneno, string dateofbirth)
        : base(username, firstname, surname, email, address, phoneno, dateofbirth)
        {
            SetPassWord(password);
            SetStaffId(StaffId);
        }

        public Staff()
        : base() { }

        public string GetStaffId() { return staffid; }
        public string GetPassWord() { return password; }
        public void SetStaffId(string StaffId) { staffid = StaffId; }
        public void SetPassWord(string PassWord) { password = PassWord; }
        
        public override bool RegexMatch(Regex regex) {
            if (!regex.Match(staffid).Success) return false;
            if (regex.Match(password).Success) return true;
            return base.RegexMatch(regex);
        }
    }
}
