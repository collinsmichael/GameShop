// ========================================================================= //
// File Name : UserPages.cs                                                  //
// File Date : 12 April 2016                                                 //
// Author(s) : Michael Collins, Louise McKeown, Alan Redding                 //
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
            form.Add("label1",    new WidgetLabel(150, 196, 122, 32, "User Name"));
            form.Add("label2",    new WidgetLabel(150, 236, 122, 32, "Password"));
            form.Add("label3",    new WidgetLabel(150, 276, 122, 32, "First Name"));
            form.Add("label4",    new WidgetLabel(150, 316, 122, 32, "Surname"));
            form.Add("label5",    new WidgetLabel(150, 356, 122, 32, "Email"));
            form.Add("label6",    new WidgetLabel(150, 396, 122, 32, "Phone No"));
            form.Add("label7",    new WidgetLabel(150, 436, 122, 32, "Address"));
            form.Add("password",  new WidgetTextBox(272, 236, 320, 32, "", false, false, true));
            form.Add("firstname", new WidgetTextBox(272, 276, 320, 32, "", false, false, false));
            form.Add("surname",   new WidgetTextBox(272, 316, 320, 32, "", false, false, false));
            form.Add("email",     new WidgetTextBox(272, 356, 320, 32, "", false, false, false));
            form.Add("phone",     new WidgetTextBox(272, 396, 320, 32, "", false, false, false));
            form.Add("address",   new WidgetTextBox(272, 436, 320, 64, "", false, true,  false));
            Form1.formgen.AddPage("user.form", form);
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
            User user = new User();
            if (page != "user.make") {
                user = Form1.context.GetUser(Form1.context.GetSelectedUser());
            }
            (Form1.formgen.GetControl(page, "username")  as TextBox).Text = user.GetUserName();
            (Form1.formgen.GetControl(page, "password")  as TextBox).Text = user.GetPassWord();
            (Form1.formgen.GetControl(page, "firstname") as TextBox).Text = user.GetFirstName();
            (Form1.formgen.GetControl(page, "surname")   as TextBox).Text = user.GetSurname();
            (Form1.formgen.GetControl(page, "email")     as TextBox).Text = user.GetEmail();
            (Form1.formgen.GetControl(page, "phone")     as TextBox).Text = user.GetPhoneNo();
            (Form1.formgen.GetControl(page, "address")   as TextBox).Text = user.GetAddress();
        }
    }
    #endregion


    #region viewpage
    [Serializable]
    public class UserViewPage : Page {
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
            view.Add("header",    new WidgetTitle("View User"));
            view.Add("username",  new WidgetTextBox(272, 196, 320, 32, "", true, false, false));
            view.Add("edit",      new WidgetButton(464, 512, 128, 32, "Edit", OnViewEditClick));
            view.Add("cancel",   new WidgetButton(272, 512, 128, 32, "Cancel", OnCancelClick));
            Form1.formgen.AddPage("user.view", view);
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
    }
    #endregion


    #region editpage
    [Serializable]
    public class UserEditPage : Page {
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
            edit.Add("header",    new WidgetTitle("Edit User"));
            edit.Add("username",  new WidgetTextBox(272, 196, 320, 32, "", true,  false, false));
            edit.Add("submit",    new WidgetButton(464, 512, 128, 32, "Submit", OnEditSubmitClick));
            edit.Add("cancel",    new WidgetButton(272, 512, 128, 32, "Cancel", OnCancelClick));
            Form1.formgen.AddPage("user.edit", edit);
        }


        // ----------------------------------------------------------------- //
        // This method is invoked on display, now we perform any last second //
        // adjustments to the controls associated with this page.            //
        // ----------------------------------------------------------------- //
        public override void DisplayPage() {
        }


        // ----------------------------------------------------------------- //
        // this method populates a user object from the fields within a form //
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

            User user = Form1.context.GetUser(Form1.context.GetSelectedUser());
            user.SetUserName((Form1.formgen.GetControl("user.edit", "username") as TextBox).Text);
            user.SetPassWord((Form1.formgen.GetControl("user.edit", "password") as TextBox).Text);
            user.SetFirstName((Form1.formgen.GetControl("user.edit", "firstname") as TextBox).Text);
            user.SetSurname((Form1.formgen.GetControl("user.edit", "surname") as TextBox).Text);
            user.SetEmail((Form1.formgen.GetControl("user.edit", "email") as TextBox).Text);
            user.SetPhoneNo((Form1.formgen.GetControl("user.edit", "phone") as TextBox).Text);
            user.SetAddress((Form1.formgen.GetControl("user.edit", "address") as TextBox).Text);

            Form1.context.SetSelected("user", user.GetUserName());
            Form1.formgen.BuildPage("user.list");
        }
    }
    #endregion


    #region makepage
    [Serializable]
    public class UserMakePage : Page {
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
            make.Add("header",    new WidgetTitle("New User"));
            make.Add("username",  new WidgetTextBox(272, 196, 320, 32, "", false, false, false));
            make.Add("submit",    new WidgetButton(464, 512, 128, 32, "Submit", OnMakeSubmitClick));
            make.Add("cancel",    new WidgetButton(272, 512, 128, 32, "Cancel", OnCancelClick));
            Form1.formgen.AddPage("user.make", make);
        }


        // ----------------------------------------------------------------- //
        // This method is invoked on display, now we perform any last second //
        // adjustments to the controls associated with this page.            //
        // ----------------------------------------------------------------- //
        public override void DisplayPage() {
        }


        // ----------------------------------------------------------------- //
        // this method creates a user object from the fields within a form.  //
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

            User user = new User();
            user.SetUserName((Form1.formgen.GetControl("user.make", "username") as TextBox).Text);
            user.SetPassWord((Form1.formgen.GetControl("user.make", "password") as TextBox).Text);
            user.SetFirstName((Form1.formgen.GetControl("user.make", "firstname") as TextBox).Text);
            user.SetSurname((Form1.formgen.GetControl("user.make", "surname") as TextBox).Text);
            user.SetEmail((Form1.formgen.GetControl("user.make", "email") as TextBox).Text);
            user.SetPhoneNo((Form1.formgen.GetControl("user.make", "phone") as TextBox).Text);
            user.SetAddress((Form1.formgen.GetControl("user.make", "address") as TextBox).Text);

            Form1.context.AddUser(user.GetUserName(), user);
            Form1.context.SetSelected("user", user.GetUserName());
            Form1.formgen.BuildPage("user.list");
        }
    }
    #endregion


    #region droppage
    [Serializable]
    public class UserDropPage : Page {
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
            drop.Add("header",    new WidgetTitle("Delete User"));
            drop.Add("username",  new WidgetTextBox(272, 196, 320, 32, "", true, false, false));
            drop.Add("delete",    new WidgetButton(464, 512, 128, 32, "Delete", OnDropDeleteClick));
            drop.Add("cancel",    new WidgetButton(272, 512, 128, 32, "Cancel", OnCancelClick));
            Form1.formgen.AddPage("user.drop", drop);
        }


        // ----------------------------------------------------------------- //
        // This method is invoked on display, now we perform any last second //
        // adjustments to the controls associated with this page.            //
        // ----------------------------------------------------------------- //
        public override void DisplayPage() {
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
                Form1.context.users.Remove(username);
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
            list.Add("header",    new WidgetTitle("Users"));
            list.Add("listview",  new WidgetListView(32, 132, 800-64, 400-28, OnListRecordClick, ListViewMakeVisible));
            list.Add("label1",    new WidgetLabel(32, 536, 80, 32, "Search"));
            list.Add("search",    new WidgetTextBox(112, 534, 368, 32, "", false, false, false));
            list.Add("addnew",    new WidgetButton(800-32-128, 532, 128, 32, "Add New", OnListAddNewClick));
            list.Add("delete",    new WidgetButton(800-32-256-16, 532, 128, 32, "Delete",  OnListDeleteClick));

            Form1.formgen.AddPage("user.list", list);
            TextBox search = Form1.formgen.GetControl("user.list", "search") as TextBox;
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
            listview.Columns.Add("UserName",  100);
            listview.Columns.Add("FirstName", 100);
            listview.Columns.Add("Surname",   100);
            listview.Columns.Add("Email",     100);
            listview.Columns.Add("Phone",     100);
            listview.Columns.Add("Address",   100);
        }


        // ----------------------------------------------------------------- //
        // this method enumerates the user list and populates listview rows. //
        // ----------------------------------------------------------------- //
        public override void OnPopulateListRecords(ListView listview) {
            listview.Items.Clear();
            foreach (KeyValuePair<string, User> user in Form1.context.users) {
                PopulateListItem(listview, user.Value);
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
                user.GetAddress()
            };
            listview.Items.Add(new ListViewItem(fields));
            return true;
        }
    }
    #endregion


}