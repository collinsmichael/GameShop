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
    public class TransactionReportForm : FormPage
    {

        public TransactionReportForm()
            : base("transaction", "transaction.form") { }

        public override void DefinePage()
        {
            // basic form fields
            Dictionary<string, Widget> form = new Dictionary<string, Widget>();

            form.Add("label1", new WidgetLabel(150, 196, 122, 32, "Transaction Id"));
            form.Add("label2", new WidgetLabel(150, 196, 122, 32, "User Name"));
            form.Add("label3", new WidgetLabel(150, 236, 122, 32, "Membership Fees"));
            form.Add("label4", new WidgetLabel(150, 276, 122, 32, "Rental Fees"));
            form.Add("label5", new WidgetLabel(150, 316, 122, 32, "Late Return Fees"));
            form.Add("transactionId", new WidgetTextBox(272, 192, 320, 32, "", true, false, false));
            form.Add("username", new WidgetTextBox(272, 192, 320, 32, "", true, false, false));
            form.Add("membershipFees", new WidgetTextBox(272, 232, 320, 32, "", true, false, false));
            form.Add("rentalFees", new WidgetTextBox(272, 272, 320, 96, "", true, false, false));
            form.Add("lateReturnFees", new WidgetTextBox(272, 312, 320, 96, "", true, false, false));
            Form1.formgen.AddPage("transaction.form", form);

        }

        // ----------------------------------------------------------------- //
        // this method populates the fields within a form page.              //
        // ----------------------------------------------------------------- //
        public override void OnPopulateForm(string page)
        {
            Form1.formgen.BuildPage(page);
            Transaction transaction = new Transaction();
            if (page != "transaction.make")
            {
                transaction = Form1.context.GetTransaction(Form1.context.GetSelectedTransaction());
            }

            (Form1.formgen.GetControl(page, "transactionId") as TextBox).Text = transaction.GetTransactionId().ToString();
            (Form1.formgen.GetControl(page, "username") as TextBox).Text = transaction.GetUsername();
            (Form1.formgen.GetControl(page, "membershipFees") as TextBox).Text = transaction.GetMembershipFee().ToString();
            (Form1.formgen.GetControl(page, "rentalFees") as TextBox).Text = transaction.GetRentalFee().ToString();
            (Form1.formgen.GetControl(page, "lateReturnFees") as TextBox).Text = transaction.GetLateReturnFee().ToString();



            Button return_button = Form1.formgen.GetControl("transaction.view", "return") as Button;
            Button cancel_button = Form1.formgen.GetControl("transaction.view", "cancel") as Button;
            if (transaction.GetTransactionId() == "")
            {
                return_button.Show();
                cancel_button.Hide();
            }
            else
            {
                cancel_button.Show();
                return_button.Hide();
            }
        }
    }
    #endregion


    #region viewpage
    [Serializable]

    public class TransactionViewPage : Page
    {
        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public TransactionViewPage()
            : base("transaction", "transaction.view") { }





        public override void DefinePage()
        {
            // extra fields for view page
            Dictionary<string, Widget> view = new Dictionary<string, Widget>();
            view.Add("header", new WidgetTitle("Account Transactions"));
            view.Add("title", new WidgetTextBox(272, 152, 320, 32, "", false, false, false));
            view.Add("fees paid", new WidgetButton(464, 428, 128, 32, "fees paid", OnMakePaidClick));
            view.Add("cancel", new WidgetButton(272, 428, 128, 32, "Cancel", OnCancelClick));
            Form1.formgen.AddPage("transaction.make", view);
        }

        public void OnMakePaidClick(object sender, EventArgs e)
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
            #endregion

            Transaction transaction = new Transaction();
            int membershipfees = 0;
            int.TryParse((Form1.formgen.GetControl("transaction.make", "membership fee") as TextBox).Text, out membershipfees);
            int rentalFees = 0;
            int.TryParse((Form1.formgen.GetControl("transaction.make", "rental fee") as TextBox).Text, out rentalFees);
            int lateReturnFee = 0;
            int.TryParse((Form1.formgen.GetControl("transaction.make", "late return fee") as TextBox).Text, out lateReturnFee);
            transaction.SetMembershipFee(membershipfees);
            transaction.SetRentalFee(rentalFees);
            transaction.SetLateReturnFee(lateReturnFee);
            Form1.context.AddTransaction(transaction.GetTransactionId(), transaction);
            Form1.context.SetSelected("transaction", transaction.GetTransactionId());
            Form1.formgen.BuildPage("transaction.list");
        }
    }
    #endregion

    #region editpage
    [Serializable]
    public class TransactionEditPage : Page
    {
        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public TransactionEditPage()
            : base("transaction", "transaction.edit") { }


        // ----------------------------------------------------------------- //
        // This method is invoked at startup. Here we define all of the form //
        // controls associated with game objects.                            //
        // ----------------------------------------------------------------- //
        public override void DefinePage()
        {
            // extra fields for edit page
            Dictionary<string, Widget> edit = new Dictionary<string, Widget>();

            edit.Add("header", new WidgetTitle("Account Transactions"));
            edit.Add("title", new WidgetTextBox(272, 152, 320, 32, "", false, false, false));
            edit.Add("fees paid", new WidgetButton(464, 428, 128, 32, "fees paid", OnEditSubmitClick));
            edit.Add("cancel", new WidgetButton(272, 428, 128, 32, "Cancel", OnCancelClick));
            Form1.formgen.AddPage("transaction.edit", edit);
        }


        // ----------------------------------------------------------------- //
        // this method populates a game object from the fields within a form //
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


        }






   
    } 
    #endregion
}
