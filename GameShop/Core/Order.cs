// ========================================================================= //
// File Name : Order.cs                                                      //
// File Date : 12 April 2016                                                 //
// Author(s) : Michael Collins, Louise McKeown, Alan Redding                 //
// File Info : The order class is responsible for assigning copies of games  //
//             to Member accounts.                                           //
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
    public class Order : Entity {
        protected string orderno;
        protected string username;
        protected string title;
        protected string orderdate;
        protected string returndate;


        // ----------------------------------------------------------------- //
        // Getters and Setters.                                              //
        // ----------------------------------------------------------------- //
        public string GetOrderNo() { return orderno; }
        public string GetUserName() { return username; }
        public string GetTitle() { return title; }
        public string GetOrderDate() { return orderdate; }
        public string GetReturnDate() { return returndate; }
        public void   SetOrderNo(string OrderNo) { orderno = OrderNo; }
        public void   SetUserName(string UserName) { username = UserName; }
        public void   SetTitle(string Title) { title = Title; }
        public void   SetOrderDate(string OrderDate) { orderdate = OrderDate; }
        public void   SetReturnDate(string ReturnDate) { returndate = ReturnDate; }


        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public Order()
        : base("order") {
            orderno    = "";
            username   = "";
            title      = "";
            orderdate  = "";
            returndate = "";
        }


        // ----------------------------------------------------------------- //
        // Factory constructor.                                              //
        // ----------------------------------------------------------------- //
        public Order(string OrderNo, string UserName, string Title,
                     string OrderDate, string ReturnDate)
        : base("order") {
            orderno    = OrderNo;
            username   = UserName;
            title      = Title;
            orderdate  = OrderDate;
            returndate = ReturnDate;
        }


        // ----------------------------------------------------------------- //
        // pure virtuals                                                     //
        // ----------------------------------------------------------------- //
        public override bool RegexMatch(Regex regex) {
            if (regex.Match(orderno).Success) return true;
            if (regex.Match(username).Success) return true;
            if (regex.Match(title).Success) return true;
            if (regex.Match(orderdate).Success) return true;
            if (regex.Match(returndate).Success) return true;
            return false;
        }
    }
}