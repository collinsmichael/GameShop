// ========================================================================= //
// File Name : StaffFormPage.cs                                              //
// File Date : 12 April 2016                                                 //
// Author(s) : Michael Collins, Louise McKeown, Alan Rowlands                //
// File Info : The StaffFormPage defines staff user interaction interface.   //
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
    public class StaffFormPage : FormPage {

        public StaffFormPage()
        : base("staff", "staff.form") { }

        // ----------------------------------------------------------------- //
        // This method is invoked at startup. Here we define all of the form //
        // controls associated with user objects.                            //
        // ----------------------------------------------------------------- //
        public override void DefinePage() {
            // basic form fields
            Dictionary<string, Widget> form = new Dictionary<string, Widget>();

            form.Add("label2", new WidgetLabel(140, 146, 132, 32, "User Name"));
            form.Add("label3", new WidgetLabel(140, 186, 132, 32, "First Name"));
            form.Add("label4", new WidgetLabel(140, 226, 132, 32, "Surname"));
            form.Add("label5", new WidgetLabel(140, 266, 132, 32, "Email"));
            form.Add("label6", new WidgetLabel(140, 306, 132, 32, "Phone No"));
            form.Add("label8", new WidgetLabel(140, 346, 132, 32, "Date of Birth"));
            form.Add("label7", new WidgetLabel(140, 386, 132, 32, "Address"));
            form.Add("label9", new WidgetLabel(140, 476, 132, 32, "Password"));
            
            form.Add("firstname",   new WidgetTextBox(272, 182, 320, 32, "", false, false, false, 2));
            form.Add("surname",     new WidgetTextBox(272, 222, 320, 32, "", false, false, false, 3));
            form.Add("email",       new WidgetTextBox(272, 262, 320, 32, "", false, false, false, 4));
            form.Add("phone",       new WidgetTextBox(272, 302, 320, 32, "", false, false, false, 5));
            form.Add("dateofbirth", new WidgetTextBox(272, 342, 320, 32, "", false, false, false, 6));
            form.Add("address",     new WidgetTextBox(272, 382, 320, 64, "", false, true, false, 7));
            form.Add("password",    new WidgetTextBox(272, 472, 320, 32, "", false, false, true, 8));
            Form1.formgen.AddPage("staff.form", form);
        }

        
        // ----------------------------------------------------------------- //
        // this method populates the fields within a form page.              //
        // ----------------------------------------------------------------- //
        public override void OnPopulateForm(string page) {
            Form1.formgen.BuildPage(page);
            Staff staff = new Staff();
            if (page != "staff.make") {
                staff = Form1.context.GetSelected("staff") as Staff;
            }

            (Form1.formgen.GetControl(page, "username")    as TextBox).Text = staff.GetUserName();
            (Form1.formgen.GetControl(page, "firstname")   as TextBox).Text = staff.GetFirstName();
            (Form1.formgen.GetControl(page, "surname")     as TextBox).Text = staff.GetSurname();
            (Form1.formgen.GetControl(page, "email")       as TextBox).Text = staff.GetEmail();
            (Form1.formgen.GetControl(page, "phone")       as TextBox).Text = staff.GetPhoneNo();
            (Form1.formgen.GetControl(page, "address")     as TextBox).Text = staff.GetAddress();
            (Form1.formgen.GetControl(page, "dateofbirth") as TextBox).Text = staff.GetDateOfBirth();
            (Form1.formgen.GetControl(page, "password")    as TextBox).Text = staff.GetPassWord();
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
            string phoneno     = (Form1.formgen.GetControl(page, "phone")       as TextBox).Text;
            string address     = (Form1.formgen.GetControl(page, "address")     as TextBox).Text;
            string dateofbirth = (Form1.formgen.GetControl(page, "dateofbirth") as TextBox).Text;
            string staffid     = (Form1.formgen.GetControl(page, "staffid")     as TextBox).Text;
            string password    = (Form1.formgen.GetControl(page, "password")    as TextBox).Text;
            
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
            Staff staff = new Staff(username, password, firstname, surname, email, address, phoneno, dateofbirth);
            Form1.context.AddStaff(username, staff);
            Form1.context.SetSelected("staff", username);
            return true;
        }
    }
    #endregion


    #region viewpage
    [Serializable]
    public class StaffViewPage : Page {
        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public StaffViewPage()
        : base("staff", "staff.view") { }


        // ----------------------------------------------------------------- //
        // This method is invoked at startup. Here we define all of the form //
        // controls associated with user objects.                            //
        // ----------------------------------------------------------------- //
        public override void DefinePage() {
            // extra fields for view page
            Dictionary<string, Widget> view = new Dictionary<string, Widget>();
            view.Add("header", new WidgetTitle("View Staff"));
            view.Add("username", new WidgetTextBox(272, 142, 320, 32, "", true, false, false, 0));
            view.Add("edit", new WidgetButton(464, 552, 128, 32, "Edit", OnViewEditClick, 9));
            view.Add("cancel", new WidgetButton(272, 552, 128, 32, "Cancel", OnCancelClick, 10));
            Form1.formgen.AddPage("staff.view", view);
        }
    }
    #endregion


    #region editpage
    [Serializable]
    public class StaffEditPage : Page {
        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public StaffEditPage()
        : base("staff", "staff.edit") { }


        // ----------------------------------------------------------------- //
        // This method is invoked at startup. Here we define all of the form //
        // controls associated with user objects.                            //
        // ----------------------------------------------------------------- //
        public override void DefinePage() {
            // extra fields for edit page
            Dictionary<string, Widget> edit = new Dictionary<string, Widget>();
            edit.Add("header", new WidgetTitle("Edit Staff"));
            edit.Add("username", new WidgetTextBox(272, 142, 320, 32, "", true, false, false, 0));
            edit.Add("submit", new WidgetButton(464, 552, 128, 32, "Submit", OnEditSubmitClick, 9));
            edit.Add("cancel", new WidgetButton(272, 552, 128, 32, "Cancel", OnCancelClick, 10));
            Form1.formgen.AddPage("staff.edit", edit);
        }


        // ----------------------------------------------------------------- //
        // this method populates a user object from the fields within a form //
        // ----------------------------------------------------------------- //
        public void OnEditSubmitClick(object sender, EventArgs e) {
            FormPage form = Form1.formgen.GetPage("staff.form") as FormPage;
            if (form != null && form.Sanitize("staff.edit") == true) {
                Form1.formgen.BuildPage("staff.list");
            }
        }
    }
    #endregion


    #region makepage
    [Serializable]
    public class StaffMakePage : Page {
        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public StaffMakePage()
        : base("staff", "staff.make") { }


        // ----------------------------------------------------------------- //
        // This method is invoked at startup. Here we define all of the form //
        // controls associated with user objects.                            //
        // ----------------------------------------------------------------- //
        public override void DefinePage() {
            // extra fields for make page
            Dictionary<string, Widget> make = new Dictionary<string, Widget>();
            make.Add("header", new WidgetTitle("New Staff"));
            make.Add("username", new WidgetTextBox(272, 142, 320, 32, "", false, false, false, 0));
            make.Add("submit", new WidgetButton(464, 552, 128, 32, "Submit", OnMakeSubmitClick, 9));
            make.Add("cancel", new WidgetButton(272, 552, 128, 32, "Cancel", OnCancelClick, 10));
            Form1.formgen.AddPage("staff.make", make);
        }


        // ----------------------------------------------------------------- //
        // This method is invoked on display, now we perform any last second //
        // adjustments to the controls associated with this page.            //
        // ----------------------------------------------------------------- //
        public override void OnLoadPage() {
            string staffid = "z" + Form1.context.staffs.Count().ToString("000");

            string pagename = "staff.make";
            (Form1.formgen.GetControl(pagename, "staffid") as TextBox).Text = staffid;
        }


        // ----------------------------------------------------------------- //
        // this method creates a user object from the fields within a form.  //
        // ----------------------------------------------------------------- //
        public void OnMakeSubmitClick(object sender, EventArgs e) {
            FormPage form = Form1.formgen.GetPage("staff.form") as FormPage;
            if (form != null && form.Sanitize("staff.make") == true) {
                Form1.formgen.BuildPage("staff.list");
            }
        }
    }
    #endregion


    #region droppage
    [Serializable]
    public class StaffDropPage : Page {
        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public StaffDropPage()
        : base("staff", "staff.drop") { }


        // ----------------------------------------------------------------- //
        // This method is invoked at startup. Here we define all of the form //
        // controls associated with user objects.                            //
        // ----------------------------------------------------------------- //
        public override void DefinePage() {
            // extra fields for drop page
            Dictionary<string, Widget> drop = new Dictionary<string, Widget>();
            drop.Add("header", new WidgetTitle("Delete Staff"));
            drop.Add("username", new WidgetTextBox(272, 142, 320, 32, "", true, false, false, 0));
            drop.Add("delete", new WidgetButton(464, 552, 128, 32, "Delete", OnDropDeleteClick, 9));
            drop.Add("cancel", new WidgetButton(272, 552, 128, 32, "Cancel", OnCancelClick, 10));
            Form1.formgen.AddPage("staff.drop", drop);
        }


        // ----------------------------------------------------------------- //
        // this method deletes a user object after warning the user.         //
        // ----------------------------------------------------------------- //
        public void OnDropDeleteClick(object sender, EventArgs e) {
            string username = (Form1.formgen.GetControl("staff.drop", "username") as TextBox).Text;
            string firstname = (Form1.formgen.GetControl("staff.drop", "firstname") as TextBox).Text;
            string surname = (Form1.formgen.GetControl("staff.drop", "surname") as TextBox).Text;

            string message = string.Format("User Name = {0}\nFirst Name = {1}\nSurname = {2}\n"
                                          + "Are you sure you wish to\ndelete this staff member form the "
                                          + "database?", username, firstname, surname);
            DialogResult result = MessageBox.Show(message, "Warning", MessageBoxButtons.YesNoCancel);
            if (result == DialogResult.Yes) {
                Form1.context.staffs.Remove(username);
            }
            Form1.formgen.BuildPage("staff.list");
        }
    }
    #endregion


    #region listpage
    [Serializable]
    public class StaffListPage : ListPage {
        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public StaffListPage()
        : base("staff", "staff.list") { }


        // ----------------------------------------------------------------- //
        // This method is invoked at startup. Here we define all of the form //
        // controls associated with user objects.                            //
        // ----------------------------------------------------------------- //
        public override void DefinePage() {
            // list games page
            Dictionary<string, Widget> list = new Dictionary<string, Widget>();
            list.Add("header", new WidgetTitle("Staff"));
            list.Add("listview", new WidgetListView(32, 132, 800 - 64, 400 - 28, OnListRecordClick, ListViewMakeVisible));
            list.Add("label1", new WidgetLabel(32, 536, 80, 32, "Search"));
            list.Add("search", new WidgetTextBox(112, 534, 368, 32, "", false, false, false, 0));
            list.Add("addnew", new WidgetButton(800 - 32 - 128, 532, 128, 32, "Add New", OnListAddNewClick, 1));
            list.Add("delete", new WidgetButton(800 - 32 - 256 - 16, 532, 128, 32, "Delete", OnListDeleteClick, 2));

            Form1.formgen.AddPage("staff.list", list);
            TextBox search = Form1.formgen.GetControl("staff.list", "search") as TextBox;
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
            listview.Columns.Add("Username", 100);
            listview.Columns.Add("First Name", 120);
            listview.Columns.Add("Surname", 100);
            listview.Columns.Add("Email", 80);
            listview.Columns.Add("Phone", 80);
            listview.Columns.Add("Address", 100);
            listview.Columns.Add("DOB", 60);
        }


        // ----------------------------------------------------------------- //
        // this method enumerates the user list and populates listview rows. //
        // ----------------------------------------------------------------- //
        public override void OnPopulateListRecords(ListView listview) {
            listview.Items.Clear();
            foreach (KeyValuePair<string, Staff> staff in Form1.context.staffs) {
                PopulateListItem(listview, staff.Value);
            }
        }


        // ----------------------------------------------------------------- //
        // this method gets invoked automatically by the list view control   //
        // any time the list view needs to be updated.                       //
        // All this method does is to assign a values to each field in the   //
        // list view record.                                                 //
        // ----------------------------------------------------------------- //
        public bool PopulateListItem(ListView listview, Staff staff) {
            // the number of values passed must match the number of columns
            // defined in PopulateListColumns
            string[] fields = new string[] {
                staff.GetUserName(), staff.GetFirstName(),
                staff.GetSurname(), staff.GetEmail(), staff.GetPhoneNo(),
                staff.GetAddress(), staff.GetDateOfBirth()
            };
            listview.Items.Add(new ListViewItem(fields));
            return true;
        }
    }
    #endregion
}