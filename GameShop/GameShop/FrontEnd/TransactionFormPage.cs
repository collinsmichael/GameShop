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
    public class TransactionFormPage : FormPage {
        public TransactionFormPage()
        : base("transaction", "transaction.form") { }

        public override void DefinePage() {
            // basic form fields
            Dictionary<string, Widget> form = new Dictionary<string, Widget>();

            form.Add("label1", new WidgetLabel(150, 156, 122, 32, "Transact Id"));
            form.Add("label2", new WidgetLabel(150, 196, 122, 32, "User Name"));
            form.Add("label3", new WidgetLabel(150, 236, 122, 32, "Member Fees"));
            form.Add("label4", new WidgetLabel(150, 276, 122, 32, "Rental Fees"));
            form.Add("label5", new WidgetLabel(150, 316, 122, 32, "Late Fees"));
          //form.Add("transactionId",  new WidgetTextBox(272, 192, 320, 32, "", true, false, false));
            form.Add("username",       new WidgetTextBox(272, 192, 320, 32, "", true, false, false));
            form.Add("membershipFees", new WidgetTextBox(272, 232, 320, 32, "", true, false, false));
            form.Add("rentalFees",     new WidgetTextBox(272, 272, 320, 96, "", true, false, false));
            form.Add("lateReturnFees", new WidgetTextBox(272, 312, 320, 96, "", true, false, false));
            Form1.formgen.AddPage("transaction.form", form);

        }

        // ----------------------------------------------------------------- //
        // this method populates the fields within a form page.              //
        // ----------------------------------------------------------------- //
        public override void OnPopulateForm(string page) {
            Form1.formgen.BuildPage(page);
            Transaction transaction = new Transaction();
            if (page != "transaction.make") {
                transaction = Form1.context.GetTransaction(Form1.context.GetSelectedTransaction());
            }

            (Form1.formgen.GetControl(page, "transactionId") as TextBox).Text = transaction.GetTransactionId().ToString();
            (Form1.formgen.GetControl(page, "username") as TextBox).Text = transaction.GetUsername();
            (Form1.formgen.GetControl(page, "membershipFees") as TextBox).Text = transaction.GetMembershipFee().ToString();
            (Form1.formgen.GetControl(page, "rentalFees") as TextBox).Text = transaction.GetRentalFee().ToString();
            (Form1.formgen.GetControl(page, "lateReturnFees") as TextBox).Text = transaction.GetLateReturnFee().ToString();

            //Button return_button = Form1.formgen.GetControl("transaction.view", "return") as Button;
            //Button cancel_button = Form1.formgen.GetControl("transaction.view", "cancel") as Button;
            //if (transaction.GetTransactionId() == "") {
            //    return_button.Show();
            //    cancel_button.Hide();
            //} else {
            //    cancel_button.Show();
            //    return_button.Hide();
            //}
        }
    }
    #endregion


    #region viewpage
    [Serializable]
    public class TransactionViewPage : Page {
        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public TransactionViewPage()
        : base("transaction", "transaction.view") { }


        public override void DefinePage() {
            // extra fields for view page
            Dictionary<string, Widget> view = new Dictionary<string, Widget>();
            view.Add("header", new WidgetTitle("Account Transactions"));
            view.Add("transactionId",  new WidgetTextBox(272, 152, 320, 32, "", true, false, false));
            view.Add("edit", new WidgetButton(464, 428, 128, 32, "Edit", OnViewEditClick));
            view.Add("cancel", new WidgetButton(272, 428, 128, 32, "Cancel", OnCancelClick));
            Form1.formgen.AddPage("transaction.view", view);
        }
    }
    #endregion


    #region editpage
    [Serializable]
    public class TransactionEditPage : Page {
        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public TransactionEditPage()
        : base("transaction", "transaction.edit") { }


        // ----------------------------------------------------------------- //
        // This method is invoked at startup. Here we define all of the form //
        // controls associated with game objects.                            //
        // ----------------------------------------------------------------- //
        public override void DefinePage() {
            // extra fields for edit page
            Dictionary<string, Widget> edit = new Dictionary<string, Widget>();

            edit.Add("header", new WidgetTitle("Edit Transaction"));
            edit.Add("transactionId",  new WidgetTextBox(272, 152, 320, 32, "", true, false, false));
            edit.Add("submit", new WidgetButton(464, 428, 128, 32, "Submit", OnEditSubmitClick));
            edit.Add("cancel", new WidgetButton(272, 428, 128, 32, "Cancel", OnCancelClick));
            Form1.formgen.AddPage("transaction.edit", edit);
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
        }
    }
    #endregion


    #region makepage
    [Serializable]
    public class TransactionMakePage : Page {
        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public TransactionMakePage()
            : base("transaction", "transaction.make") { }


        // ----------------------------------------------------------------- //
        // This method is invoked at startup. Here we define all of the form //
        // controls associated with game objects.                            //
        // ----------------------------------------------------------------- //
        public override void DefinePage() {
            // extra fields for edit page
            Dictionary<string, Widget> make = new Dictionary<string, Widget>();
            make.Add("header", new WidgetTitle("Account Transactions"));
            make.Add("transactionId",  new WidgetTextBox(272, 152, 320, 32, "", false, false, false));
            make.Add("fees paid", new WidgetButton(464, 428, 128, 32, "Fees Paid", OnMakeSubmitClick));
            make.Add("cancel", new WidgetButton(272, 428, 128, 32, "Cancel", OnCancelClick));
            Form1.formgen.AddPage("transaction.make", make);
        }


        // ----------------------------------------------------------------- //
        // This method is invoked on display, now we perform any last second //
        // adjustments to the controls associated with this page.            //
        // ----------------------------------------------------------------- //
        public override void OnLoadPage() {
            DateTime now = DateTime.Now;
            string transactionId = "z" + Form1.context.orders.Count().ToString("000");
            string username = Form1.context.GetSelectedUser();

            string pagename = typename + ".make";
            (Form1.formgen.GetControl(pagename, "transactionId") as TextBox).Text = transactionId;
            (Form1.formgen.GetControl(pagename, "username") as TextBox).Text = username;
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

            Transaction transaction = new Transaction();
            int membershipfees = 0;
            int.TryParse((Form1.formgen.GetControl("transaction.make", "membershipFees") as TextBox).Text, out membershipfees);
            int rentalFees = 0;
            int.TryParse((Form1.formgen.GetControl("transaction.make", "rentalFees") as TextBox).Text, out rentalFees);
            int lateReturnFee = 0;
            int.TryParse((Form1.formgen.GetControl("transaction.make", "lateReturnFees") as TextBox).Text, out lateReturnFee);

            transaction.SetTransactionId((Form1.formgen.GetControl("transaction.make", "transactionId") as TextBox).Text);
            transaction.SetUsername((Form1.formgen.GetControl("transaction.make", "username") as TextBox).Text);
            transaction.SetMembershipFee(membershipfees);
            transaction.SetRentalFee(rentalFees);
            transaction.SetLateReturnFee(lateReturnFee);

            Form1.context.AddTransaction(transaction.GetTransactionId(), transaction);
            Form1.context.SetSelected("transaction", transaction.GetTransactionId());
            Form1.formgen.BuildPage("transaction.list");
        }
    }
    #endregion


    #region listpage
    [Serializable]
    public class TransactionListPage : ListPage {
        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public TransactionListPage()
            : base("transaction", "transaction.list") { }


        // ----------------------------------------------------------------- //
        // This method is invoked at startup. Here we define all of the form //
        // controls associated with order objects.                           //
        // ----------------------------------------------------------------- //
        public override void DefinePage()
        {
            // list transactions page
            Dictionary<string, Widget> list = new Dictionary<string, Widget>();
            list = new Dictionary<string, Widget>();
            list.Add("header", new WidgetTitle("Transactions"));
            list.Add("listview", new WidgetListView(32, 132, 800 - 64, 400 - 28, OnListRecordClick, ListViewMakeVisible));
            list.Add("label1", new WidgetLabel(32, 536, 80, 32, "Search"));
            list.Add("search", new WidgetTextBox(112, 534, 368, 32, "", false, false, false));
            list.Add("addnew", new WidgetButton(800 - 32 - 128, 532, 128, 32, "Add New", OnListAddNewClick));
            list.Add("edit", new WidgetButton(800 - 32 - 128, 532, 128, 32, "Edit", OnListEditClick));
            //list.Add("delete",    new WidgetButton(800-32-256-16, 532, 128, 32, "Delete",  OnListDeleteClick));

            Form1.formgen.AddPage("transaction.list", list);
            TextBox search = Form1.formgen.GetControl("transaction.list", "search") as TextBox;
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
            listview.Columns.Add("Transaction ID", 100);
            listview.Columns.Add("User Name", 100);
            listview.Columns.Add("Membership Fee", 100);
            listview.Columns.Add("Rental Fee", 100);
            listview.Columns.Add("Late Return Fee", 100);
        }


        // ----------------------------------------------------------------- //
        // this method enumerates the order list and populates listview rows //
        // ----------------------------------------------------------------- //
        public override void OnPopulateListRecords(ListView listview) {
            listview.Items.Clear();
            foreach (KeyValuePair<string, Transaction> transaction in Form1.context.transactions) {
                PopulateListItem(listview, transaction.Value);
            }
        }


        // ----------------------------------------------------------------- //
        // this method gets invoked automatically by the list view control   //
        // any time the list view needs to be updated.                       //
        // All this method does is to assign a values to each field in the   //
        // list view record.                                                 //
        // ----------------------------------------------------------------- //
        public bool PopulateListItem(ListView listview, Transaction transaction) {
            // the number of values passed must match the number of columns
            // defined in PopulateListColumns
            string[] fields = new string[] {
                transaction.GetTransactionId(), transaction.GetUsername(),
                transaction.GetMembershipFee().ToString(),
                transaction.GetRentalFee().ToString(),
                transaction.GetLateReturnFee().ToString()
            };
            listview.Items.Add(new ListViewItem(fields));
            return true;
        }

    }

    #endregion
}
