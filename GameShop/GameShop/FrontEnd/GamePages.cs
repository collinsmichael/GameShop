// ========================================================================= //
// File Name : GamePages.cs                                                  //
// File Date : 12 April 2016                                                 //
// Author(s) : Michael Collins, Louise McKeown, Alan Redding                 //
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
            form.Add("label1", new WidgetLabel(160, 196, 112, 32, "Title"));
            form.Add("label2", new WidgetLabel(160, 236, 112, 32, "Genre"));
            form.Add("label3", new WidgetLabel(160, 276, 112, 32, "Info"));
            form.Add("genre",  new WidgetTextBox(272, 236, 320, 32, "", true, false, false));
            form.Add("info",   new WidgetTextBox(272, 276, 320, 96, "", true, true,  false));
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
            (Form1.formgen.GetControl(page, "title") as TextBox).Text = game.GetTitle();
            (Form1.formgen.GetControl(page, "genre") as TextBox).Text = game.GetGenre();
            (Form1.formgen.GetControl(page, "info")  as TextBox).Text = game.GetInfo();
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
            view.Add("title",  new WidgetTextBox(272, 196, 320, 32, "", true, false, false));
            view.Add("edit",   new WidgetButton(464, 388, 128, 32, "Edit", OnViewEditClick));
            view.Add("order",  new WidgetButton(272, 388, 128, 32, "Order", OnViewOrderClick));
            Form1.formgen.AddPage("game.view", view);
        }


        // ----------------------------------------------------------------- //
        // Clicking edit on the view page will reroute the user to the edit  //
        // page and populate the form with the fields from the entity.       //
        // ----------------------------------------------------------------- //
        public void OnViewEditClick(object sender, EventArgs e) {
            string pagename = typename + ".edit";
            Form1.formgen.BuildPage(pagename);
            FormPage formpage = Form1.formgen.GetPage(typename + ".form") as FormPage;
            formpage.OnPopulateForm(pagename);
        }


        // ----------------------------------------------------------------- //
        // Clicking order on the view page will reroute the user to an order //
        // page and populate the form with the fields from the entity.       //
        // ----------------------------------------------------------------- //
        public void OnViewOrderClick(object sender, EventArgs e) {
            string pagename = typename + ".order";
            MessageBox.Show(typename + "\n" + pagename);
            //Form1.formgen.BuildPage(pagename);
            //FormPage formpage = Form1.formgen.GetPage(typename + ".form") as FormPage;
            //formpage.OnPopulateForm(pagename);
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
            edit.Add("title",  new WidgetTextBox(272, 196, 320, 32, "", true, false, false));
            edit.Add("submit", new WidgetButton(464, 388, 128, 32, "Submit", OnEditSubmitClick));
            edit.Add("cancel", new WidgetButton(272, 388, 128, 32, "Cancel", OnCancelClick));
            Form1.formgen.AddPage("game.edit", edit);
        }


        // ----------------------------------------------------------------- //
        // this method populates a game object from the fields within a form //
        // ----------------------------------------------------------------- //
        public void OnEditSubmitClick(object sender, EventArgs e) {
            // TODO : Sanitize field data (use regular expression parsing)
            // TODO : If you're not using any of these please comment out
            //        rather than deleting these took a lot of time to build
            #region sanitize
            Regex regexname = new Regex(@"^[A-Za-z]+", RegexOptions.IgnoreCase);
            Regex regexdate = new Regex(@"^[0-9]{1,2}[-/.]{1}[0-9]{1,2}[-/.]{1}[0-9]{2,4}");
            Regex regexvalu = new Regex(@"^[0-9]+", RegexOptions.IgnoreCase);
            Regex regexurls = new Regex(@"^www[.][a-z]{1,15}[.](com|org)$");
            Regex regexmail = new Regex(@"^[a-zA-Z0-9]{1,10}@[a-zA-Z]{1,10}.(com|org)$");

            Match matchname = regexname.Match("mike");
        	if (matchname.Success) MessageBox.Show(matchname.Value);
        	else                   MessageBox.Show("No Match");

            Match matchvalu = regexvalu.Match("123456");
        	if (matchvalu.Success) MessageBox.Show(matchvalu.Value);
        	else                   MessageBox.Show("No Match");

            Match matchdate = regexdate.Match("1-1-2000");
        	if (matchdate.Success) MessageBox.Show(matchdate.Value);
        	else                   MessageBox.Show("No Match");
            matchdate = regexdate.Match("1/1/2000");
        	if (matchdate.Success) MessageBox.Show(matchdate.Value);
        	else                   MessageBox.Show("No Match");
            matchdate = regexdate.Match("1.1.2000");
        	if (matchdate.Success) MessageBox.Show(matchdate.Value);
        	else                   MessageBox.Show("No Match");

            Match matchurls = regexurls.Match("www.domain.com");
        	if (matchurls.Success) MessageBox.Show(matchurls.Value);
        	else                   MessageBox.Show("No Match");

            Match matchmail = regexmail.Match("mike@collins.com");
        	if (matchmail.Success) MessageBox.Show(matchmail.Value);
        	else                   MessageBox.Show("No Match");
            #endregion

            Game game = Form1.context.GetGame(Form1.context.GetSelectedGame());
            game.SetTitle((Form1.formgen.GetControl("game.edit", "title") as TextBox).Text);
            game.SetGenre((Form1.formgen.GetControl("game.edit", "genre") as TextBox).Text);
            game.SetInfo((Form1.formgen.GetControl("game.edit", "info") as TextBox).Text);

            Form1.context.SetSelected("game", game.GetTitle());
            Form1.formgen.BuildPage("game.list");
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
            make.Add("title",  new WidgetTextBox(272, 196, 320, 32, "", false, false, false));
            make.Add("submit", new WidgetButton(464, 388, 128, 32, "Submit", OnMakeSubmitClick));
            make.Add("cancel", new WidgetButton(272, 388, 128, 32, "Cancel", OnCancelClick));
            Form1.formgen.AddPage("game.make", make);
        }


        // ----------------------------------------------------------------- //
        // this method creates a game object from the fields within a form.  //
        // ----------------------------------------------------------------- //
        public void OnMakeSubmitClick(object sender, EventArgs e) {
            // TODO : Sanitize field data (use regular expression parsing)
            // TODO : If you're not using any of these please comment out
            //        rather than deleting these took a lot of time to build
            #region sanitize
            Regex regexname = new Regex(@"^[A-Za-z]+", RegexOptions.IgnoreCase);
            Regex regexdate = new Regex(@"^[0-9]{1,2}[-/.]{1}[0-9]{1,2}[-/.]{1}[0-9]{2,4}");
            Regex regexvalu = new Regex(@"^[0-9]+", RegexOptions.IgnoreCase);
            Regex regexurls = new Regex(@"^www[.][a-z]{1,15}[.](com|org)$");
            Regex regexmail = new Regex(@"^[a-zA-Z0-9]{1,10}@[a-zA-Z]{1,10}.(com|org)$");

            Match matchname = regexname.Match("mike");
        	if (matchname.Success) MessageBox.Show(matchname.Value);
        	else                   MessageBox.Show("No Match");

            Match matchvalu = regexvalu.Match("123456");
        	if (matchvalu.Success) MessageBox.Show(matchvalu.Value);
        	else                   MessageBox.Show("No Match");

            Match matchdate = regexdate.Match("1-1-2000");
        	if (matchdate.Success) MessageBox.Show(matchdate.Value);
        	else                   MessageBox.Show("No Match");
            matchdate = regexdate.Match("1/1/2000");
        	if (matchdate.Success) MessageBox.Show(matchdate.Value);
        	else                   MessageBox.Show("No Match");
            matchdate = regexdate.Match("1.1.2000");
        	if (matchdate.Success) MessageBox.Show(matchdate.Value);
        	else                   MessageBox.Show("No Match");

            Match matchurls = regexurls.Match("www.domain.com");
        	if (matchurls.Success) MessageBox.Show(matchurls.Value);
        	else                   MessageBox.Show("No Match");

            Match matchmail = regexmail.Match("mike@collins.com");
        	if (matchmail.Success) MessageBox.Show(matchmail.Value);
        	else                   MessageBox.Show("No Match");
            #endregion

            Game game = new Game();
            game.SetTitle((Form1.formgen.GetControl("game.make", "title") as TextBox).Text);
            game.SetGenre((Form1.formgen.GetControl("game.make", "genre") as TextBox).Text);
            game.SetInfo((Form1.formgen.GetControl("game.make", "info") as TextBox).Text);

            Form1.context.AddGame(game.GetTitle(), game);
            Form1.context.SetSelected("game", game.GetTitle());
            Form1.formgen.BuildPage("game.list");
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
            drop.Add("title",  new WidgetTextBox(272, 196, 320, 32, "", true, false, false));
            drop.Add("delete", new WidgetButton(464, 388, 128, 32, "Delete", OnDropDeleteClick));
            drop.Add("cancel", new WidgetButton(272, 388, 128, 32, "Cancel", OnCancelClick));
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
            list.Add("search",    new WidgetTextBox(112, 534, 368, 32, "", false, false, false));
            list.Add("addnew",    new WidgetButton(800-32-128, 532, 128, 32, "Add New", OnListAddNewClick));
            list.Add("delete",    new WidgetButton(800-32-256-16, 532, 128, 32, "Delete",  OnListDeleteClick));

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
            listview.Columns.Add("Title", 100);
            listview.Columns.Add("Genre", 100);
            listview.Columns.Add("Info",  200);
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
                game.GetTitle(), game.GetGenre(), game.GetInfo()
            };
            listview.Items.Add(new ListViewItem(fields));
            return true;
        }
    }
    #endregion


}