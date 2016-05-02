// ========================================================================= //
// File Name : User.cs                                                       //
// File Date : 12 April 2016                                                 //
// Author(s) : Michael Collins, Louise McKeown, Alan Rowlands                //
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
        protected string firstname;
        protected string surname;
        protected string email;
        protected string address;
        protected string phoneno;
        protected string dateofbirth;


        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public User()
        : base("user") {
            username    = "";
            firstname   = "";
            surname     = "";
            email       = "";
            address     = "";
            phoneno     = "";
            dateofbirth = "";
        }


        // ----------------------------------------------------------------- //
        // Factory constructor.                                              //
        // ----------------------------------------------------------------- //
        public User(string UserName, string FirstName, string SurName, string Email,
                    string Address, string PhoneNo, string DateOfBirth)
        : base("user") {
            SetUserName(UserName);
            SetFirstName(FirstName);
            SetSurname(SurName);
            SetEmail(Email);
            SetAddress(Address);
            SetPhoneNo(PhoneNo);
            SetDateOfBirth(DateOfBirth);
        }

        // ----------------------------------------------------------------- //
        // Factory constructor.                                              //
        // ----------------------------------------------------------------- //
        public override string Read() {
            string text = "\n User";
            text = text + "\n UserName    = "+username.ToString();
            text = text + "\n FirstName   = "+firstname.ToString();
            text = text + "\n Surname     = "+surname.ToString();
            text = text + "\n Email       = "+email.ToString();
            text = text + "\n Address     = "+address.ToString();
            text = text + "\n PhoneNo     = "+phoneno.ToString();
            text = text + "\n DateOfBirth = "+dateofbirth.ToString();
            return text + "\n";
        }



        // ----------------------------------------------------------------- //
        // Getters and Setters.                                              //
        // ----------------------------------------------------------------- //
        public string GetUserName() { return username; }
        public string GetFirstName() { return firstname; }
        public string GetSurname() { return surname; }
        public string GetEmail() { return email; }
        public string GetAddress() { return address; }
        public string GetPhoneNo() { return phoneno; }
        public string GetDateOfBirth() { return dateofbirth; }

        public void SetUserName(string UserName) { username = UserName; }
        public void SetFirstName(string FirstName) { firstname  = FirstName; }
        public void SetSurname(string Surname) { surname  = Surname; }
        public void SetAddress(string Address) { address = Address; }
        public void SetEmail(string Email) { email = Email; }
        public void SetPhoneNo(string PhoneNo) { phoneno = PhoneNo; }
        public void SetDateOfBirth(string DateOfBirth) { dateofbirth = DateOfBirth; }


        // ----------------------------------------------------------------- //
        // pure virtuals                                                     //
        // ----------------------------------------------------------------- //
        public override bool RegexMatch(Regex regex) {
            if (regex.Match(username).Success) return true;
            if (regex.Match(firstname).Success) return true;
            if (regex.Match(surname).Success) return true;
            if (regex.Match(email).Success) return true;
            if (regex.Match(address).Success) return true;
            if (regex.Match(phoneno).Success) return true;
            if (regex.Match(dateofbirth).Success) return true;
            return false;
        }
    }
}