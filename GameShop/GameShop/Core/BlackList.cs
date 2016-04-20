// ========================================================================= //
// File Name : BlackList.cs                                                  //
// File Date : 12 April 2016                                                 //
// Author(s) : Michael Collins, Louise McKeown, Alan Rowlands                //
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
                "user.drop",        // this will block the entire page from access
                "user.list.delete", // this will block a specific control within the list page
                "user.make",        // this will block the entire page from access
                "user.list.addnew"  // this will block a specific control within the list page
            };

            blacklists.Add((new User()).GetType(), userblacklist);
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