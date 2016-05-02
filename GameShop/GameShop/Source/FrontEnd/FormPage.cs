// ========================================================================= //
// File Name : FormPage.cs                                                   //
// File Date : 12 April 2016                                                 //
// Author(s) : Michael Collins, Louise McKeown, Alan Rowlands                //
// File Info : The FormPage Manager super class, contains generic methods.   //
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
    #region page
    [Serializable]
    public abstract class Page {
        protected string typename;
        protected string pagename;


        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public Page(string TypeName, string PageName) {
            typename = TypeName;
            pagename = PageName;
        }


        // ----------------------------------------------------------------- //
        // pure virtuals                                                     //
        // ----------------------------------------------------------------- //
        public abstract void DefinePage();


        // ----------------------------------------------------------------- //
        // virtuals                                                          //
        // ----------------------------------------------------------------- //
        public virtual void OnLoadPage() {
            // override this method to apply any last minute modifications
            // to a page (setting the current date, or generating ids)
        }


        // ----------------------------------------------------------------- //
        // Clicking cancel on the create page will reroute the user back to  //
        // the main list page.                                               //
        // ----------------------------------------------------------------- //
        public void OnCancelClick(object sender, EventArgs e) {
            string pagename = typename + ".list";
            Form1.formgen.BuildPage(pagename);
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


    #region formpage
    [Serializable]
    public abstract class FormPage : Page {
        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public FormPage(string TypeName, string PageName)
        : base(TypeName, PageName) { }


        // ----------------------------------------------------------------- //
        // pure virtuals                                                     //
        // ----------------------------------------------------------------- //
        public abstract void OnPopulateForm(string page);
        public abstract bool Sanitize(string page);

    }
    #endregion


    #region listpage
    [Serializable]
    public abstract class ListPage : Page {
        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public ListPage(string TypeName, string PageName)
        : base(TypeName, PageName) { }


        // ----------------------------------------------------------------- //
        // pure virtuals                                                     //
        // ----------------------------------------------------------------- //
        public abstract void OnPopulateListColumns(ListView listview);
        public abstract void OnPopulateListRecords(ListView listview);


        // ----------------------------------------------------------------- //
        // This method populates the contents of the list on the list page.  //
        // ----------------------------------------------------------------- //
        public void ListViewMakeVisible(object sender, EventArgs e) {
            string pagename = typename + ".list";

            //Form1.context.GetSelected(parts[0]);
            Form1.form.SuspendLayout();
            ListView listview = Form1.formgen.GetControl(pagename, "listview") as ListView;
            OnPopulateListColumns(listview);
            OnPopulateListRecords(listview);
            Form1.form.ResumeLayout();
            listview.ItemSelectionChanged += OnListSelectionChanged;
        }


        // ----------------------------------------------------------------- //
        // Clicking a record on the list page will reroute the user to the   //
        // view page and populate the contents of the form.                  //
        // ----------------------------------------------------------------- //
        public void OnListSelectionChanged(object sender, EventArgs e) {
            ListView listview = sender as ListView;
            if (listview.SelectedItems.Count > 0) {
                string itemname = listview.SelectedItems[0].Text;
                Form1.context.SetSelected(typename, itemname);
            }
        }


        // ----------------------------------------------------------------- //
        // Clicking a record on the list page will reroute the user to the   //
        // view page and populate the contents of the form.                  //
        // ----------------------------------------------------------------- //
        public void OnListRecordClick(object sender, EventArgs e) {
            ListView listview = sender as ListView;
            string pagename = typename + ".view";
            //string itemname = listview.SelectedItems[0].Text;
            //Form1.context.SetSelected(typename, itemname);
            //Entity entity = Form1.context.GetSelected(typename);
            Form1.formgen.BuildPage(pagename);
            FormPage formpage = Form1.formgen.GetPage(typename + ".form") as FormPage;
            formpage.OnPopulateForm(pagename);
        }


        // ----------------------------------------------------------------- //
        // Clicking addnew on the list page will reroute the user to the     //
        // create page and empty the contents of the form.                   //
        // ----------------------------------------------------------------- //
        public void OnListAddNewClick(object sender, EventArgs e) {
            string pagename = typename + ".make";
            FormPage formpage = Form1.formgen.GetPage(typename + ".form") as FormPage;
            formpage.OnPopulateForm(pagename);
            Form1.formgen.BuildPage(pagename);
        }


        // ----------------------------------------------------------------- //
        // Clicking delete on the list page will reroute the user to the     //
        // delete page and populate the form with the fields from the entity //
        // this confirms to the user that they are about to delete the right //
        // item from the list.                                               //
        // ----------------------------------------------------------------- //
        public void OnListDeleteClick(object sender, EventArgs e) {
            string pagename = typename + ".drop";
            Form1.formgen.BuildPage(pagename);
            FormPage formpage = Form1.formgen.GetPage(typename + ".form") as FormPage;
            formpage.OnPopulateForm(pagename);
        }

        // ----------------------------------------------------------------- //
        // Clicking edit on the list page will reroute the user to the       //
        // edit page and populate the form with the fields from the entity   //
        // the user can then make changes to a field or fields and save      //
        // the changes                                                       //
        // ----------------------------------------------------------------- //
        public void OnListEditClick(object sender, EventArgs e) {
            string pagename = typename + ".edit";
            Form1.formgen.BuildPage(pagename);
            FormPage formpage = Form1.formgen.GetPage(typename + ".form") as FormPage;
            formpage.OnPopulateForm(pagename);
            Form1.formgen.BuildPage(pagename);
        }



        // ----------------------------------------------------------------- //
        // this method gets called when the user types into the search bar.  //
        // ----------------------------------------------------------------- //
        public void OnUpdateSearch(object sender, EventArgs e) {
            TextBox textbox = sender as TextBox;

            if (textbox.Text == "") EmptySearch();
            else RegexSearch(textbox.Text);
        }


        // ----------------------------------------------------------------- //
        // fill the ListView with everything you possibly can.               //
        // ----------------------------------------------------------------- //
        public void EmptySearch() {
            ListView listview;
            switch (typename) {
            case "user": case "member":
                listview = Form1.formgen.GetControl("user.list", "listview") as ListView;
                UserListPage userpage = Form1.formgen.GetPage(typename+".list") as UserListPage;
                if (userpage == null) return;
                listview.Items.Clear();
                foreach (KeyValuePair<string, Member> member in Form1.context.members) {
                    userpage.PopulateListItem(listview, member.Value);
                }
                break;
            case "staff": case "manager":
                listview = Form1.formgen.GetControl("staff.list", "listview") as ListView;
                StaffListPage staffpage = Form1.formgen.GetPage(typename+".list") as StaffListPage;
                if (staffpage == null) return;
                listview.Items.Clear();
                foreach (KeyValuePair<string, Staff> staff in Form1.context.staffs) {
                    staffpage.PopulateListItem(listview, staff.Value);
                }
                break;
            case "game":
                listview = Form1.formgen.GetControl("game.list", "listview") as ListView;
                GameListPage gamepage = Form1.formgen.GetPage("game.list") as GameListPage;
                if (gamepage == null) return;
                listview.Items.Clear();
                foreach (KeyValuePair<string, Game> game in Form1.context.games) {
                    gamepage.PopulateListItem(listview, game.Value);
                }
                break;
            case "report":
                listview = Form1.formgen.GetControl("report.list", "listview") as ListView;
                ReportListPage reportpage = Form1.formgen.GetPage("report.list") as ReportListPage;
                if (reportpage == null) return;
                listview.Items.Clear();
                foreach (KeyValuePair<string, Report> report in Form1.context.reports) {
                    reportpage.PopulateListItem(listview, report.Value);
                }
                break;
            }
        }


        // ----------------------------------------------------------------- //
        // filter the ListView powered by regular expression.                //
        // ----------------------------------------------------------------- //
        public void RegexSearch(string filter) {
            ListView listview;
            Regex regex = null;
            try { regex = new Regex(@"" + filter, RegexOptions.IgnoreCase); }
            catch { return; }

            switch (typename) {
            case "user": case "member":
                listview = Form1.formgen.GetControl("user.list", "listview") as ListView;
                if (listview == null) return;
                UserListPage userpage = Form1.formgen.GetPage("user.list") as UserListPage;
                if (userpage == null) return;
                listview.Items.Clear();
                foreach (KeyValuePair<string, Member> member in Form1.context.members) {
                    if (!member.Value.RegexMatch(regex)) continue;
                    userpage.PopulateListItem(listview, member.Value);
                }
                break;
            case "staff": case "manager":
                listview = Form1.formgen.GetControl("staff.list", "listview") as ListView;
                StaffListPage staffpage = Form1.formgen.GetPage("staff.list") as StaffListPage;
                if (staffpage == null) return;
                listview.Items.Clear();
                foreach (KeyValuePair<string, Staff> staff in Form1.context.staffs) {
                    if (!staff.Value.RegexMatch(regex)) continue;
                    staffpage.PopulateListItem(listview, staff.Value);
                }
                break;
            case "game":
                listview = Form1.formgen.GetControl("game.list", "listview") as ListView;
                GameListPage gamepage = Form1.formgen.GetPage("game.list") as GameListPage;
                if (gamepage == null) return;
                listview.Items.Clear();
                foreach (KeyValuePair<string, Game> game in Form1.context.games) {
                    if (!game.Value.RegexMatch(regex)) continue;
                    gamepage.PopulateListItem(listview, game.Value);
                }
                break;
            case "report":
                listview = Form1.formgen.GetControl("report.list", "listview") as ListView;
                ReportListPage reportpage = Form1.formgen.GetPage("report.list") as ReportListPage;
                if (reportpage == null) return;
                listview.Items.Clear();
                foreach (KeyValuePair<string, Report> report in Form1.context.reports) {
                    if (!report.Value.RegexMatch(regex)) continue;
                    reportpage.PopulateListItem(listview, report.Value);
                }
                break;
            }
        }
    }
    #endregion
}