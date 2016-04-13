// ========================================================================= //
// File Name : User.cs                                                       //
// File Date : 12 April 2016                                                 //
// Author(s) : Michael Collins, Louise McKeown, Alan Redding                 //
// File Info : The User super class is an abstraction that encases Managers, //
//             Members, and Staff. Depending on the User Type the system may //
//             restrict some functionality.                                  //
//             The User class is at the early stages of work in progress.    //
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
    public class User : Entity {
        protected string username;
        protected string password;
        protected string firstname;
        protected string surname;


        // ----------------------------------------------------------------- //
        // Getters and Setters.                                              //
        // ----------------------------------------------------------------- //
        public string GetUserName()                  { return username;       }
        public string GetPassWord()                  { return password;       }
        public string GetFirstName()                 { return firstname;      }
        public string GetSurname()                   { return surname;        }
        public void   SetUserName(string UserName)   { username  = UserName;  }
        public void   SetPassWord(string PassWord)   { password  = PassWord;  }
        public void   SetFirstName(string FirstName) { firstname = FirstName; }
        public void   SetSurname(string Surname)     { surname   = Surname;   }


        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public User()
        : base("user") {
            username  = "";
            password  = "";
            firstname = "";
            surname   = "";
        }


        // ----------------------------------------------------------------- //
        // Factory constructor.                                              //
        // ----------------------------------------------------------------- //
        public User(string UserName, string PassWord, string FirstName,
                    string SurName)
        : base("user") {
            username  = UserName;
            password  = PassWord;
            firstname = FirstName;
            surname   = SurName;
        }


        // ----------------------------------------------------------------- //
        // pure virtuals                                                     //
        // ----------------------------------------------------------------- //
        public override bool RegexMatch(Regex regex) {
            if (regex.Match(username).Success) return true;
            if (regex.Match(password).Success) return true;
            if (regex.Match(firstname).Success) return true;
            if (regex.Match(surname).Success) return true;
            return false;
        }
    }
}