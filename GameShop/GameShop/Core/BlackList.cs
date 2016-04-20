// ========================================================================= //
// File Name : BlackList.cs                                                  //
// File Date : 12 April 2016                                                 //
// Author(s) : Michael Collins, Louise McKeown, Alan Redding                 //
// File Info : The BlackList class is responsible for access permissions. No //
//             Controls can be displayed in the form without first getting a //
//             thumbs up from the BlackList.                                 //
//             The BlackList is defined internally and operates invisibly to //
//             all other sub systems with the exception of FormGen.          //
// ========================================================================= //

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace GameShop {
    class BlackList {
        private Dictionary<Type, List<string>> blacklists;

        public BlackList() {
            blacklists = new Dictionary<Type, List<string>>();

            List<string> userblacklist = new List<string> {
                "user.list",         "user.view",         "user.edit",         "user.make",         "user.drop",
                "game.list",         "game.view",         "game.edit",         "game.make",         "game.drop",
                "order.list",        "order.view",        "order.edit",        "order.make",        "order.drop",

                "staff.list",        "staff.view",        "staff.edit",        "staff.make",        "staff.drop",
                "transactions.list", "transactions.view", "transactions.edit", "transactions.make", "transactions.drop",
                "header.page"
            };

            List<string> staffblacklist = new List<string> {
                "staff.list",        "staff.view",        "staff.edit",        "staff.make",        "staff.drop",
                "game.drop",         "order.drop",        "transactions.drop",
                "order.list.delete", "game.list.delete",
                "header.page.staff"
            };

            List<string> managerblacklist = new List<string> { "" };

            blacklists.Add((new User()).GetType(), userblacklist);
            blacklists.Add((new Staff()).GetType(), staffblacklist);
            blacklists.Add((new Manager()).GetType(), managerblacklist);
        }


        // ----------------------------------------------------------------- //
        // Checks to see if the named control is black listed. Each class of //
        // User has their own blacklist associated with them. The logged in  //
        // User is querried to isolate the correct blacklist.                //
        // ----------------------------------------------------------------- //
        public bool IsBlackListed(string control) {
            User logged = Form1.context.GetLogged("user") as User;
            if (logged == null) return false;

            List<string> blacklist = new List<string>();
            if (!blacklists.TryGetValue(logged.GetType(), out blacklist)) {
                return false;
            }

            return blacklist.Contains(control);
        }
    }
}