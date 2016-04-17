﻿// ========================================================================= //
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
        public Dictionary<string, User>  users;  // list of all users
        public Dictionary<string, Game>  games;  // list of all games
        public Dictionary<string, Order> orders; // list of all orders
        private string logged_user;              // user which is currently logged in
        private string selected_user;            // currently selected user
        private string selected_game;            // currently selected game
        private string selected_order;           // currently selected order


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
        // Once a game has been added it can later be referred to by name.   //
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

        
        #region orderio
        // ----------------------------------------------------------------- //
        // AddOrder is used to add a defined order to the order list.        //
        // Once an order has been added it can later be referred to by name. //
        // ----------------------------------------------------------------- //
        public bool AddOrder(string key, Order order) {
            if (orders.ContainsKey(key)) orders.Remove(key);
            orders.Add(key, order);
            return true;
        }


        // ----------------------------------------------------------------- //
        // GetOrder allows internal access to the contents of the order list //
        // To access an order you must provide the orderno.                  //
        // ----------------------------------------------------------------- //
        public Order GetOrder(string OrderNo) {
            Order order = new Order();
            if (!orders.TryGetValue(OrderNo, out order)) {
                ///MessageBox.Show("Game '" + title + "' was not found!");
                return order;
            }
            return order;
        }


        // ----------------------------------------------------------------- //
        // Returns the primary key of the currently selected order.          //
        // ----------------------------------------------------------------- //
        public string GetSelectedOrder() {
            Order select_order = GetOrder(selected_order);
            if (select_order != null && select_order.GetOrderNo() == selected_order) {
                return selected_order;
            }

            foreach (KeyValuePair<string, Order> order in orders) {
                selected_order = order.Key;
                break;
            }
            return selected_order;
        }
        #endregion

        
        #region entityio
        // ----------------------------------------------------------------- //
        // Gets the selected entity based on the given type                  //
        // ----------------------------------------------------------------- //
        public Entity GetSelected(string type) {
            switch (type.ToLower()) {
            case "user":  return GetUser(Form1.context.GetSelectedUser());
            case "game":  return GetGame(Form1.context.GetSelectedGame());
            case "order": return GetOrder(Form1.context.GetSelectedOrder());
            }
            return null;
        }


        // ----------------------------------------------------------------- //
        // Sets the selected entity based on the given type                  //
        // ----------------------------------------------------------------- //
        public void SetSelected(string type, string key) {
            switch (type.ToLower()) {
            case "user":  selected_user  = key; break;
            case "game":  selected_game  = key; break;
            case "order": selected_order = key; break;
            }
        }
        #endregion


        #region internal
        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public Context() {
            users  = new Dictionary<string, User>();
            games  = new Dictionary<string, Game>();
            orders = new Dictionary<string, Order>();

            logged_user    = "";
            selected_user  = "mike";
            selected_game  = "pacman";
            selected_order = "x000";

            // mockup a logon page
            users = new Dictionary<string, User>();
            users.Add("mike",   new User("mike",   "letmein", "mike",   "collins", "mike@collins.com", "limerick", "061-123456"));
            users.Add("louise", new User("louise", "letmein", "louise", "mckeown", "louise@mckeown.com", "limerick", "087-9876543"));
            users.Add("alan",   new User("alan",   "letmein", "alan",   "redding", "alan@redding.com", "limerick", "1800-555-12345"));

            games = new Dictionary<string, Game>();
            games.Add("mario",     new Game("mario",     "platform", 10, "can mario save the princess?"));
            games.Add("pacman",    new Game("pacman",    "maze",     10, "help pacman get out of the ghost ridden maze"));
            games.Add("asteroids", new Game("asteroids", "shooter",  10, "save the universe or die trying"));

            orders = new Dictionary<string, Order>();
            orders.Add("x000", new Order("x000", "mike",   "mario",     "01/04/2016", ""));
            orders.Add("x001", new Order("x001", "louise", "pacman",    "02/04/2016", ""));
            orders.Add("x002", new Order("x002", "alan",   "asteroids", "03/04/2016", ""));
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

            try {
                file = new FileStream("orders.bin", FileMode.Open, FileAccess.Read);
                formatter = new BinaryFormatter();
                orders = formatter.Deserialize(file) as Dictionary<string, Order>;
                file.Close();
            } catch {
                MessageBox.Show("File not found! 'orders.bin'");
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

            file = new FileStream("orders.bin", FileMode.OpenOrCreate, FileAccess.Write);
            formatter = new BinaryFormatter();
            formatter.Serialize(file, orders);
            file.Close();
        }
        #endregion
    }
}