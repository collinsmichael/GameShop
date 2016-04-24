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
        }


        // ----------------------------------------------------------------- //
        // Clicking a record on the list page will reroute the user to the   //
        // view page and populate the contents of the form.                  //
        // ----------------------------------------------------------------- //
        public void OnListRecordClick(object sender, EventArgs e) {
            ListView listview = sender as ListView;
            string pagename = typename + ".view";
            string itemname = listview.SelectedItems[0].Text;

            Form1.context.SetSelected(typename, itemname);
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
            ListView listview = Form1.formgen.GetControl(pagename, "listview") as ListView;
            UserListPage userpage = Form1.formgen.GetPage("user.list") as UserListPage;
            GameListPage gamepage = Form1.formgen.GetPage("game.list") as GameListPage;
            if (listview == null || userpage == null || gamepage == null) return;
            listview.Items.Clear();

            if (typename == "user") {
                foreach (KeyValuePair<string, User> user in Form1.context.users) {
                    userpage.PopulateListItem(listview, user.Value);
                }
            } else if (typename == "game") {
                foreach (KeyValuePair<string, Game> game in Form1.context.games) {
                    gamepage.PopulateListItem(listview, game.Value);
                }
            }
        }


        // ----------------------------------------------------------------- //
        // filter the ListView powered by regular expression.                //
        // ----------------------------------------------------------------- //
        public void RegexSearch(string filter) {
            ListView listview = Form1.formgen.GetControl(pagename, "listview") as ListView;
            UserListPage userpage = Form1.formgen.GetPage("user.list") as UserListPage;
            GameListPage gamepage = Form1.formgen.GetPage("game.list") as GameListPage;
            if (listview == null || userpage == null || gamepage == null) return;

            Regex regex = null;
            try { regex = new Regex(@"" + filter, RegexOptions.IgnoreCase); }
            catch { return; }

            listview.Items.Clear();
            if (typename == "user") {
                foreach (KeyValuePair<string, User> user in Form1.context.users) {
                    if (!user.Value.RegexMatch(regex)) continue;
                    userpage.PopulateListItem(listview, user.Value);
                }
            } else if (typename == "game") {
                foreach (KeyValuePair<string, Game> game in Form1.context.games) {
                    if (!game.Value.RegexMatch(regex)) continue;
                    gamepage.PopulateListItem(listview, game.Value);
                }
            }
        }
    }
    #endregion
}