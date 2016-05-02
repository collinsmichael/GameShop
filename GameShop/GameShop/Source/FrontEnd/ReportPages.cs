// ========================================================================= //
// File Name : ReportPages.cs                                           //
// File Date : 12 April 2016                                                 //
// Author(s) : Michael Collins, Louise McKeown, Alan Rowlands                //
// File Info : The ReportFormPage defines report user interface.   //
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
    public class ReportFormPage : FormPage {
        public ReportFormPage()
        : base("report", "report.form") { }

        public override void DefinePage() {
            // basic form fields
            Dictionary<string, Widget> form = new Dictionary<string, Widget>();

            form.Add("label1", new WidgetLabel(140, 156, 132, 32, "Report Id"));
            form.Add("label2", new WidgetLabel(140, 196, 132, 32, "User Name"));
            form.Add("label3", new WidgetLabel(140, 236, 132, 32, "Report Date"));
            form.Add("label4", new WidgetLabel(140, 276, 132, 32, "Member Fees"));
            form.Add("label5", new WidgetLabel(140, 316, 132, 32, "Rental Fees"));
            form.Add("label6", new WidgetLabel(140, 356, 132, 32, "Late Fees"));
            form.Add("username",   new WidgetTextBox(272, 192, 320, 32, "", true, false, false, 1));
            form.Add("reportdate", new WidgetTextBox(272, 232, 320, 32, "", true, false, false, 2));
            form.Add("memberfees", new WidgetTextBox(272, 272, 320, 32, "", true, false, false, 2));
            form.Add("rentalfees", new WidgetTextBox(272, 312, 320, 96, "", true, false, false, 3));
            form.Add("latefees",   new WidgetTextBox(272, 352, 320, 96, "", true, false, false, 4));
            Form1.formgen.AddPage("report.form", form);

        }

        // ----------------------------------------------------------------- //
        // this method populates the fields within a form page.              //
        // ----------------------------------------------------------------- //
        public override void OnPopulateForm(string page) {
            Form1.formgen.BuildPage(page);
            Report report = new Report();
            if (page != "report.make") {
                report = Form1.context.GetReport(Form1.context.GetSelectedReport());
            }

            (Form1.formgen.GetControl(page, "reportid") as TextBox).Text = report.GetReportId();
            (Form1.formgen.GetControl(page, "username") as TextBox).Text = report.GetUsername();
            (Form1.formgen.GetControl(page, "memberfees") as TextBox).Text = report.GetMemberFees().ToString();
            (Form1.formgen.GetControl(page, "rentalfees") as TextBox).Text = report.GetRentalFees().ToString();
            (Form1.formgen.GetControl(page, "latefees") as TextBox).Text = report.GetLateFees().ToString();
        }


        // ----------------------------------------------------------------- //
        // this method sanitizes values obtained from the form, and will not //
        // apply changes unless all values pass the sanity test.             //
        // ----------------------------------------------------------------- //
        public override bool Sanitize(string page) {
            // get values from form
            string reportid   = (Form1.formgen.GetControl(page, "reportid")   as TextBox).Text;
            string username   = (Form1.formgen.GetControl(page, "username")   as TextBox).Text;
            string reportdate = (Form1.formgen.GetControl(page, "reportdate") as TextBox).Text;
            string memberfees = (Form1.formgen.GetControl(page, "memberfees") as TextBox).Text;
            string rentalfees = (Form1.formgen.GetControl(page, "rentalfees") as TextBox).Text;
            string latefees   = (Form1.formgen.GetControl(page, "latefees")   as TextBox).Text;

            #region sanitize
            Regex regexdate = new Regex(@"^[0-9]{1,2}[-/.]{1}[0-9]{1,2}[-/.]{1}[0-9]{2,4}");
            Regex regenumber = (new Regex(@"^[0-9]+"));
            
            // is the date of birth a valid date?
            if ((!regexdate.Match(reportdate).Success)) {
                MessageBox.Show("Report Date is not valid!");
                return false;
            }

            // will only accept positive whole numbers
            if (!regenumber.Match(memberfees).Success) {
                MessageBox.Show("Please enter a positive whole number in the Member Fees field!");
                return false;
            }
            if (!regenumber.Match(rentalfees).Success) {
                MessageBox.Show("Please enter a positive whole number in the Rental Fees field!");
                return false;
            }
            if (!regenumber.Match(latefees).Success) {
                MessageBox.Show("Please enter a positive whole number in the Late Fees field!");
                return false;
            }
            #endregion

            // add order to the database and to the users account
            Report report = new Report(reportid, username, reportdate, int.Parse(rentalfees), int.Parse(latefees), int.Parse(memberfees));
            Form1.context.AddReport(reportid, report);
            Form1.context.SetSelected("report", reportid);
            return true;
        }
    }
    #endregion


    #region viewpage
    [Serializable]
    public class ReportViewPage : Page {
        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public ReportViewPage()
        : base("report", "report.view") { }


        public override void DefinePage() {
            // extra fields for view page
            Dictionary<string, Widget> view = new Dictionary<string, Widget>();
            view.Add("header", new WidgetTitle("Account Reports"));
            view.Add("reportid",  new WidgetTextBox(272, 152, 320, 32, "", true, false, false, 0));
            view.Add("edit", new WidgetButton(464, 428, 128, 32, "Edit", OnViewEditClick, 5));
            view.Add("cancel", new WidgetButton(272, 428, 128, 32, "Cancel", OnCancelClick, 6));
            Form1.formgen.AddPage("report.view", view);
        }


        public void OnMakePaidClick(object sender, EventArgs e) {
            // TODO : Sanitize field data (use regular expression parsing)
            // TODO : If you're not using any of these please comment out
            //        rather than deleting these took a lot of time to build
            #region sanitize
            Regex regexname = new Regex(@"^[A-Za-z]+", RegexOptions.IgnoreCase);
            Regex regexdate = new Regex(@"^[0-9]{1,2}[-/.]{1}[0-9]{1,2}[-/.]{1}[0-9]{2,4}");
            Regex regexvalu = new Regex(@"^[0-9]+", RegexOptions.IgnoreCase);
            Regex regexurls = new Regex(@"^www[.][a-z]{1,15}[.](com|org)$");
            Regex regexmail = new Regex(@"^[a-zA-Z0-9]{1,10}@[a-zA-Z]{1,10}.(com|org)$");

            Match matchname = regexname.Match("louise");
            if (matchname.Success) MessageBox.Show(matchname.Value);
            else MessageBox.Show("No Match");

            Match matchvalu = regexvalu.Match("78912");
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

            Match matchmail = regexmail.Match("louise@mckeown.com");
            if (matchmail.Success) MessageBox.Show(matchmail.Value);
            else MessageBox.Show("No Match");


            //Report report = new Report();
            //int membershipfees = 0;
            //int.TryParse((Form1.formgen.GetControl("report.make", "membership fee") as TextBox).Text, out membershipfees);
            //int rentalfees = 0;
            //int.TryParse((Form1.formgen.GetControl("report.make", "rental fee") as TextBox).Text, out rentalfees);
            //int lateReturnFee = 0;
            //int.TryParse((Form1.formgen.GetControl("report.make", "late return fee") as TextBox).Text, out lateReturnFee);
            //report.SetMembershipFee(membershipfees);
            //report.SetRentalFee(rentalfees);
            //report.SetLateReturnFee(lateReturnFee);
            //Form1.context.AddReport(report.GetReportId(), report);
            //Form1.context.SetSelected("report", report.GetReportId());
            //Form1.formgen.BuildPage("report.list");
        
    
            #endregion
        }
    }
    #endregion


    #region editpage
    [Serializable]
    public class ReportEditPage : Page {
        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public ReportEditPage()
        : base("report", "report.edit") { }


        // ----------------------------------------------------------------- //
        // This method is invoked at startup. Here we define all of the form //
        // controls associated with game objects.                            //
        // ----------------------------------------------------------------- //
        public override void DefinePage() {
            // extra fields for edit page
            Dictionary<string, Widget> edit = new Dictionary<string, Widget>();

            edit.Add("header", new WidgetTitle("Edit Report"));
            edit.Add("reportid",  new WidgetTextBox(272, 152, 320, 32, "", true, false, false, 0));
            edit.Add("submit", new WidgetButton(464, 428, 128, 32, "Submit", OnEditSubmitClick, 5));
            edit.Add("cancel", new WidgetButton(272, 428, 128, 32, "Cancel", OnCancelClick, 6));
            Form1.formgen.AddPage("report.edit", edit);
        }


        // ----------------------------------------------------------------- //
        // this method populates a game object from the fields within a form //
        // ----------------------------------------------------------------- //
        public void OnEditSubmitClick(object sender, EventArgs e) {
            FormPage form = Form1.formgen.GetPage("report.form") as FormPage;
            if (form != null && form.Sanitize("report.edit") == true) {
                Form1.formgen.BuildPage("report.list");
            }
        }
    }
    #endregion


    #region makepage
    [Serializable]
    public class ReportMakePage : Page {
        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public ReportMakePage()
        : base("report", "report.make") { }


        // ----------------------------------------------------------------- //
        // This method is invoked at startup. Here we define all of the form //
        // controls associated with game objects.                            //
        // ----------------------------------------------------------------- //
        public override void DefinePage() {
            // extra fields for edit page
            Dictionary<string, Widget> make = new Dictionary<string, Widget>();
            make.Add("header", new WidgetTitle("Account Reports"));
            make.Add("reportid",  new WidgetTextBox(272, 152, 320, 32, "", false, false, false, 0));
            make.Add("fees paid", new WidgetButton(464, 428, 128, 32, "Fees Paid", OnMakeSubmitClick, 5));
            make.Add("cancel", new WidgetButton(272, 428, 128, 32, "Cancel", OnCancelClick, 6));
            Form1.formgen.AddPage("report.make", make);
        }


        // ----------------------------------------------------------------- //
        // This method is invoked on display, now we perform any last second //
        // adjustments to the controls associated with this page.            //
        // ----------------------------------------------------------------- //
        public override void OnLoadPage() {
            string reportid = "z" + Form1.context.reports.Count().ToString("000");
            string username = Form1.context.GetSelectedMember();

            string pagename = typename + ".make";
            (Form1.formgen.GetControl(pagename, "reportid") as TextBox).Text = reportid;
            (Form1.formgen.GetControl(pagename, "username") as TextBox).Text = username;
            (Form1.formgen.GetControl(pagename, "reportdate") as TextBox).Text = DateTime.Now.ToString("d/M/yyy");
        }


        // ----------------------------------------------------------------- //
        // this method creates a game object from the fields within a form.  //
        // ----------------------------------------------------------------- //
        public void OnMakeSubmitClick(object sender, EventArgs e) {
            FormPage form = Form1.formgen.GetPage("report.form") as FormPage;
            if (form != null && form.Sanitize("report.make") == true) {
                Form1.formgen.BuildPage("report.list");
            }
        }
    }
    #endregion


    #region listpage
    [Serializable]
    public class ReportListPage : ListPage {
        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public ReportListPage()
        : base("report", "report.list") { }


        // ----------------------------------------------------------------- //
        // This method is invoked at startup. Here we define all of the form //
        // controls associated with order objects.                           //
        // ----------------------------------------------------------------- //
        public override void DefinePage() {
            // list reports page
            Dictionary<string, Widget> list = new Dictionary<string, Widget>();
            list = new Dictionary<string, Widget>();
            list.Add("header", new WidgetTitle("Reports"));
            list.Add("listview", new WidgetListView(32, 132, 800 - 64, 400 - 28, OnListRecordClick, ListViewMakeVisible));
            list.Add("label1", new WidgetLabel(32, 536, 80, 32, "Search"));
            list.Add("search", new WidgetTextBox(112, 534, 368, 32, "", false, false, false, 0));
            list.Add("addnew", new WidgetButton(800 - 32 - 128, 532, 128, 32, "Add New", OnListAddNewClick, 1));
            list.Add("edit", new WidgetButton(800 - 32 - 128, 532, 128, 32, "Edit", OnListEditClick, 2));
            //list.Add("delete",    new WidgetButton(800-32-256-16, 532, 128, 32, "Delete",  OnListDeleteClick));

            Form1.formgen.AddPage("report.list", list);
            TextBox search = Form1.formgen.GetControl("report.list", "search") as TextBox;
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
            listview.Columns.Add("Report ID", 150);
            listview.Columns.Add("User Name", 120);
            listview.Columns.Add("Member Fees", 120);
            listview.Columns.Add("Rental Fees", 120);
            listview.Columns.Add("Late Fees",  150);
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
            listview.ContextMenu = cm;
        }
        private void OnClickView(object sender, EventArgs e) {
            Form1.formgen.ClearPage("report.list");
            FormPage formpage = Form1.formgen.GetPage("report.form") as FormPage;
            formpage.OnPopulateForm("report.view");
            Form1.formgen.BuildPage("report.view");
        }
        private void OnClickEdit(object sender, EventArgs e) {
            Form1.formgen.ClearPage("report.list");
            FormPage formpage = Form1.formgen.GetPage("report.form") as FormPage;
            formpage.OnPopulateForm("report.edit");
            Form1.formgen.BuildPage("report.edit");
        }
        private void OnClickMake(object sender, EventArgs e) {
            Form1.formgen.ClearPage("report.list");
            FormPage formpage = Form1.formgen.GetPage("report.form") as FormPage;
            formpage.OnPopulateForm("report.make");
            Form1.formgen.BuildPage("report.make");
        }


        // ----------------------------------------------------------------- //
        // this method enumerates the order list and populates listview rows //
        // ----------------------------------------------------------------- //
        public override void OnPopulateListRecords(ListView listview) {
            listview.Items.Clear();
            foreach (KeyValuePair<string, Report> report in Form1.context.reports) {
                PopulateListItem(listview, report.Value);
            }
        }


        // ----------------------------------------------------------------- //
        // this method gets invoked automatically by the list view control   //
        // any time the list view needs to be updated.                       //
        // All this method does is to assign a values to each field in the   //
        // list view record.                                                 //
        // ----------------------------------------------------------------- //
        public bool PopulateListItem(ListView listview, Report report) {
            // the number of values passed must match the number of columns
            // defined in PopulateListColumns
            string[] fields = new string[] {
                report.GetReportId(),
                report.GetUsername(),
                report.GetRentalFees().ToString(),
                report.GetMemberFees().ToString(),
                report.GetLateFees().ToString()
            };
            listview.Items.Add(new ListViewItem(fields));
            return true;
        }

    }

    #endregion
}