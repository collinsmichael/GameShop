// ========================================================================= //
// File Name : UserPages.cs                                                  //
// File Date : 12 April 2016                                                 //
// Author(s) : Michael Collins, Louise McKeown, Alan Rowlands                //
// File Info : The User Pages are responsible for representing the Users in  //
//             the form. There are a number of User Pages each responsible   //
//             for their own Form.                                           //
//                 o user.list  (list box form)                              //
//                 o user.form  (user member controls)                       //
//                 o user.view  (edit cancel buttons on the view form)       //
//                 o user.edit  (submit cancel buttons on the edit form)     //
//                 o user.make  (submit cancel buttons on the make form)     //
//                 o user.drop  (delete cancel buttons on the drop form)     //
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
    public class UserFormPage : FormPage {
        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public UserFormPage()
        : base("user", "user.form") { }


        // ----------------------------------------------------------------- //
        // This method is invoked at startup. Here we define all of the form //
        // controls associated with user objects.                            //
        // ----------------------------------------------------------------- //
        public override void DefinePage() {
            // basic form fields
            Dictionary<string, Widget> form = new Dictionary<string, Widget>();

            form.Add("label1",      new WidgetLabel(140, 156, 132, 32, "User Name"));
            form.Add("label2",      new WidgetLabel(140, 196, 132, 32, "First Name"));
            form.Add("label3",      new WidgetLabel(140, 236, 132, 32, "Surname"));
            form.Add("label4",      new WidgetLabel(140, 276, 132, 32, "Email"));
            form.Add("label5",      new WidgetLabel(140, 316, 132, 32, "Phone No"));
            form.Add("label6",      new WidgetLabel(140, 356, 132, 32, "Address"));
            form.Add("label7",      new WidgetLabel(140, 436, 132, 32, "Birth Date"));
            form.Add("firstname",   new WidgetTextBox(272, 192, 320, 32, "", false, false, false, 1));
            form.Add("surname",     new WidgetTextBox(272, 232, 320, 32, "", false, false, false, 2));
            form.Add("email",       new WidgetTextBox(272, 272, 320, 32, "", false, false, false, 3));
            form.Add("phoneno",     new WidgetTextBox(272, 312, 320, 32, "", false, false, false, 4));
            form.Add("address",     new WidgetTextBox(272, 352, 320, 64, "", false, true,  false, 5));
            form.Add("dateofbirth", new WidgetTextBox(272, 432, 320, 32, "", false, false, false, 6));
            Form1.formgen.AddPage("user.form", form);
        }


        // ----------------------------------------------------------------- //
        // this method populates the fields within a form page.              //
        // ----------------------------------------------------------------- //
        public override void OnPopulateForm(string page) {
            Form1.formgen.BuildPage(page);
            User user = new User();
            if (page != "user.make") {
                //user = Form1.context.GetUser(Form1.context.GetSelectedUser());
                user = Form1.context.GetSelected("user") as User;
            }
            (Form1.formgen.GetControl(page, "username")    as TextBox).Text = user.GetUserName();
            (Form1.formgen.GetControl(page, "firstname")   as TextBox).Text = user.GetFirstName();
            (Form1.formgen.GetControl(page, "surname")     as TextBox).Text = user.GetSurname();
            (Form1.formgen.GetControl(page, "email")       as TextBox).Text = user.GetEmail();
            (Form1.formgen.GetControl(page, "phoneno")     as TextBox).Text = user.GetPhoneNo();
            (Form1.formgen.GetControl(page, "address")     as TextBox).Text = user.GetAddress();
            (Form1.formgen.GetControl(page, "dateofbirth") as TextBox).Text = user.GetDateOfBirth();
        }


        // ----------------------------------------------------------------- //
        // this method sanitizes values obtained from the form, and will not //
        // apply changes unless all values pass the sanity test.             //
        // ----------------------------------------------------------------- //
        public override bool Sanitize(string page) {
            // get values from form
            string username    = (Form1.formgen.GetControl(page, "username")    as TextBox).Text;
            string firstname   = (Form1.formgen.GetControl(page, "firstname")   as TextBox).Text;
            string surname     = (Form1.formgen.GetControl(page, "surname")     as TextBox).Text;
            string email       = (Form1.formgen.GetControl(page, "email")       as TextBox).Text;
            string phoneno     = (Form1.formgen.GetControl(page, "phoneno")     as TextBox).Text;
            string address     = (Form1.formgen.GetControl(page, "address")     as TextBox).Text;
            string dateofbirth = (Form1.formgen.GetControl(page, "dateofbirth") as TextBox).Text;
            
            #region sanitize
            Regex regexdate = new Regex(@"^[0-9]{1,2}[-/.]{1}[0-9]{1,2}[-/.]{1}[0-9]{2,4}");
            Regex regexmail = new Regex(@"^[a-zA-Z0-9]{1,10}@[a-zA-Z]{1,10}.[a-zA-Z.-]{2,10}$");
            Regex regexurls = new Regex(@"^www[.][a-z]{1,15}[.](com|org)$");
            
            // is the date of birth a valid date?
            if ((!regexdate.Match(dateofbirth).Success)) {
                MessageBox.Show("Birth Date is not valid!");
                return false;
            }

            // does the email look valid?
            if (!regexmail.Match(email).Success) {
                MessageBox.Show("Email is invalid!");
                return false;
            }
            #endregion
            
            // if you get this far, then everything is golden
            Member member = new Member(username, firstname, surname, email, address, phoneno, dateofbirth);
            Form1.context.AddMember(username, member);
            Form1.context.SetSelected("member", username);
            return true;
        }
    }
    #endregion


    #region viewpage
    [Serializable]
    public class UserViewPage : ViewPage {
        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public UserViewPage()
        : base("user", "user.view") { }


        // ----------------------------------------------------------------- //
        // This method is invoked at startup. Here we define all of the form //
        // controls associated with user objects.                            //
        // ----------------------------------------------------------------- //
        public override void DefinePage() {
            // extra fields for view page
            Dictionary<string, Widget> view = new Dictionary<string, Widget>();
            view.Add("header",    new WidgetTitle("View Member"));
            view.Add("username",  new WidgetTextBox(272, 152, 320, 32, "", true, false, false, 0));
            view.Add("orders",    new WidgetButton(400-70-128, 532, 128, 32, "Orders", OnOrdersClick, 7));
            view.Add("report",    new WidgetButton(400-64,    532, 128, 32, "Reports", OnReportsClick, 8));
            view.Add("edit",      new WidgetButton(400+70,    532, 128, 32, "Edit", OnViewEditClick, 9));
            Form1.formgen.AddPage("user.view", view);
        }


        // ----------------------------------------------------------------- //
        // this method populates an order list specific to the user          //
        // ----------------------------------------------------------------- //
        public void OnOrdersClick(object sender, EventArgs e) {
            Form1.formgen.BuildPage("order.list");
            Form1.formgen.BuildPage("order.list");

            User user = Form1.context.GetSelected("user") as User;
            string username = user.GetUserName();

            string pattern = "^"+username+"$";
            TextBox search = Form1.formgen.GetControl("order.list", "search") as TextBox;
            search.Text = pattern;

            OrderListPage orderlist = Form1.formgen.GetPage("order.list") as OrderListPage;
            orderlist.RegexSearch(pattern);
        }


        // ----------------------------------------------------------------- //
        // this method populates a report list specific to the user          //
        // ----------------------------------------------------------------- //
        public void OnReportsClick(object sender, EventArgs e) {
            Form1.formgen.BuildPage("report.list");
            Form1.formgen.BuildPage("report.list");

            User user = Form1.context.GetSelected("user") as User;
            string username = user.GetUserName();

            string pattern = "^"+username+"$";
            TextBox search = Form1.formgen.GetControl("report.list", "search") as TextBox;
            search.Text = pattern;

            ReportListPage reportlist = Form1.formgen.GetPage("report.list") as ReportListPage;
            reportlist.RegexSearch(pattern);
        }
    }
    #endregion


    #region editpage
    [Serializable]
    public class UserEditPage : EditPage {
        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public UserEditPage()
        : base("user", "user.edit") { }


        // ----------------------------------------------------------------- //
        // This method is invoked at startup. Here we define all of the form //
        // controls associated with user objects.                            //
        // ----------------------------------------------------------------- //
        public override void DefinePage() {
            // extra fields for edit page
            Dictionary<string, Widget> edit = new Dictionary<string, Widget>();
            edit.Add("header",    new WidgetTitle("Edit Member"));
            edit.Add("username",  new WidgetTextBox(272, 152, 320, 32, "", true,  false, false, 0));
            edit.Add("submit",    new WidgetButton(464, 532, 128, 32, "Submit", OnEditSubmitClick, 7));
            edit.Add("cancel",    new WidgetButton(272, 532, 128, 32, "Cancel", OnCancelClick, 8));
            Form1.formgen.AddPage("user.edit", edit);
        }


        // ----------------------------------------------------------------- //
        // this method populates a user object from the fields within a form //
        // ----------------------------------------------------------------- //
        public void OnEditSubmitClick(object sender, EventArgs e) {
            FormPage form = Form1.formgen.GetPage("user.form") as FormPage;
            if (form != null && form.Sanitize("user.edit") == true) {
                Form1.formgen.BuildPage("user.list");
            }
        }
    }
    #endregion


    #region makepage
    [Serializable]
    public class UserMakePage : MakePage {
        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public UserMakePage()
        : base("user", "user.make") { }


        // ----------------------------------------------------------------- //
        // This method is invoked at startup. Here we define all of the form //
        // controls associated with user objects.                            //
        // ----------------------------------------------------------------- //
        public override void DefinePage() {
            // extra fields for make page
            Dictionary<string, Widget> make = new Dictionary<string, Widget>();
            make.Add("header",    new WidgetTitle("New Member"));
            make.Add("username",  new WidgetTextBox(272, 152, 320, 32, "", false, false, false, 0));
            make.Add("submit",    new WidgetButton(464, 532, 128, 32, "Submit", OnMakeSubmitClick, 7));
            make.Add("cancel",    new WidgetButton(272, 532, 128, 32, "Cancel", OnCancelClick, 8));
            Form1.formgen.AddPage("user.make", make);
        }


        // ----------------------------------------------------------------- //
        // this method creates a user object from the fields within a form.  //
        // ----------------------------------------------------------------- //
        public void OnMakeSubmitClick(object sender, EventArgs e) {
            FormPage form = Form1.formgen.GetPage("user.form") as FormPage;
            if (form != null && form.Sanitize("user.make") == true) {
                Form1.formgen.BuildPage("user.list");
            }
        }
    }
    #endregion


    #region droppage
    [Serializable]
    public class UserDropPage : DropPage {
        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public UserDropPage()
        : base("user", "user.drop") { }


        // ----------------------------------------------------------------- //
        // This method is invoked at startup. Here we define all of the form //
        // controls associated with user objects.                            //
        // ----------------------------------------------------------------- //
        public override void DefinePage() {
            // extra fields for drop page
            Dictionary<string, Widget> drop = new Dictionary<string, Widget>();
            drop.Add("header",    new WidgetTitle("Delete Member"));
            drop.Add("username",  new WidgetTextBox(272, 152, 320, 32, "", true, false, false, 0));
            drop.Add("delete",    new WidgetButton(464, 532, 128, 32, "Delete", OnDropDeleteClick, 7));
            drop.Add("cancel",    new WidgetButton(272, 532, 128, 32, "Cancel", OnCancelClick, 8));
            Form1.formgen.AddPage("user.drop", drop);
        }


        // ----------------------------------------------------------------- //
        // this method deletes a user object after warning the user.         //
        // ----------------------------------------------------------------- //
        public void OnDropDeleteClick(object sender, EventArgs e) {
            string username  = (Form1.formgen.GetControl("user.drop", "username") as TextBox).Text;
            string firstname = (Form1.formgen.GetControl("user.drop", "firstname") as TextBox).Text;
            string surname   = (Form1.formgen.GetControl("user.drop", "surname") as TextBox).Text;

            string message = string.Format("User Name = {0}\nFirst Name = {1}\nSurname = {2}\n"
                                          +"Are you sure you wish to\ndelete this user form the "
                                          +"database?", username, firstname, surname);
            DialogResult result = MessageBox.Show(message, "Warning", MessageBoxButtons.YesNoCancel);
            if (result == DialogResult.Yes) {
                Form1.context.members.Remove(username);
            }
            Form1.formgen.BuildPage("user.list");
        }
    }
    #endregion


    #region listpage
    [Serializable]
    public class UserListPage : ListPage {
        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public UserListPage()
        : base("user", "user.list") { }


        // ----------------------------------------------------------------- //
        // This method is invoked at startup. Here we define all of the form //
        // controls associated with user objects.                            //
        // ----------------------------------------------------------------- //
        public override void DefinePage() {
            // list games page
            Dictionary<string, Widget> list = new Dictionary<string, Widget>();
            list.Add("header",    new WidgetTitle("Members"));
            list.Add("listview",  new WidgetListView(32, 132, 800-64, 400-28, OnListRecordClick, ListViewMakeVisible));
            list.Add("label1",    new WidgetLabel(32, 536, 80, 32, "Search"));
            list.Add("search",    new WidgetTextBox(112, 534, 368, 32, "", false, false, false, 0));
            list.Add("addnew",    new WidgetButton(800-32-128, 532, 128, 32, "Add New", OnListAddNewClick, 1));
            list.Add("delete",    new WidgetButton(800-32-256-16, 532, 128, 32, "Delete",  OnListDeleteClick, 2));

            Form1.formgen.AddPage("user.list", list);
            TextBox search = Form1.formgen.GetControl("user.list", "search") as TextBox;
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
            listview.Columns.Add("UserName",  100);
            listview.Columns.Add("FirstName", 100);
            listview.Columns.Add("Surname",   100);
            listview.Columns.Add("Email",      80);
            listview.Columns.Add("Phone",      80);
            listview.Columns.Add("Address",   100);
            listview.Columns.Add("DOB",       100);
            listview.MouseClick += OnMouseClick;
        }

        private void OnMouseClick(object sender, MouseEventArgs e) {
            if (e.Button != MouseButtons.Right) return;
            ListView listview = sender as ListView;
            if (sender == null) return;;

            ContextMenu cm = new ContextMenu();
            cm.MenuItems.Add("View",    new EventHandler(OnClickView));
            cm.MenuItems.Add("Edit",    new EventHandler(OnClickEdit));
            cm.MenuItems.Add("Add New", new EventHandler(OnClickMake));
            cm.MenuItems.Add("Delete",  new EventHandler(OnClickDrop));
            listview.ContextMenu = cm;
        }
        private void OnClickView(object sender, EventArgs e) {
            Form1.formgen.ClearPage("user.list");
            FormPage formpage = Form1.formgen.GetPage("user.form") as FormPage;
            formpage.OnPopulateForm("user.view");
            Form1.formgen.BuildPage("user.view");
        }
        private void OnClickEdit(object sender, EventArgs e) {
            Form1.formgen.ClearPage("user.list");
            FormPage formpage = Form1.formgen.GetPage("user.form") as FormPage;
            formpage.OnPopulateForm("user.edit");
            Form1.formgen.BuildPage("user.edit");
        }
        private void OnClickMake(object sender, EventArgs e) {
            Form1.formgen.ClearPage("user.list");
            FormPage formpage = Form1.formgen.GetPage("user.form") as FormPage;
            formpage.OnPopulateForm("user.make");
            Form1.formgen.BuildPage("user.make");
        }
        private void OnClickDrop(object sender, EventArgs e) {
            Form1.formgen.ClearPage("user.list");
            FormPage formpage = Form1.formgen.GetPage("user.form") as FormPage;
            formpage.OnPopulateForm("user.drop");
            Form1.formgen.BuildPage("user.drop");
        }


        // ----------------------------------------------------------------- //
        // this method enumerates the user list and populates listview rows. //
        // ----------------------------------------------------------------- //
        public override void OnPopulateListRecords(ListView listview) {
            listview.Items.Clear();
            foreach (KeyValuePair<string, Member> member in Form1.context.members) {
                PopulateListItem(listview, member.Value);
            }
        }


        // ----------------------------------------------------------------- //
        // this method gets invoked automatically by the list view control   //
        // any time the list view needs to be updated.                       //
        // All this method does is to assign a values to each field in the   //
        // list view record.                                                 //
        // ----------------------------------------------------------------- //
        public bool PopulateListItem(ListView listview, User user) {
            // the number of values passed must match the number of columns
            // defined in PopulateListColumns
            string[] fields = new string[] {
                user.GetUserName(), user.GetFirstName(),
                user.GetSurname(), user.GetEmail(), user.GetPhoneNo(),
                user.GetAddress(), user.GetDateOfBirth()
            };
            listview.Items.Add(new ListViewItem(fields));
            return true;
        }
    }
    #endregion
}