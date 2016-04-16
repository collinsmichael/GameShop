// ========================================================================= //
// File Name : OrderPages.cs                                                 //
// File Date : 16 April 2016                                                 //
// Author(s) : Michael Collins, Louise McKeown, Alan Redding                 //
// File Info : The Order Pages are responsible for representing the Orders.  //
//             Orders kee track of which members have reserved games. Orders //
//             have a different page for each of the following tasks.        //
//                 o order.list  (list box form)                             //
//                 o order.form  (game member controls)                      //
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
            form.Add("label1", new WidgetLabel(150, 196, 122, 32, "Order No"));
            form.Add("label2", new WidgetLabel(150, 236, 122, 32, "User Name"));
            form.Add("label3", new WidgetLabel(150, 276, 122, 32, "Title"));
            form.Add("label4", new WidgetLabel(150, 316, 122, 32, "Order Date"));
            form.Add("label5", new WidgetLabel(150, 356, 122, 32, "Return Date"));
            form.Add("username",   new WidgetTextBox(272, 236, 320, 32, "", true, false, false));
            form.Add("title",      new WidgetTextBox(272, 276, 320, 32, "", true, false, false));
            form.Add("orderdate",  new WidgetTextBox(272, 316, 320, 96, "", true, false, false));
            form.Add("returndate", new WidgetTextBox(272, 356, 320, 96, "", true, false, false));
            Form1.formgen.AddPage("order.form", form);
        }


        // ----------------------------------------------------------------- //
        // This method is invoked on display, now we perform any last second //
        // adjustments to the controls associated with this page.            //
        // ----------------------------------------------------------------- //
        public override void DisplayPage() {
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
            view.Add("orderno", new WidgetTextBox(272, 196, 320, 32, "", true, false, false));
            view.Add("edit",    new WidgetButton(464, 428, 128, 32, "Edit", OnViewEditClick));
            view.Add("return",  new WidgetButton(272, 428, 128, 32, "Return", OnViewReturnClick));
            view.Add("cancel",  new WidgetButton(272, 428, 128, 32, "Cancel", OnCancelClick));
            Form1.formgen.AddPage("order.view", view);
        }


        // ----------------------------------------------------------------- //
        // This method is invoked on display, now we perform any last second //
        // adjustments to the controls associated with this page.            //
        // ----------------------------------------------------------------- //
        public override void DisplayPage() {
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
        public void OnViewReturnClick(object sender, EventArgs e) {
            Order order = Form1.context.GetSelected("order") as Order;
            if (order == null || order.GetReturnDate() != "") return;
            DateTime now = DateTime.Now;
            order.SetReturnDate(now.ToShortDateString());

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
            edit.Add("orderno", new WidgetTextBox(272, 196, 320, 32, "", true, false, false));
            edit.Add("submit",  new WidgetButton(464, 428, 128, 32, "Submit", OnEditSubmitClick));
            edit.Add("cancel",  new WidgetButton(272, 428, 128, 32, "Cancel", OnCancelClick));
            Form1.formgen.AddPage("order.edit", edit);
        }


        // ----------------------------------------------------------------- //
        // This method is invoked on display, now we perform any last second //
        // adjustments to the controls associated with this page.            //
        // ----------------------------------------------------------------- //
        public override void DisplayPage() {
        }


        // ----------------------------------------------------------------- //
        // this method populates a game object from the fields within a form //
        // ----------------------------------------------------------------- //
        public void OnEditSubmitClick(object sender, EventArgs e) {
            /*
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

            Order order = Form1.context.GetOrder(Form1.context.GetSelectedGame());
            order.SetTitle((Form1.formgen.GetControl("order.edit", "title") as TextBox).Text);
            order.SetGenre((Form1.formgen.GetControl("order.edit", "genre") as TextBox).Text);
            order.SetInfo((Form1.formgen.GetControl("order.edit", "info") as TextBox).Text);

            Form1.context.SetSelected("order", order.GetTitle());
            Form1.formgen.BuildPage("order.list");

            DateTime now = DateTime.Now;
            order.SetReturnDate(now.ToShortDateString());
            */
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
            make.Add("orderno", new WidgetTextBox(272, 196, 320, 32, "", false, false, false));
            make.Add("submit",  new WidgetButton(464, 428, 128, 32, "Submit", OnMakeSubmitClick));
            make.Add("cancel",  new WidgetButton(272, 428, 128, 32, "Cancel", OnCancelClick));
            Form1.formgen.AddPage("order.make", make);
        }


        // ----------------------------------------------------------------- //
        // This method is invoked on display, now we perform any last second //
        // adjustments to the controls associated with this page.            //
        // ----------------------------------------------------------------- //
        public override void DisplayPage() {
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
            /*
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

            Order order = new Order();
            order.SetTitle((Form1.formgen.GetControl("order.make", "title") as TextBox).Text);
            order.SetGenre((Form1.formgen.GetControl("order.make", "genre") as TextBox).Text);
            order.SetInfo((Form1.formgen.GetControl("order.make", "info") as TextBox).Text);

            Form1.context.AddOrder(order.GetTitle(), order);
            Form1.context.SetSelected("order", order.GetTitle());
            Form1.formgen.BuildPage("order.list");
            */

            Order order = new Order();
            order.SetOrderNo((Form1.formgen.GetControl("order.make", "orderno") as TextBox).Text);
            order.SetUserName((Form1.formgen.GetControl("order.make", "username") as TextBox).Text);
            order.SetTitle((Form1.formgen.GetControl("order.make", "title") as TextBox).Text);
            order.SetOrderDate((Form1.formgen.GetControl("order.make", "orderdate") as TextBox).Text);

            Form1.context.AddOrder(order.GetOrderNo(), order);
            Form1.context.SetSelected("order", order.GetOrderNo());
            Form1.formgen.BuildPage("order.list");
        }
    }
    #endregion


    #region droppage
    [Serializable]
    public class OrderDropPage : Page {
        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public OrderDropPage()
        : base("order", "order.drop") { }


        // ----------------------------------------------------------------- //
        // This method is invoked at startup. Here we define all of the form //
        // controls associated with game objects.                            //
        // ----------------------------------------------------------------- //
        public override void DefinePage() {
            // extra fields for drop page
            Dictionary<string, Widget> drop = new Dictionary<string, Widget>();
            drop.Add("header",  new WidgetTitle("Delete Order"));
            drop.Add("orderno", new WidgetTextBox(272, 196, 320, 32, "", true, false, false));
            drop.Add("delete",  new WidgetButton(464, 428, 128, 32, "Delete", OnDropDeleteClick));
            drop.Add("cancel",  new WidgetButton(272, 428, 128, 32, "Cancel", OnCancelClick));
            Form1.formgen.AddPage("order.drop", drop);
        }


        // ----------------------------------------------------------------- //
        // This method is invoked on display, now we perform any last second //
        // adjustments to the controls associated with this page.            //
        // ----------------------------------------------------------------- //
        public override void DisplayPage() {
        }


        // ----------------------------------------------------------------- //
        // this method deletes a game object after warning the user.         //
        // ----------------------------------------------------------------- //
        public void OnDropDeleteClick(object sender, EventArgs e) {
            //string title = (Form1.formgen.GetControl("order.drop", "title") as TextBox).Text;
            //string genre = (Form1.formgen.GetControl("order.drop", "genre") as TextBox).Text;

            //string message = string.Format("Title = {0}\nGenre = {1}\nAre you sure you wish to\n"
            //                              +"delete this game form the database?", title, genre);
            //DialogResult result = MessageBox.Show(message, "Warning", MessageBoxButtons.YesNoCancel);
            //if (result == DialogResult.Yes) {
            //    Form1.context.orders.Remove(order);
            //}
            //Form1.formgen.BuildPage("order.list");
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
            list.Add("search",    new WidgetTextBox(112, 534, 368, 32, "", false, false, false));
            list.Add("addnew",    new WidgetButton(800-32-128, 532, 128, 32, "Add New", OnListAddNewClick));
            list.Add("delete",    new WidgetButton(800-32-256-16, 532, 128, 32, "Delete",  OnListDeleteClick));

            Form1.formgen.AddPage("order.list", list);
            TextBox search = Form1.formgen.GetControl("order.list", "search") as TextBox;
            if (search != null) search.TextChanged += OnUpdateSearch;
        }


        // ----------------------------------------------------------------- //
        // This method is invoked on display, now we perform any last second //
        // adjustments to the controls associated with this page.            //
        // ----------------------------------------------------------------- //
        public override void DisplayPage() {
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
    }
    #endregion
}