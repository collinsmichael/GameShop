// ========================================================================= //
// File Name : GamePages.cs                                                  //
// File Date : 12 April 2016                                                 //
// Author(s) : Michael Collins, Louise McKeown, Alan Rowlands                //
// File Info : The Game Pages are responsible for representing the Games in  //
//             the form. There are a number of Game Pages each responsible   //
//             for their own Form.                                           //
//                 o game.list  (list box form)                              //
//                 o game.form  (game member controls)                       //
//                 o game.view  (edit cancel buttons on the view form)       //
//                 o game.edit  (submit cancel buttons on the edit form)     //
//                 o game.make  (submit cancel buttons on the make form)     //
//                 o game.drop  (delete cancel buttons on the drop form)     //
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
    #region formpage
    [Serializable]
    public class GameFormPage : FormPage {
        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public GameFormPage()
        : base("game", "game.form") { }


        // ----------------------------------------------------------------- //
        // This method is invoked at startup. Here we define all of the form //
        // controls associated with game objects.                            //
        // ----------------------------------------------------------------- //
        public override void DefinePage() {
            // basic form fields
            Dictionary<string, Widget> form = new Dictionary<string, Widget>();
            form.Add("label1",     new WidgetLabel(150, 156, 122, 32, "Title"));
            form.Add("label2",     new WidgetLabel(150, 196, 122, 32, "Genre"));
            form.Add("label3",     new WidgetLabel(150, 236, 122, 32, "Age Rating"));
            form.Add("label4",     new WidgetLabel(150, 276, 122, 32, "Stock"));
            form.Add("label5",     new WidgetLabel(150, 316, 122, 32, "Info"));
            form.Add("genre",      new WidgetTextBox(272, 192, 320, 32, "", true, false, false, 1));
            form.Add("agerating",  new WidgetTextBox(272, 232, 320, 32, "", true, false, false, 2));
            form.Add("stock",      new WidgetTextBox(272, 272, 320, 32, "", true, false, false, 3));
            form.Add("info",       new WidgetTextBox(272, 312, 320, 96, "", true, true,  false, 4));
            Form1.formgen.AddPage("game.form", form);
        }


        // ----------------------------------------------------------------- //
        // this method populates the fields within a form page.              //
        // ----------------------------------------------------------------- //
        public override void OnPopulateForm(string page) {
            Form1.formgen.BuildPage(page);
            Game game = new Game();
            if (page != "game.make") {
                game = Form1.context.GetGame(Form1.context.GetSelectedGame());
            }
            (Form1.formgen.GetControl(page, "title")     as TextBox).Text = game.GetTitle();
            (Form1.formgen.GetControl(page, "genre")     as TextBox).Text = game.GetGenre();
            (Form1.formgen.GetControl(page, "agerating") as TextBox).Text = game.GetAgeRating();
            (Form1.formgen.GetControl(page, "stock")     as TextBox).Text = game.GetStock().ToString();
            (Form1.formgen.GetControl(page, "info")      as TextBox).Text = game.GetInfo();
        }


        // ----------------------------------------------------------------- //
        // this method sanitizes values obtained from the form, and will not //
        // apply changes unless all values pass the sanity test.             //
        // ----------------------------------------------------------------- //
        public override bool Sanitize(string page) {
            // get values from form
            string title     = (Form1.formgen.GetControl(page, "title")     as TextBox).Text;
            string genre     = (Form1.formgen.GetControl(page, "genre")     as TextBox).Text;
            string agerating = (Form1.formgen.GetControl(page, "agerating") as TextBox).Text;
            string info      = (Form1.formgen.GetControl(page, "info")      as TextBox).Text;
            string stocktext = (Form1.formgen.GetControl(page, "stock")     as TextBox).Text;

            // will only accept positive whole numbers
            Regex regenumber = (new Regex(@"^[0-9]+"));
            if (!regenumber.Match(stocktext).Success) {
                MessageBox.Show("Please enter a positive whole number in the Stock field!");
                return false;
            }

            // add game to the database and to the users account
            Game game = new Game(title, genre, agerating, int.Parse(stocktext), info);
            Form1.context.AddGame(title, game);
            Form1.context.SetSelected("game", title);
            return true;
        }
    }
    #endregion


    #region viewpage
    [Serializable]
    public class GameViewPage : Page {
        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public GameViewPage()
        : base("game", "game.view") { }


        // ----------------------------------------------------------------- //
        // This method is invoked at startup. Here we define all of the form //
        // controls associated with game objects.                            //
        // ----------------------------------------------------------------- //
        public override void DefinePage() {
            // extra fields for view page
            Dictionary<string, Widget> view = new Dictionary<string, Widget>();
            view.Add("header", new WidgetTitle("View Game"));
            view.Add("title",  new WidgetTextBox(272, 152, 320, 32, "", true, false, false, 0));
            view.Add("edit",   new WidgetButton(464, 428, 128, 32, "Edit", OnViewEditClick, 5));
            view.Add("order",  new WidgetButton(272, 428, 128, 32, "Order", OnViewOrderClick, 6));
            Form1.formgen.AddPage("game.view", view);
        }


        // ----------------------------------------------------------------- //
        // Clicking order on the view page will reroute the user to an order //
        // page and populate the form with the fields from the entity.       //
        // ----------------------------------------------------------------- //
        public void OnViewOrderClick(object sender, EventArgs e) {
            User user = Form1.context.GetSelected("user") as User;
            Game game = Form1.context.GetSelected("game") as Game;
            Form1.formgen.BuildPage("order.make");
            Form1.formgen.BuildPage("order.make");
            FormPage formpage = Form1.formgen.GetPage("order.form") as FormPage;
            (Form1.formgen.GetControl("order.make", "username") as TextBox).Text = user.GetUserName();
            (Form1.formgen.GetControl("order.make", "title")    as TextBox).Text = game.GetTitle();
        }
    }
    #endregion


    #region editpage
    [Serializable]
    public class GameEditPage : Page {
        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public GameEditPage()
        : base("game", "game.edit") { }


        // ----------------------------------------------------------------- //
        // This method is invoked at startup. Here we define all of the form //
        // controls associated with game objects.                            //
        // ----------------------------------------------------------------- //
        public override void DefinePage() {
            // extra fields for edit page
            Dictionary<string, Widget> edit = new Dictionary<string, Widget>();
            edit.Add("header", new WidgetTitle("Edit Game"));
            edit.Add("title",  new WidgetTextBox(272, 152, 320, 32, "", true, false, false, 0));
            edit.Add("submit", new WidgetButton(464, 428, 128, 32, "Submit", OnEditSubmitClick, 5));
            edit.Add("cancel", new WidgetButton(272, 428, 128, 32, "Cancel", OnCancelClick, 6));
            Form1.formgen.AddPage("game.edit", edit);
        }


        // ----------------------------------------------------------------- //
        // this method populates a game object from the fields within a form //
        // ----------------------------------------------------------------- //
        public void OnEditSubmitClick(object sender, EventArgs e) {
            FormPage form = Form1.formgen.GetPage("game.form") as FormPage;
            if (form != null && form.Sanitize("game.edit") == true) {
                Form1.formgen.BuildPage("game.list");
            }
        }
    }
    #endregion


    #region makepage
    [Serializable]
    public class GameMakePage : Page {
        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public GameMakePage()
        : base("game", "game.make") { }


        // ----------------------------------------------------------------- //
        // This method is invoked at startup. Here we define all of the form //
        // controls associated with game objects.                            //
        // ----------------------------------------------------------------- //
        public override void DefinePage() {
            // extra fields for make page
            Dictionary<string, Widget> make = new Dictionary<string, Widget>();
            make.Add("header", new WidgetTitle("New Game"));
            make.Add("title",  new WidgetTextBox(272, 152, 320, 32, "", false, false, false, 0));
            make.Add("submit", new WidgetButton(464, 428, 128, 32, "Submit", OnMakeSubmitClick, 5));
            make.Add("cancel", new WidgetButton(272, 428, 128, 32, "Cancel", OnCancelClick, 6));
            Form1.formgen.AddPage("game.make", make);
        }


        // ----------------------------------------------------------------- //
        // this method creates a game object from the fields within a form.  //
        // ----------------------------------------------------------------- //
        public void OnMakeSubmitClick(object sender, EventArgs e) {
            FormPage form = Form1.formgen.GetPage("game.form") as FormPage;
            if (form != null && form.Sanitize("game.make") == true) {
                Form1.formgen.BuildPage("game.list");
            }
        }
    }
    #endregion


    #region droppage
    [Serializable]
    public class GameDropPage : Page {
        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public GameDropPage()
        : base("game", "game.drop") { }


        // ----------------------------------------------------------------- //
        // This method is invoked at startup. Here we define all of the form //
        // controls associated with game objects.                            //
        // ----------------------------------------------------------------- //
        public override void DefinePage() {
            // extra fields for drop page
            Dictionary<string, Widget> drop = new Dictionary<string, Widget>();
            drop.Add("header", new WidgetTitle("Delete Game"));
            drop.Add("title",  new WidgetTextBox(272, 152, 320, 32, "", true, false, false, 0));
            drop.Add("delete", new WidgetButton(464, 428, 128, 32, "Delete", OnDropDeleteClick, 5));
            drop.Add("cancel", new WidgetButton(272, 428, 128, 32, "Cancel", OnCancelClick, 6));
            Form1.formgen.AddPage("game.drop", drop);
        }


        // ----------------------------------------------------------------- //
        // this method deletes a game object after warning the user.         //
        // ----------------------------------------------------------------- //
        public void OnDropDeleteClick(object sender, EventArgs e) {
            string title = (Form1.formgen.GetControl("game.drop", "title") as TextBox).Text;
            string genre = (Form1.formgen.GetControl("game.drop", "genre") as TextBox).Text;

            string message = string.Format("Title = {0}\nGenre = {1}\nAre you sure you wish to\n"
                                          +"delete this game form the database?", title, genre);
            DialogResult result = MessageBox.Show(message, "Warning", MessageBoxButtons.YesNoCancel);
            if (result == DialogResult.Yes) {
                Form1.context.games.Remove(title);
            }
            Form1.formgen.BuildPage("game.list");
        }
    }
    #endregion


    #region listpage
    [Serializable]
    public class GameListPage : ListPage {
        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public GameListPage()
        : base("game", "game.list") { }


        // ----------------------------------------------------------------- //
        // This method is invoked at startup. Here we define all of the form //
        // controls associated with game objects.                            //
        // ----------------------------------------------------------------- //
        public override void DefinePage() {
            // list games page
            Dictionary<string, Widget> list = new Dictionary<string, Widget>();
            list = new Dictionary<string, Widget>();
            list.Add("header",    new WidgetTitle("Games"));
            list.Add("listview",  new WidgetListView(32, 132, 800-64, 400-28, OnListRecordClick, ListViewMakeVisible));
            list.Add("label1",    new WidgetLabel(32, 536, 80, 32, "Search"));
            list.Add("search",    new WidgetTextBox(112, 534, 368, 32, "", false, false, false, 0));
            list.Add("addnew",    new WidgetButton(800-32-128, 532, 128, 32, "Add New", OnListAddNewClick, 1));
            list.Add("delete",    new WidgetButton(800-32-256-16, 532, 128, 32, "Delete",  OnListDeleteClick, 2));

            Form1.formgen.AddPage("game.list", list);
            TextBox search = Form1.formgen.GetControl("game.list", "search") as TextBox;
            if (search != null) search.TextChanged += OnUpdateSearch;
        }


        // ----------------------------------------------------------------- //
        // this method gets invoked automatically by the list view control   //
        // any time the list view needs to be updated.                       //
        // All this method does is to assign a name to each required column. //
        // ----------------------------------------------------------------- //
        public override void OnPopulateListColumns(ListView listview) {
            // the number of columns defined must match the number of values
            // passed in PopulateListItem
            listview.Columns.Clear();
            listview.Columns.Add("Title",      100);
            listview.Columns.Add("Genre",      100);
            listview.Columns.Add("Age Rating", 120);
            listview.Columns.Add("Stock",      80);
            listview.Columns.Add("Info",       300);
        }


        // ----------------------------------------------------------------- //
        // this method enumerates the game list and populates listview rows. //
        // ----------------------------------------------------------------- //
        public override void OnPopulateListRecords(ListView listview) {
            listview.Items.Clear();
            foreach (KeyValuePair<string, Game> game in Form1.context.games) {
                PopulateListItem(listview, game.Value);
            }
        }


        // ----------------------------------------------------------------- //
        // this method gets invoked automatically by the list view control   //
        // any time the list view needs to be updated.                       //
        // All this method does is to assign a values to each field in the   //
        // list view record.                                                 //
        // ----------------------------------------------------------------- //
        public bool PopulateListItem(ListView listview, Game game) {
            // the number of values passed must match the number of columns
            // defined in PopulateListColumns
            string[] fields = new[] {
                game.GetTitle(), game.GetGenre(), game.GetAgeRating(),
                game.GetStock().ToString(), game.GetInfo()
            };
            listview.Items.Add(new ListViewItem(fields));
            return true;
        }
    }
    #endregion
}