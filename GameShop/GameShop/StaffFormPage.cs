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

namespace GameShop
{
    #region formpage
    [Serializable]
    public class StaffFormPage:FormPage
    {

          public StaffFormPage()
        : base("staff", "staff.form") { }

          // ----------------------------------------------------------------- //
          // This method is invoked at startup. Here we define all of the form //
          // controls associated with user objects.                            //
          // ----------------------------------------------------------------- //
          public override void DefinePage()
          {
              // basic form fields
              Dictionary<string, Widget> form = new Dictionary<string, Widget>();

              form.Add("label1", new WidgetLabel(150, 156, 122, 32, "Staff Id"));
              form.Add("label2", new WidgetLabel(150, 156, 122, 32, "User Name"));
              form.Add("label3", new WidgetLabel(150, 196, 122, 32, "First Name"));
              form.Add("label4", new WidgetLabel(150, 236, 122, 32, "Surname"));
              form.Add("label5", new WidgetLabel(150, 276, 122, 32, "Email"));
              form.Add("label6", new WidgetLabel(150, 316, 122, 32, "Phone No"));
              form.Add("label7", new WidgetLabel(150, 356, 122, 32, "Address"));
              form.Add("label8", new WidgetLabel(150, 436, 122, 32, "Date of Birth"));
              form.Add("label9", new WidgetLabel(150, 476, 122, 32, "Password"));
              form.Add("staffId", new WidgetTextBox(272, 192, 320, 32, "", false, false, false));
              form.Add("firstname", new WidgetTextBox(272, 192, 320, 32, "", false, false, false));
              form.Add("surname", new WidgetTextBox(272, 232, 320, 32, "", false, false, false));
              form.Add("email", new WidgetTextBox(272, 272, 320, 32, "", false, false, false));
              form.Add("phone", new WidgetTextBox(272, 312, 320, 32, "", false, false, false));
              form.Add("address", new WidgetTextBox(272, 352, 320, 64, "", false, true, false));
              form.Add("dateofbirth", new WidgetTextBox(272, 432, 320, 32, "", false, false, false));
              form.Add("password", new WidgetTextBox(272, 472, 320, 32, "", false, false, true));
              Form1.formgen.AddPage("staff.form", form);
          }

           // this method populates the fields within a form page.              //
        // ----------------------------------------------------------------- //
        public override void OnPopulateForm(string page) {
            Form1.formgen.BuildPage(page);
            Staff staff = new Staff();
            if (page != "staff.make") {
                staff = Form1.context.GetStaff(Form1.context.GetSelectedStaff());
            }
            (Form1.formgen.GetControl(page, "staffId") as TextBox).Text = staff.GetStaffId();
            (Form1.formgen.GetControl(page, "username")    as TextBox).Text = staff.GetUserName();
            (Form1.formgen.GetControl(page, "firstname")   as TextBox).Text = staff.GetFirstName();
            (Form1.formgen.GetControl(page, "surname")     as TextBox).Text = staff.GetSurname();
            (Form1.formgen.GetControl(page, "email")       as TextBox).Text = staff.GetEmail();
            (Form1.formgen.GetControl(page, "phone")       as TextBox).Text = staff.GetPhoneNo();
            (Form1.formgen.GetControl(page, "address")     as TextBox).Text = staff.GetAddress();
            (Form1.formgen.GetControl(page, "dateofbirth") as TextBox).Text = staff.GetDateOfBirth();
            (Form1.formgen.GetControl(page, "password")    as TextBox).Text = staff.GetPassWord();
        }
    }
    #endregion


    #region viewpage
    [Serializable]
    public class StaffViewPage : Page
    {
        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public StaffViewPage()
            : base("staff", "staff.view") { }


        // ----------------------------------------------------------------- //
        // This method is invoked at startup. Here we define all of the form //
        // controls associated with user objects.                            //
        // ----------------------------------------------------------------- //
        public override void DefinePage()
        {
            // extra fields for view page
            Dictionary<string, Widget> view = new Dictionary<string, Widget>();
            view.Add("header", new WidgetTitle("View Staff"));
            view.Add("username", new WidgetTextBox(272, 152, 320, 32, "", true, false, false));
            view.Add("edit", new WidgetButton(464, 532, 128, 32, "Edit", OnViewEditClick));
            view.Add("cancel", new WidgetButton(272, 532, 128, 32, "Cancel", OnCancelClick));
            Form1.formgen.AddPage("staff.view", view);
        }
    }
    #endregion


    #region editpage
    [Serializable]
    public class StaffEditPage : Page
    {
        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public StaffEditPage()
            : base("staff", "staff.edit") { }


        // ----------------------------------------------------------------- //
        // This method is invoked at startup. Here we define all of the form //
        // controls associated with user objects.                            //
        // ----------------------------------------------------------------- //
        public override void DefinePage()
        {
            // extra fields for edit page
            Dictionary<string, Widget> edit = new Dictionary<string, Widget>();
            edit.Add("header", new WidgetTitle("Edit User"));
            edit.Add("username", new WidgetTextBox(272, 152, 320, 32, "", true, false, false));
            edit.Add("submit", new WidgetButton(464, 532, 128, 32, "Submit", OnEditSubmitClick));
            edit.Add("cancel", new WidgetButton(272, 532, 128, 32, "Cancel", OnCancelClick));
            Form1.formgen.AddPage("staff.edit", edit);
        }


        // ----------------------------------------------------------------- //
        // this method populates a user object from the fields within a form //
        // ----------------------------------------------------------------- //
        public void OnEditSubmitClick(object sender, EventArgs e)
        {
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
            else MessageBox.Show("No Match");

            Match matchvalu = regexvalu.Match("123456");
            if (matchvalu.Success) MessageBox.Show(matchvalu.Value);
            else MessageBox.Show("No Match");

            Match matchdate = regexdate.Match("1-1-2000");
            if (matchdate.Success) MessageBox.Show(matchdate.Value);
            else MessageBox.Show("No Match");
            matchdate = regexdate.Match("1/1/2000");
            if (matchdate.Success) MessageBox.Show(matchdate.Value);
            else MessageBox.Show("No Match");
            matchdate = regexdate.Match("1.1.2000");
            if (matchdate.Success) MessageBox.Show(matchdate.Value);
            else MessageBox.Show("No Match");

            Match matchurls = regexurls.Match("www.domain.com");
            if (matchurls.Success) MessageBox.Show(matchurls.Value);
            else MessageBox.Show("No Match");

            Match matchmail = regexmail.Match("mike@collins.com");
            if (matchmail.Success) MessageBox.Show(matchmail.Value);
            else MessageBox.Show("No Match");
            #endregion

            Staff staff = Form1.context.GetStaff(Form1.context.GetSelectedStaff());
            staff.SetStaffId((Form1.formgen.GetControl("staff.edit", "staffId") as TextBox).Text);
            staff.SetUserName((Form1.formgen.GetControl("user.edit", "username") as TextBox).Text);
            staff.SetFirstName((Form1.formgen.GetControl("user.edit", "firstname") as TextBox).Text);
            staff.SetSurname((Form1.formgen.GetControl("user.edit", "surname") as TextBox).Text);
            staff.SetEmail((Form1.formgen.GetControl("user.edit", "email") as TextBox).Text);
            staff.SetPhoneNo((Form1.formgen.GetControl("user.edit", "phone") as TextBox).Text);
            staff.SetAddress((Form1.formgen.GetControl("user.edit", "address") as TextBox).Text);
            staff.SetDateOfBirth((Form1.formgen.GetControl("user.edit", "dateofbirth") as TextBox).Text);
            staff.SetPassWord((Form1.formgen.GetControl("user.edit", "password") as TextBox).Text);

            Form1.context.SetSelected("staff", staff.GetStaffId());
            Form1.formgen.BuildPage("staff.list");
        }
    }
    #endregion


    #region makepage
    [Serializable]
    public class StaffMakePage : Page
    {
        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public StaffMakePage()
            : base("staff", "staff.make") { }


        // ----------------------------------------------------------------- //
        // This method is invoked at startup. Here we define all of the form //
        // controls associated with user objects.                            //
        // ----------------------------------------------------------------- //
        public override void DefinePage()
        {
            // extra fields for make page
            Dictionary<string, Widget> make = new Dictionary<string, Widget>();
            make.Add("header", new WidgetTitle("New User"));
            make.Add("username", new WidgetTextBox(272, 152, 320, 32, "", false, false, false));
            make.Add("submit", new WidgetButton(464, 532, 128, 32, "Submit", OnMakeSubmitClick));
            make.Add("cancel", new WidgetButton(272, 532, 128, 32, "Cancel", OnCancelClick));
            Form1.formgen.AddPage("staff.make", make);
        }


        // ----------------------------------------------------------------- //
        // this method creates a user object from the fields within a form.  //
        // ----------------------------------------------------------------- //
        public void OnMakeSubmitClick(object sender, EventArgs e)
        {
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
            else MessageBox.Show("No Match");

            Match matchvalu = regexvalu.Match("123456");
            if (matchvalu.Success) MessageBox.Show(matchvalu.Value);
            else MessageBox.Show("No Match");

            Match matchdate = regexdate.Match("1-1-2000");
            if (matchdate.Success) MessageBox.Show(matchdate.Value);
            else MessageBox.Show("No Match");
            matchdate = regexdate.Match("1/1/2000");
            if (matchdate.Success) MessageBox.Show(matchdate.Value);
            else MessageBox.Show("No Match");
            matchdate = regexdate.Match("1.1.2000");
            if (matchdate.Success) MessageBox.Show(matchdate.Value);
            else MessageBox.Show("No Match");

            Match matchurls = regexurls.Match("www.domain.com");
            if (matchurls.Success) MessageBox.Show(matchurls.Value);
            else MessageBox.Show("No Match");

            Match matchmail = regexmail.Match("mike@collins.com");
            if (matchmail.Success) MessageBox.Show(matchmail.Value);
            else MessageBox.Show("No Match");
            #endregion

            Staff staff = new Staff();
            staff.SetStaffId((Form1.formgen.GetControl("staff.make", "staffId") as TextBox).Text);
            staff.SetUserName((Form1.formgen.GetControl("user.make", "username") as TextBox).Text);
            staff.SetFirstName((Form1.formgen.GetControl("user.make", "firstname") as TextBox).Text);
            staff.SetSurname((Form1.formgen.GetControl("user.make", "surname") as TextBox).Text);
            staff.SetEmail((Form1.formgen.GetControl("user.make", "email") as TextBox).Text);
            staff.SetPhoneNo((Form1.formgen.GetControl("user.make", "phone") as TextBox).Text);
            staff.SetAddress((Form1.formgen.GetControl("user.make", "address") as TextBox).Text);
            staff.SetDateOfBirth((Form1.formgen.GetControl("user.make", "dateofbirth") as TextBox).Text);
            staff.SetPassWord((Form1.formgen.GetControl("user.make", "password") as TextBox).Text);

            Form1.context.AddUser(staff.GetStaffId(), staff);
            Form1.context.SetSelected("staff", staff.GetStaffId());
            Form1.formgen.BuildPage("staff.list");
        }
    }
    #endregion


    #region droppage
    [Serializable]
    public class StaffDropPage : Page
    {
        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public StaffDropPage()
            : base("staff", "staff.drop") { }


        // ----------------------------------------------------------------- //
        // This method is invoked at startup. Here we define all of the form //
        // controls associated with user objects.                            //
        // ----------------------------------------------------------------- //
        public override void DefinePage()
        {
            // extra fields for drop page
            Dictionary<string, Widget> drop = new Dictionary<string, Widget>();
            drop.Add("header", new WidgetTitle("Delete User"));
            drop.Add("username", new WidgetTextBox(272, 152, 320, 32, "", true, false, false));
            drop.Add("delete", new WidgetButton(464, 532, 128, 32, "Delete", OnDropDeleteClick));
            drop.Add("cancel", new WidgetButton(272, 532, 128, 32, "Cancel", OnCancelClick));
            Form1.formgen.AddPage("staff.drop", drop);
        }


        // ----------------------------------------------------------------- //
        // this method deletes a user object after warning the user.         //
        // ----------------------------------------------------------------- //
        public void OnDropDeleteClick(object sender, EventArgs e)
        {
            string username = (Form1.formgen.GetControl("staff.drop", "username") as TextBox).Text;
            string firstname = (Form1.formgen.GetControl("staff.drop", "firstname") as TextBox).Text;
            string surname = (Form1.formgen.GetControl("staff.drop", "surname") as TextBox).Text;

            string message = string.Format("User Name = {0}\nFirst Name = {1}\nSurname = {2}\n"
                                          + "Are you sure you wish to\ndelete this staff member form the "
                                          + "database?", username, firstname, surname);
            DialogResult result = MessageBox.Show(message, "Warning", MessageBoxButtons.YesNoCancel);
            if (result == DialogResult.Yes)
            {
                Form1.context.users.Remove(username);
            }
            Form1.formgen.BuildPage("staff.list");
        }
    }
    #endregion


    #region listpage
    [Serializable]
    public class StaffListPage : ListPage
    {
        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public StaffListPage()
            : base("staff", "staff.list") { }


        // ----------------------------------------------------------------- //
        // This method is invoked at startup. Here we define all of the form //
        // controls associated with user objects.                            //
        // ----------------------------------------------------------------- //
        public override void DefinePage()
        {
            // list games page
            Dictionary<string, Widget> list = new Dictionary<string, Widget>();
            list.Add("header", new WidgetTitle("Staff"));
            list.Add("listview", new WidgetListView(32, 132, 800 - 64, 400 - 28, OnListRecordClick, ListViewMakeVisible));
            list.Add("label1", new WidgetLabel(32, 536, 80, 32, "Search"));
            list.Add("search", new WidgetTextBox(112, 534, 368, 32, "", false, false, false));
            list.Add("addnew", new WidgetButton(800 - 32 - 128, 532, 128, 32, "Add New", OnListAddNewClick));
            list.Add("delete", new WidgetButton(800 - 32 - 256 - 16, 532, 128, 32, "Delete", OnListDeleteClick));
            list.Add("edit", new WidgetButton(800 - 32 - 256 - 16, 532, 128, 32, "Edit", OnListEditClick));

            Form1.formgen.AddPage("staff.list", list);
            TextBox search = Form1.formgen.GetControl("staff.list", "search") as TextBox;
            if (search != null) search.TextChanged += OnUpdateSearch;
        }


        // ----------------------------------------------------------------- //
        // this method gets invoked automatically by the list view control   //
        // any time the list view needs to be updated.                       //
        // All this method does is to assign a name to each required column. //
        // ----------------------------------------------------------------- //
        public override void OnPopulateListColumns(ListView listview)
        {
            // the number of columns defined must match the number of values
            // passed in PopulateListItem
            listview.Columns.Clear();
            listview.Columns.Add("StaffId", 100);
            listview.Columns.Add("UserName", 100);
            listview.Columns.Add("FirstName", 100);
            listview.Columns.Add("Surname", 100);
            listview.Columns.Add("Email", 100);
            listview.Columns.Add("Phone", 100);
            listview.Columns.Add("Address", 100);
            listview.Columns.Add("DateOfBirth", 100);
        }


        // ----------------------------------------------------------------- //
        // this method enumerates the user list and populates listview rows. //
        // ----------------------------------------------------------------- //
        public override void OnPopulateListRecords(ListView listview) {
            listview.Items.Clear();
            foreach (KeyValuePair<string, User> user in Form1.context.users) {
                Staff staff = user.Value as Staff;
                if (staff != null) PopulateListItem(listview, staff);
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
                staff.GetStaffId(),staff.GetUserName(), staff.GetFirstName(),
                staff.GetSurname(), staff.GetEmail(), staff.GetPhoneNo(),
                staff.GetAddress(), staff.GetDateOfBirth()
            };
            listview.Items.Add(new ListViewItem(fields));
            return true;
        }
    }
    #endregion
}