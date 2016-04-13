// ========================================================================= //
// File Name : Entity.cs                                                     //
// File Date : 12 April 2016                                                 //
// Author(s) : Michael Collins, Louise McKeown, Alan Redding                 //
// File Info : The Entity class is an abstraction that allows the context to //
//             generate forms without regard to the content being presented. //
//             In this manner both games and users can be treated the same   //
//             in terms of generating the forms used to manipulate them.     //
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
    public abstract class Entity {
        protected string typename;


        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public Entity(string TypeName) {
            typename = TypeName;
        }


        // ----------------------------------------------------------------- //
        // pure virtuals                                                     //
        // ----------------------------------------------------------------- //
        public abstract bool RegexMatch(Regex regex);
    }
}