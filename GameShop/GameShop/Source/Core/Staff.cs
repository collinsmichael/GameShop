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
    public class Staff : Entity {
        protected string password;
        protected string username;
        protected string firstname;
        protected string surname;
        protected string email;
        protected string address;
        protected string phoneno;
        protected string dateofbirth;
         
        public Staff(string UserName, string PassWord, string FirstName,
                     string SurName, string Email, string Address, string PhoneNo, string DateOfBirth)
        : base("staff")
        {
            SetPassWord(PassWord);
            SetUserName(UserName);
            SetFirstName(FirstName);
            SetSurname(SurName);
            SetEmail(Email);
            SetAddress(Address);
            SetPhoneNo(PhoneNo);
            SetDateOfBirth(DateOfBirth);
        }

        public Staff()
        : base("staff") {
            password    = "";
            username    = "";
            firstname   = "";
            surname     = "";
            email       = "";
            address     = "";
            phoneno     = "";
            dateofbirth = "";
        }

        public override string Read() {
            string text = "\n Staff";
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
        public string GetPassWord() { return password; }
        public void SetPassWord(string PassWord) { password = PassWord; }
        
        public override bool RegexMatch(Regex regex) {
            if (regex.Match(password).Success) return true;
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
