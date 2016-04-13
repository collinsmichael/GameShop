// ========================================================================= //
// File Name : Context.cs                                                    //
// File Date : 12 April 2016                                                 //
// Author(s) : Michael Collins, Louise McKeown, Alan Redding                 //
// File Info : The Context class is responsible for maintaining the lists of //
//             users and games. All object access must pass through Context. //
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
    public class Context {
        public Dictionary<string, User> users;
        public Dictionary<string, Game> games;
        private string logged_user;   // user which is currently logged in
        private string selected_user; // currently selected user
        private string selected_game; // currently selected game


        #region userio
        // ----------------------------------------------------------------- //
        // AddUser is used to add a defined user to the user list.           //
        // Once a game has been added it can later be referred to by name.   //
        // ----------------------------------------------------------------- //
        public bool AddUser(string key, User user) {
            if (users.ContainsKey(key)) users.Remove(key);
            users.Add(key, user);
            return true;
        }


        // ----------------------------------------------------------------- //
        // GetUser allows internal access to the contents of the user list.  //
        // To access a user you must provide the username.                   //
        // ----------------------------------------------------------------- //
        public User GetUser(string username) {
            User user = new User();
            if (!users.TryGetValue(username, out user)) {
                //MessageBox.Show("User '" + username + "' was not found!");
                return user;
            }
            return user;
        }


        // ----------------------------------------------------------------- //
        // Returns the primary key of the currently selected user.           //
        // ----------------------------------------------------------------- //
        public string GetSelectedUser() {
            User select_user = GetUser(selected_user);
            if (select_user != null && select_user.GetUserName() == selected_user) {
                return selected_user;
            }

            foreach (KeyValuePair<string, User> user in users) {
                selected_user = user.Key;
                break;
            }
            return selected_user;
        }


        // ----------------------------------------------------------------- //
        // Caches a local copy of the logged in users primary key            //
        // ----------------------------------------------------------------- //
        public void SetLogged(string key) {
            logged_user = key;
        }


        // ----------------------------------------------------------------- //
        // Gets the logged in user                                           //
        // ----------------------------------------------------------------- //
        public Entity GetLogged(string type) {
            return GetUser(Form1.context.logged_user);
        }
        #endregion


        #region gameio
        // ----------------------------------------------------------------- //
        // AddGame is used to add a defined game to the game list.           //
        // Once a user has been added it can later be referred to by name.   //
        // ----------------------------------------------------------------- //
        public bool AddGame(string key, Game game) {
            if (games.ContainsKey(key)) games.Remove(key);
            games.Add(key, game);
            return true;
        }


        // ----------------------------------------------------------------- //
        // GetGame allows internal access to the contents of the game list.  //
        // To access a game you must provide the title.                      //
        // ----------------------------------------------------------------- //
        public Game GetGame(string title) {
            Game game = new Game();
            if (!games.TryGetValue(title, out game)) {
                ///MessageBox.Show("Game '" + title + "' was not found!");
                return game;
            }
            return game;
        }


        // ----------------------------------------------------------------- //
        // Returns the primary key of the currently selected game.           //
        // ----------------------------------------------------------------- //
        public string GetSelectedGame() {
            Game select_game = GetGame(selected_game);
            if (select_game != null && select_game.GetTitle() == selected_game) {
                return selected_game;
            }

            foreach (KeyValuePair<string, Game> game in games) {
                selected_game = game.Key;
                break;
            }
            return selected_game;
        }
        #endregion

        
        #region entityio
        // ----------------------------------------------------------------- //
        // Gets the selected entity based on the given type                  //
        // ----------------------------------------------------------------- //
        public Entity GetSelected(string type) {
            switch (type.ToLower()) {
            case "user": return GetUser(Form1.context.GetSelectedUser());
            case "game": return GetGame(Form1.context.GetSelectedGame());
            }
            return null;
        }


        // ----------------------------------------------------------------- //
        // Sets the selected entity based on the given type                  //
        // ----------------------------------------------------------------- //
        public void SetSelected(string type, string key) {
            switch (type.ToLower()) {
            case "user": selected_user = key; break;
            case "game": selected_game = key; break;
            }
        }
        #endregion


        #region internal
        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public Context() {
            users = new Dictionary<string, User>();
            games = new Dictionary<string, Game>();

            // mockup a logon page
            users = new Dictionary<string, User>();
            users.Add("mike",   new User("mike",   "letmein", "mike",   "collins"));
            users.Add("louise", new User("louise", "letmein", "louise", "mckeown"));
            users.Add("alan",   new User("alan",   "letmein", "alan",   "redding"));

            games = new Dictionary<string, Game>();
            games.Add("mario",     new Game("mario",     "platform", "can mario save the princess?"));
            games.Add("pacman",    new Game("pacman",    "maze",     "help pacman get out of the ghost ridden maze"));
            games.Add("asteroids", new Game("asteroids", "shooter",  "save the universe or die trying"));

            selected_user = "mike";
            selected_game = "pacman";
        }


        // ----------------------------------------------------------------- //
        // Startup prior to runtime                                          //
        // ----------------------------------------------------------------- //
        public void OnLoad() {
            FileStream file;
            BinaryFormatter formatter;
            try {
                file = new FileStream("users.bin", FileMode.Open, FileAccess.Read);
                formatter = new BinaryFormatter();
                users = formatter.Deserialize(file) as Dictionary<string, User>;
                file.Close();
            } catch {
                MessageBox.Show("File not found! 'users.bin'");
            }

            try {
                file = new FileStream("games.bin", FileMode.Open, FileAccess.Read);
                formatter = new BinaryFormatter();
                games = formatter.Deserialize(file) as Dictionary<string, Game>;
                file.Close();
            } catch {
                MessageBox.Show("File not found! 'games.bin'");
            }
        }


        // ----------------------------------------------------------------- //
        // Cleanup prior to shutdown                                         //
        // ----------------------------------------------------------------- //
        public void OnExit() {
            FileStream file;
            BinaryFormatter formatter;

            file = new FileStream("users.bin", FileMode.OpenOrCreate, FileAccess.Write);
            formatter = new BinaryFormatter();
            formatter.Serialize(file, users);
            file.Close();

            file = new FileStream("games.bin", FileMode.OpenOrCreate, FileAccess.Write);
            formatter = new BinaryFormatter();
            formatter.Serialize(file, games);
            file.Close();
        }
        #endregion


    }
}