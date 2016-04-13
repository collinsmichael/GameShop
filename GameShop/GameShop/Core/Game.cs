// ========================================================================= //
// File Name : Game.cs                                                       //
// File Date : 12 April 2016                                                 //
// Author(s) : Michael Collins, Louise McKeown, Alan Redding                 //
// File Info : The Game class is at the early stages of work in progress.    //
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
    [Serializable]
    public class Game : Entity {
        protected string title;
        protected string genre;
        protected string info;


        // ----------------------------------------------------------------- //
        // Getters and Setters.                                              //
        // ----------------------------------------------------------------- //
        public string GetTitle()             { return title;  }
        public string GetGenre()             { return genre;  }
        public string GetInfo()              { return info;   }
        public void   SetTitle(string Title) { title = Title; }
        public void   SetGenre(string Genre) { genre = Genre; }
        public void   SetInfo(string Info)   { info  = Info;  }


        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public Game()
        : base("game") {
            title = "";
            genre = "";
            info  = "";
        }


        // ----------------------------------------------------------------- //
        // Factory constructor.                                              //
        // ----------------------------------------------------------------- //
        public Game(string Title, string Genre, string Info)
        : base("game") {
            title = Title;
            genre = Genre;
            info  = Info;
        }
    }
}