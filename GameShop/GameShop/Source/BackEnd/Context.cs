// ========================================================================= //
// File Name : Context.cs                                                    //
// File Date : 12 April 2016                                                 //
// Author(s) : Michael Collins, Louise McKeown, Alan Rowlands                //
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
        public Dictionary<string, Member>  members;  // list of all users
        public Dictionary<string, Staff> staffs;     // list of all users
        public Dictionary<string, Game>  games;      // list of all games
        public Dictionary<string, Order> orders;     // list of all orders
        public Dictionary<string, Report> reports;
        private string logged_staff;             // user which is currently logged in
        private string selected_member;          // currently selected user
        private string selected_game;            // currently selected game
        private string selected_order;           // currently selected order
        private string selected_report;
        private string selected_staff; 

        #region memberio
        // ----------------------------------------------------------------- //
        // AddUser is used to add a defined user to the user list.           //
        // Once a user has been added it can later be referred to by name.   //
        // ----------------------------------------------------------------- //
        public bool AddMember(string key, Member member) {
            if (members.ContainsKey(key)) {
                string message = "Member "+key+" Already exists!\n Replace existing member?";
                DialogResult result = MessageBox.Show(message, "Warning", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes) {
                    members.Remove(key);
                    members.Add(key, member);
                }
            } else {
                members.Add(key, member);
            }
            return true;
        }


        // ----------------------------------------------------------------- //
        // GetUser allows internal access to the contents of the user list.  //
        // To access a user you must provide the username.                   //
        // ----------------------------------------------------------------- //
        public Member GetMember(string username) {
            Member member = new Member();
            if (!members.TryGetValue(username, out member)) {
                MessageBox.Show("Member '" + username + "' was not found!");
                return member;
            }
            return member;
        }


        // ----------------------------------------------------------------- //
        // Returns the primary key of the currently selected user.           //
        // ----------------------------------------------------------------- //
        public string GetSelectedMember() {
            User select_member = GetMember(selected_member);
            if (select_member != null && select_member.GetUserName() == selected_member) {
                return selected_member;
            }

            foreach (KeyValuePair<string, Member> member in members) {
                selected_member = member.Key;
                break;
            }
            return selected_member;
        }
        #endregion


        #region staffio
        // ----------------------------------------------------------------- //
        // AddUser is used to add a defined user to the user list.           //
        // Once a user has been added it can later be referred to by name.   //
        // ----------------------------------------------------------------- //
        public bool AddStaff(string key, Staff staff) {
            if (staffs.ContainsKey(key)) {
                string message = "Staff "+key+" Already exists!\n Replace existing staff?";
                DialogResult result = MessageBox.Show(message, "Warning", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes) {
                    staffs.Remove(key);
                    staffs.Add(key, staff);
                }
            } else {
                staffs.Add(key, staff);
            }
            return true;
        }


        // ----------------------------------------------------------------- //
        // GetUser allows internal access to the contents of the user list.  //
        // To access a user you must provide the username.                   //
        // ----------------------------------------------------------------- //
        public Staff GetStaff(string username) {
            Staff staff = new Staff();
            if (!staffs.TryGetValue(username, out staff)) {
                return staff;
            }
            return staff;
        }


        // ----------------------------------------------------------------- //
        // Returns the primary key of the currently selected user.           //
        // ----------------------------------------------------------------- //
        public string GetSelectedStaff() {
            Staff select_staff = GetStaff(selected_staff);
            if (select_staff != null && select_staff.GetUserName() == selected_staff) {
                return selected_staff;
            }

            foreach (KeyValuePair<string, Staff> staff in staffs) {
                selected_staff = staff.Key;
                break;
            }
            return selected_staff;
        }


        // ----------------------------------------------------------------- //
        // Caches a local copy of the logged in users primary key            //
        // ----------------------------------------------------------------- //
        public void SetLogged(string key) {
            logged_staff = key;
        }


        // ----------------------------------------------------------------- //
        // Gets the logged in user                                           //
        // ----------------------------------------------------------------- //
        public Staff GetLogged(string type) {
            return GetStaff(Form1.context.logged_staff);
        }
        #endregion


        #region gameio
        // ----------------------------------------------------------------- //
        // AddGame is used to add a defined game to the game list.           //
        // Once a game has been added it can later be referred to by name.   //
        // ----------------------------------------------------------------- //
        public bool AddGame(string key, Game game) {
            if (games.ContainsKey(key)) {
                string message = "Game "+key+" Already exists!\n Replace existing game?";
                DialogResult result = MessageBox.Show(message, "Warning", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes) {
                    games.Remove(key);
                    games.Add(key, game);
                }
            } else {
                games.Add(key, game);
            }
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


        #region reportio
        // ----------------------------------------------------------------- //
        // AddGame is used to add a defined game to the game list.           //
        // Once a game has been added it can later be referred to by name.   //
        // ----------------------------------------------------------------- //
        public bool AddReport(string key, Report report) {
            if (reports.ContainsKey(key)) {
                string message = "Report "+key+" Already exists!\n Replace existing report?";
                DialogResult result = MessageBox.Show(message, "Warning", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes) {
                    reports.Remove(key);
                    reports.Add(key, report);
                }
            } else {
                reports.Add(key, report);
            }
            return true;
        }


        // ----------------------------------------------------------------- //
        // GetGame allows internal access to the contents of the game list.  //
        // To access a game you must provide the title.                      //
        // ----------------------------------------------------------------- //
        public Report GetReport(string reportid) {
            Report report = new Report();
            if (!reports.TryGetValue(reportid, out report)) {
                ///MessageBox.Show("Report '" + title + "' was not found!");
                return report;
            }
            return report;
        }


        // ----------------------------------------------------------------- //
        // Returns the primary key of the currently selected game.           //
        // ----------------------------------------------------------------- //
        public string GetSelectedReport() {
            Report select_report = GetReport(selected_report);
            if (select_report != null && select_report.GetReportId() == selected_report) {
                return selected_report;
            }

            foreach (KeyValuePair<string,Report> report in reports) {
                selected_report = report.Key;
                break;
            }
            return selected_report;
        }
        #endregion


        #region orderio
        // ----------------------------------------------------------------- //
        // AddOrder is used to add a defined order to the order list.        //
        // Once an order has been added it can later be referred to by name. //
        // ----------------------------------------------------------------- //
        public bool AddOrder(string key, Order order) {
            if (orders.ContainsKey(key)) {
                string message = "Order "+key+" Already exists!\n Replace existing order?";
                DialogResult result = MessageBox.Show(message, "Warning", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes) {
                    orders.Remove(key);
                    orders.Add(key, order);
                }
            } else {
                orders.Add(key, order);
            }
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
            case "user":
            case "member": return GetMember(Form1.context.GetSelectedMember());
            case "staff":  return GetStaff(Form1.context.GetSelectedStaff());
            case "game":   return GetGame(Form1.context.GetSelectedGame());
            case "order":  return GetOrder(Form1.context.GetSelectedOrder());
            case "report": return GetReport(Form1.context.GetSelectedReport());
            }
            return null;
        }


        // ----------------------------------------------------------------- //
        // Sets the selected entity based on the given type                  //
        // ----------------------------------------------------------------- //
        public void SetSelected(string type, string key) {
            switch (type.ToLower()) {
            case "user":
            case "member": selected_member = key; break;
            case "game":   selected_game   = key; break;
            case "order":  selected_order  = key; break;
            case "staff":  selected_staff  = key; break;
            case "report": selected_report = key; break;
            }
        }
        #endregion


        #region internal
        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public Context() {
            members = new Dictionary<string, Member>();
            staffs  = new Dictionary<string, Staff>();
            games   = new Dictionary<string, Game>();
            orders  = new Dictionary<string, Order>();
            reports = new Dictionary<string, Report>();

            logged_staff    = "";
            selected_member = "member";
            selected_staff  = "staff";
            selected_game   = "pacman";
            selected_order  = "x000";
            selected_report = "t000";

            staffs = new Dictionary<string, Staff>();
            staffs.Add("manager", new Manager("manager",  "",       "manager", "test",    "manager@gameshop.com", "limerick", "067-555123",     "01/01/1990"));
            staffs.Add("staff",   new Staff("staff",    "",       "staff",   "test",    "staff@gameshop.com",   "limerick", "067-555123",     "01/01/1990"));
            staffs.Add("mike",    new Manager("mike",    "letmein", "mike",    "collins", "mike@collins.com",     "limerick", "061-123456",     "01/01/1990"));
            staffs.Add("louise",  new Manager("louise",  "letmein", "louise",  "mckeown", "louise@mckeown.com",   "limerick", "087-9876543",    "01/01/1990"));
            staffs.Add("alan",    new Manager("alan",    "letmein", "alan",    "redding", "alan@redding.com",     "limerick", "1800-555-12345", "01/01/1990"));

            members = new Dictionary<string, Member>();
            members.Add("member",  new Member(         "member",             "member",  "test",    "member@gameshop.com",  "limerick", "067-555123",     "01/01/1990"));
            members.Add("john",    new Member(         "john",               "john",    "smith",   "john@gameshop.com",    "limerick", "067-555123",     "01/01/1990"));
            members.Add("mary",    new Member(         "mary",               "mary",    "jones",   "mary@gameshop.com",    "limerick", "067-555123",     "01/01/1990"));
            members.Add("lisa",    new Member(         "lisa",               "lisa",    "o' dea",  "lisa@gameshop.com",    "limerick", "067-555123",     "01/01/1990"));
 
            games = new Dictionary<string, Game>();
            games.Add("mario",     new Game("mario",     "platform", "12", 10, "can mario save the princess?"));
            games.Add("pacman",    new Game("pacman",    "maze",     "12", 10, "help pacman get out of the ghost ridden maze"));
            games.Add("asteroids", new Game("asteroids", "shooter",  "18", 10, "save the universe or die trying"));
            games.Add("fallout 4", new Game("fallout 4", "rpg",      "18", 10, "explore the post apocolyptic world after a nuclear fallout"));
            games.Add("forza 2",   new Game("forza 2",   "racing",   "3",  10, "race over beautiful race tracks perfectly recreated from around the world in awide varity of cars"));
            games.Add("minecraft", new Game("minecraft", "rpg",      "7",  10, "explore and build in a generated block world, let your imagination run wild"));
            games.Add("the division", new Game("the division", "rpg", "16", 10, "save a devastated new york city from the dollar flu that has infected the city into a quarintine zone"));
            games.Add("fifa 16", new Game("fifa 16", "sports", "3", 10, "play as some of your favourite teams across a wide varity of different stadiums and competitions")); 

            orders = new Dictionary<string, Order>();
            orders.Add("x000", new Order("x000", "mike",   "mario",     "01/04/2016", ""));
            orders.Add("x001", new Order("x001", "louise", "pacman",    "02/04/2016", ""));
            orders.Add("x002", new Order("x002", "alan",   "asteroids", "03/04/2016", ""));

            reports = new Dictionary<string, Report>();
            reports.Add("z000", new Report("z000", "mike",   "4/4/2016", 3, 1, 20));
            reports.Add("z001", new Report("z001", "louise", "4/4/2016", 3, 1, 20));
            reports.Add("z002", new Report("z002", "alan",   "4/4/2016", 3, 1, 20));
        }


        // ----------------------------------------------------------------- //
        // Startup prior to runtime                                          //
        // ----------------------------------------------------------------- //
        public void OnLoad() {
            FileStream file;
            BinaryFormatter formatter;
            try {
                file = new FileStream("staff.bin", FileMode.Open, FileAccess.Read);
                formatter = new BinaryFormatter();
                staffs = formatter.Deserialize(file) as Dictionary<string, Staff>;
                file.Close();
            } catch {
                MessageBox.Show("File not found! 'staff.bin'");
            }

            try {
                file = new FileStream("members.bin", FileMode.Open, FileAccess.Read);
                formatter = new BinaryFormatter();
                members = formatter.Deserialize(file) as Dictionary<string, Member>;
                file.Close();
            } catch {
                MessageBox.Show("File not found! 'members.bin'");
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

            try {
                 file = new FileStream("reports.bin", FileMode.Open, FileAccess.Read);
                 formatter = new BinaryFormatter();
                 reports= formatter.Deserialize(file) as Dictionary<string, Report>;
                 file.Close();
             } catch {
                 MessageBox.Show("File not found! 'reports.bin'");
             }
        }
        


        // ----------------------------------------------------------------- //
        // Cleanup prior to shutdown                                         //
        // ----------------------------------------------------------------- //
        public void OnExit() {
            FileStream file;
            BinaryFormatter formatter = new BinaryFormatter();

            string state = "";
            foreach (KeyValuePair<string, Member> member in members) state = state + member.Value.Read();
            foreach (KeyValuePair<string, Staff>  staff  in staffs)  state = state + staff.Value.Read();
            foreach (KeyValuePair<string, Game>   game   in games)   state = state + game.Value.Read();
            foreach (KeyValuePair<string, Order>  order  in orders)  state = state + order.Value.Read();
            foreach (KeyValuePair<string, Report> report in reports) state = state + report.Value.Read();
            
            System.IO.File.WriteAllLines("logfile.txt", state.Split(new char[] { '\n' }));


            file = new FileStream("staff.bin", FileMode.OpenOrCreate, FileAccess.Write);
            formatter.Serialize(file, staffs);
            file.Close();

            file = new FileStream("members.bin", FileMode.OpenOrCreate, FileAccess.Write);
            formatter.Serialize(file, members);
            file.Close();

            file = new FileStream("games.bin", FileMode.OpenOrCreate, FileAccess.Write);
            formatter.Serialize(file, games);
            file.Close();

            file = new FileStream("orders.bin", FileMode.OpenOrCreate, FileAccess.Write);
            formatter.Serialize(file, orders);
            file.Close();

            file = new FileStream("reports.bin", FileMode.OpenOrCreate, FileAccess.Write);
            formatter.Serialize(file, reports);
            file.Close();

        }
        #endregion
    }
}