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
        protected string email;
        protected string address;
        protected string phoneno;


        // ----------------------------------------------------------------- //
        // Getters and Setters.                                              //
        // ----------------------------------------------------------------- //
        public string GetUserName() { return username; }
        public string GetPassWord() { return password; }
        public string GetFirstName() { return firstname; }
        public string GetSurname() { return surname; }
        public string GetEmail() { return email; }
        public string GetAddress() { return address; }
        public string GetPhoneNo() { return phoneno; }

        public bool SetUserName(string UserName) {
            Regex regexusername = new Regex(@"^[a-zA-Z][a-zA-Z0-9_.-]{1,15}");
        	if (regexusername.Match(UserName).Success) username = UserName;
            else MessageBox.Show("'" + UserName + "'\nIs not a valid email username\nOnly upper case, lower case, numeric digits, and _ . - are allowed");
            return regexusername.Match(UserName).Success;
        }

        public void SetPassWord(string PassWord) {
            password = PassWord;
        }

        public bool SetFirstName(string FirstName) {
            Regex regexname = new Regex(@"^[a-zA-Z]([a-zA-Z]|-|'){1,32}");
        	if (regexname.Match(FirstName).Success) firstname = FirstName;
            else MessageBox.Show("'" + FirstName + "'\nIs not a valid name\nOnly upper case, lower case, and hyphens are allowed");
            return regexname.Match(FirstName).Success;
        }

        public bool SetSurname(string Surname) {
            Regex regexname = new Regex(@"^[a-zA-Z]([a-zA-Z]|-|'){1,32}");
        	if (regexname.Match(Surname).Success) surname = Surname;
            else MessageBox.Show("'" + Surname + "'\nIs not a valid name\nOnly upper case, lower case, and hyphens are allowed");
            return regexname.Match(Surname).Success;
        }
        
        public void SetAddress(string Address) {
            address = Address;
        }
        
        public bool SetEmail(string Email) {
            Regex regexmail = new Regex(@"^[a-zA-Z0-9]{1,10}@[a-zA-Z]{1,10}.([a-zA-Z.]{2,5})$");
        	if (regexmail.Match(Email).Success) email = Email;
            else MessageBox.Show("'" + Email + "'\nIs not a valid email address");
            return regexmail.Match(Email).Success;
        }

        public bool SetPhoneNo(string PhoneNo) {
            Regex regexphone = new Regex(@"^[0-9]{1,4}-[0-9-]{1,12}.([0-9]{1,10})$");
        	if (regexphone.Match(PhoneNo).Success) phoneno = PhoneNo;
            else MessageBox.Show("'" + PhoneNo + "'\nIs not a valid phone number");
            return regexphone.Match(PhoneNo).Success;
        }


        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public User()
        : base("user") {
            username  = "";
            password  = "";
            firstname = "";
            surname   = "";
            email     = "";
            address   = "";
            phoneno   = "";
        }


        // ----------------------------------------------------------------- //
        // Factory constructor.                                              //
        // ----------------------------------------------------------------- //
        public User(string UserName, string PassWord,
                    string FirstName, string SurName, string Email,
                    string Address, string PhoneNo)
        : base("user") {
            SetUserName(UserName);
            SetPassWord(PassWord);
            SetFirstName(FirstName);
            SetSurname(SurName);
            SetEmail(Email);
            SetAddress(Address);
            SetPhoneNo(PhoneNo);
        }


        // ----------------------------------------------------------------- //
        // pure virtuals                                                     //
        // ----------------------------------------------------------------- //
        public override bool RegexMatch(Regex regex) {
            if (regex.Match(username).Success) return true;
            if (regex.Match(password).Success) return true;
            if (regex.Match(firstname).Success) return true;
            if (regex.Match(surname).Success) return true;
            if (regex.Match(email).Success) return true;
            if (regex.Match(address).Success) return true;
            if (regex.Match(phoneno).Success) return true;
            return false;
        }
    }
}