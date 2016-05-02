// ========================================================================= //
// File Name : OrderPages.cs                                                 //
// File Date : 16 April 2016                                                 //
// Author(s) : Michael Collins, Louise McKeown, Alan Rowlands                //
// File Info : The Order Pages are responsible for representing the Orders.  //
//             Orders kee track of which members have reserved games. Orders //
//             have a different page for each of the following tasks.        //
//                 o order.list  (list box form)                             //
//                 o order.form  (order member controls)                     //
//                 o order.view  (edit cancel buttons on the view form)      //
//                 o order.edit  (submit cancel buttons on the edit form)    //
//                 o order.make  (submit cancel buttons on the make form)    //
//                 o order.drop  (delete cancel buttons on the drop form)    //
// ========================================================================= //

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
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
    public class OrderFormPage : FormPage {
        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public OrderFormPage()
        : base("order", "order.form") { }


        // ----------------------------------------------------------------- //
        // This method is invoked at startup. Here we define all of the form //
        // controls associated with game objects.                            //
        // ----------------------------------------------------------------- //
        public override void DefinePage() {
            // basic form fields
            Dictionary<string, Widget> form = new Dictionary<string, Widget>();
            form.Add("label1",     new WidgetLabel(150, 156, 122, 32, "Order No"));
            form.Add("label2",     new WidgetLabel(150, 196, 122, 32, "User Name"));
            form.Add("label3",     new WidgetLabel(150, 236, 122, 32, "Title"));
            form.Add("label4",     new WidgetLabel(150, 276, 122, 32, "Order Date"));
            form.Add("label5",     new WidgetLabel(150, 316, 122, 32, "Return Date"));
            form.Add("username",   new WidgetTextBox(272, 192, 320, 32, "", true, false, false, 1));
            form.Add("title",      new WidgetTextBox(272, 232, 320, 32, "", true, false, false, 2));
            form.Add("orderdate",  new WidgetTextBox(272, 272, 320, 96, "", true, false, false, 3));
            form.Add("returndate", new WidgetTextBox(272, 312, 320, 96, "", true, false, false, 4));
            Form1.formgen.AddPage("order.form", form);
        }


        // ----------------------------------------------------------------- //
        // this method populates the fields within a form page.              //
        // ----------------------------------------------------------------- //
        public override void OnPopulateForm(string page) {
            Form1.formgen.BuildPage(page);
            Order order = new Order();
            if (page != "order.make") {
                order = Form1.context.GetOrder(Form1.context.GetSelectedOrder());
            }
            (Form1.formgen.GetControl(page, "orderno")    as TextBox).Text = order.GetOrderNo();
            (Form1.formgen.GetControl(page, "username")   as TextBox).Text = order.GetUserName();
            (Form1.formgen.GetControl(page, "title")      as TextBox).Text = order.GetTitle();
            (Form1.formgen.GetControl(page, "orderdate")  as TextBox).Text = order.GetOrderDate();
            (Form1.formgen.GetControl(page, "returndate") as TextBox).Text = order.GetReturnDate();

            Button return_button = Form1.formgen.GetControl("order.view", "return") as Button;
            Button cancel_button = Form1.formgen.GetControl("order.view", "cancel") as Button;
            if (order.GetReturnDate() == "") {
                return_button.Show();
                cancel_button.Hide();
            } else {
                cancel_button.Show();
                return_button.Hide();
            }
        }


        // ----------------------------------------------------------------- //
        // this method sanitizes values obtained from the form, and will not //
        // apply changes unless all values pass the sanity test.             //
        // ----------------------------------------------------------------- //
        public override bool Sanitize(string page) {
            // get values from form
            string orderno    = (Form1.formgen.GetControl(page, "orderno")    as TextBox).Text;
            string username   = (Form1.formgen.GetControl(page, "username")   as TextBox).Text;
            string title      = (Form1.formgen.GetControl(page, "title")      as TextBox).Text;
            string orderdate  = (Form1.formgen.GetControl(page, "orderdate")  as TextBox).Text;
            string returndate = (Form1.formgen.GetControl(page, "returndate") as TextBox).Text;
            
            #region sanitize
            Regex regexdate = new Regex(@"^[0-9]{1,2}[-/.]{1}[0-9]{1,2}[-/.]{1}[0-9]{2,4}");
            Regex regexmail = new Regex(@"^[a-zA-Z0-9]{1,10}@[a-zA-Z]{1,10}.(com|org)$");
            Regex regexurls = new Regex(@"^www[.][a-z]{1,15}[.](com|org)$");
            
            // is the date of birth a valid date?
            if (!regexdate.Match(orderdate).Success) {
                MessageBox.Show("Order Date is not valid!");
                return false;
            }

            // is the date of birth a valid date?
            if (returndate != null && !regexdate.Match(orderdate).Success) {
                MessageBox.Show("Return Date is not valid!");
                return false;
            }

            // does the email look valid?
            if (Form1.context.GetMember(username) == null) {
                MessageBox.Show("Member not found!");
                return false;
            }

            // does the email look valid?
            if (Form1.context.GetGame(title) == null) {
                MessageBox.Show("Game not found!");
                return false;
            }
            #endregion
            
            // if you get this far, then everything is golden
            Order order = new Order(orderno, username, title, orderdate, returndate);
            Form1.context.AddOrder(orderno, order);
            Form1.context.SetSelected("order", orderno);
            User user = Form1.context.GetMember(username);
            return true;
        }
    }
    #endregion


    #region viewpage
    [Serializable]
    public class OrderViewPage : Page {
        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public OrderViewPage()
        : base("order", "order.view") { }


        // ----------------------------------------------------------------- //
        // This method is invoked at startup. Here we define all of the form //
        // controls associated with game objects.                            //
        // ----------------------------------------------------------------- //
        public override void DefinePage() {
            // extra fields for view page
            Dictionary<string, Widget> view = new Dictionary<string, Widget>();
            view.Add("header",  new WidgetTitle("View Order"));
            view.Add("orderno", new WidgetTextBox(272, 152, 320, 32, "", true, false, false, 0));
            view.Add("edit",    new WidgetButton(464, 428, 128, 32, "Edit", OnViewEditClick, 5));
            view.Add("return",  new WidgetButton(272, 428, 128, 32, "Return", OnViewReturnClick, 6));
            view.Add("cancel",  new WidgetButton(272, 428, 128, 32, "Cancel", OnCancelClick, 6));
            Form1.formgen.AddPage("order.view", view);
        }


        // ----------------------------------------------------------------- //
        // Clicking order on the view page will reroute the user to an order //
        // page and populate the form with the fields from the entity.       //
        // ----------------------------------------------------------------- //
        public void OnViewReturnClick(object sender, EventArgs e) {
            Order order = Form1.context.GetSelected("order") as Order;
            if (order == null || order.GetReturnDate() != "") return;
            order.SetReturnDate(DateTime.Now.ToString("d/M/yyy"));

            // incerment stock
            string title = (Form1.formgen.GetControl("order.view", "title") as TextBox).Text;
            Game game = Form1.context.GetGame(title);
            if (game == null) {
                MessageBox.Show("Game not found! " + title);
                return;
            }
            game.SetStock(game.GetStock()+1);

            string pagename = typename + ".view";
            Form1.formgen.BuildPage(pagename);
            FormPage formpage = Form1.formgen.GetPage(typename + ".form") as FormPage;
            formpage.OnPopulateForm(pagename);
        }
    }
    #endregion


    #region editpage
    [Serializable]
    public class OrderEditPage : Page {
        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public OrderEditPage()
        : base("order", "order.edit") { }


        // ----------------------------------------------------------------- //
        // This method is invoked at startup. Here we define all of the form //
        // controls associated with game objects.                            //
        // ----------------------------------------------------------------- //
        public override void DefinePage() {
            // extra fields for edit page
            Dictionary<string, Widget> edit = new Dictionary<string, Widget>();
            edit.Add("header",  new WidgetTitle("Edit Order"));
            edit.Add("orderno", new WidgetTextBox(272, 152, 320, 32, "", true, false, false, 0));
            edit.Add("submit",  new WidgetButton(464, 428, 128, 32, "Submit", OnEditSubmitClick, 5));
            edit.Add("cancel",  new WidgetButton(272, 428, 128, 32, "Cancel", OnCancelClick, 6));
            Form1.formgen.AddPage("order.edit", edit);
        }


        // ----------------------------------------------------------------- //
        // this method populates a game object from the fields within a form //
        // ----------------------------------------------------------------- //
        public void OnEditSubmitClick(object sender, EventArgs e) {
            FormPage form = Form1.formgen.GetPage("order.form") as FormPage;
            if (form != null && form.Sanitize("order.make") == true) {
                Form1.formgen.BuildPage("order.list");
            }
        }
    }
    #endregion


    #region makepage
    [Serializable]
    public class OrderMakePage : Page {
        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public OrderMakePage()
        : base("order", "order.make") { }


        // ----------------------------------------------------------------- //
        // This method is invoked at startup. Here we define all of the form //
        // controls associated with game objects.                            //
        // ----------------------------------------------------------------- //
        public override void DefinePage() {
            // extra fields for make page
            Dictionary<string, Widget> make = new Dictionary<string, Widget>();
            make.Add("header",  new WidgetTitle("New Order"));
            make.Add("orderno", new WidgetTextBox(272, 152, 320, 32, "", false, false, false, 0));
            make.Add("submit",  new WidgetButton(464, 428, 128, 32, "Submit", OnMakeSubmitClick, 5));
            make.Add("cancel",  new WidgetButton(272, 428, 128, 32, "Cancel", OnCancelClick, 6));
            Form1.formgen.AddPage("order.make", make);
        }


        // ----------------------------------------------------------------- //
        // This method is invoked on display, now we perform any last second //
        // adjustments to the controls associated with this page.            //
        // ----------------------------------------------------------------- //
        public override void OnLoadPage() {
            DateTime now = DateTime.Now;
            string today   = now.ToShortDateString();
            string orderno = "x" + Form1.context.orders.Count().ToString("000");

            string pagename = typename + ".make";
            (Form1.formgen.GetControl(pagename, "orderno")    as TextBox).Text = orderno;
            (Form1.formgen.GetControl(pagename, "orderdate")  as TextBox).Text = today;
        }


        // ----------------------------------------------------------------- //
        // this method creates a game object from the fields within a form.  //
        // ----------------------------------------------------------------- //
        public void OnMakeSubmitClick(object sender, EventArgs e) {
            string title = (Form1.formgen.GetControl("order.make", "title") as TextBox).Text;
            Game game = Form1.context.GetGame(title);
            if (game == null) {
                MessageBox.Show("Game not found! " + title);
                return;
            }
            if (game.GetStock() <= 0) {
                MessageBox.Show("Game is out of stock! " + title);
                return;
            }

            // decerment stock
            FormPage form = Form1.formgen.GetPage("order.form") as FormPage;
            if (form != null && form.Sanitize("order.make") == true) {
                game.SetStock(game.GetStock()-1);
                Form1.formgen.BuildPage("order.list");
            }
        }
    }
    #endregion


    #region listpage
    [Serializable]
    public class OrderListPage : ListPage {
        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public OrderListPage()
        : base("order", "order.list") { }


        // ----------------------------------------------------------------- //
        // This method is invoked at startup. Here we define all of the form //
        // controls associated with order objects.                           //
        // ----------------------------------------------------------------- //
        public override void DefinePage() {
            // list orders page
            Dictionary<string, Widget> list = new Dictionary<string, Widget>();
            list = new Dictionary<string, Widget>();
            list.Add("header",    new WidgetTitle("Orders"));
            list.Add("listview",  new WidgetListView(32, 132, 800-64, 400-28, OnListRecordClick, ListViewMakeVisible));
            list.Add("label1",    new WidgetLabel(32, 536, 80, 32, "Search"));
            list.Add("search",    new WidgetTextBox(112, 534, 368, 32, "", false, false, false, 0));
            list.Add("addnew",    new WidgetButton(800-32-128, 532, 128, 32, "Add New", OnListAddNewClick, 5));
            list.Add("overdue",   new WidgetButton(800-32-256-16, 532, 128, 32, "Overdue",  OnListOverdueClick, 6));

            Form1.formgen.AddPage("order.list", list);
            TextBox search = Form1.formgen.GetControl("order.list", "search") as TextBox;
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
            listview.Columns.Add("Order No",    100);
            listview.Columns.Add("User Name",   100);
            listview.Columns.Add("Title",       100);
            listview.Columns.Add("Order Date",  100);
            listview.Columns.Add("Return Date", 100);
        }


        // ----------------------------------------------------------------- //
        // this method enumerates the order list and populates listview rows //
        // ----------------------------------------------------------------- //
        public override void OnPopulateListRecords(ListView listview) {
            listview.Items.Clear();
            foreach (KeyValuePair<string, Order> order in Form1.context.orders) {
                PopulateListItem(listview, order.Value);
            }
        }


        // ----------------------------------------------------------------- //
        // this method gets invoked automatically by the list view control   //
        // any time the list view needs to be updated.                       //
        // All this method does is to assign a values to each field in the   //
        // list view record.                                                 //
        // ----------------------------------------------------------------- //
        public bool PopulateListItem(ListView listview, Order order) {
            // the number of values passed must match the number of columns
            // defined in PopulateListColumns
            string[] fields = new[] {
                order.GetOrderNo(), order.GetUserName(), order.GetTitle(),
                order.GetOrderDate(), order.GetReturnDate()
            };
            listview.Items.Add(new ListViewItem(fields));
            return true;
        }


        // ----------------------------------------------------------------- //
        // this method gets invoked automatically by the list view control   //
        // any time the list view needs to be updated.                       //
        // All this method does is to assign a values to each field in the   //
        // list view record.                                                 //
        // ----------------------------------------------------------------- //
        public void OnListOverdueClick(object sender, EventArgs e) {
            ListView listview = Form1.formgen.GetControl("order.list", "listview") as ListView;
            listview.Items.Clear();
            foreach (KeyValuePair<string, Order> order in Form1.context.orders) {
                if (order.Value.GetReturnDate() != "") continue;

                // calculate time in terms of seconds since unix epoch
                double today_date = DateTimeToUnixTimestamp(DateTime.ParseExact(DateTime.Now.ToString("d/M/yyy"), "d/M/yyyy", CultureInfo.InvariantCulture));
                double order_date = DateTimeToUnixTimestamp(DateTime.ParseExact(order.Value.GetOrderDate(),       "d/M/yyyy", CultureInfo.InvariantCulture));

                // time_lapse = total seconds since order was placed and today.
                double time_lapse = today_date - order_date;

                // now we calculate how many seconds there are in 3 days
                double seconds = 1;
                double minutes = 60*seconds;
                double hours   = 60*minutes;
                double days    = 24*hours;
                if (time_lapse > 3*days) {
                    PopulateListItem(listview, order.Value);
                }
            }
        }

        // Converts a DateTime to the number of seconds passed since the unix epoch (1/1/1970 00:00:00)
        public static double DateTimeToUnixTimestamp(DateTime dateTime) {
            return (TimeZoneInfo.ConvertTimeToUtc(dateTime) - new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds;
        }
    }
    #endregion
}