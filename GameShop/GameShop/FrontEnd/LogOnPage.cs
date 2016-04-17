// ========================================================================= //
// File Name : LogOn.cs                                                      //
// File Date : 12 April 2016                                                 //
// Author(s) : Michael Collins, Louise McKeown, Alan Redding                 //
// File Info : The LogOn class is responsible for user account verification. //
//             Users of the system must supply valid log in credentials to   //
//             gain access to the system. The LogOn class also releases a    //
//             log in session during log out.                                //
// ========================================================================= //

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace GameShop {
    #region logon
    // --------------------------------------------------------------------- //
    // LogOn is responsible for logging users into the system. If the log on //
    // attempt is successful, then the user is presented with appropriate    //
    // form pages.                                                           //
    // --------------------------------------------------------------------- //
    [Serializable]
    public class LogOnPage : Page {
        private static int attempts;


        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public LogOnPage()
        : base("logon", "logon.page") { }


        // ----------------------------------------------------------------- //
        // This method is invoked at startup. Here we define all of the form //
        // controls associated with logging in or out.                       //
        // ----------------------------------------------------------------- //
        public override void DefinePage() {
            // logon form
            Dictionary<string, Widget> widgets = new Dictionary<string, Widget>();
            widgets.Add("header",   new WidgetTitle("Log On"));
            widgets.Add("label1",   new WidgetLabel(160, 196, 112, 32, "Username"));
            widgets.Add("label2",   new WidgetLabel(160, 236, 112, 32, "Password"));
            widgets.Add("username", new WidgetTextBox(272, 196, 320, 32, "", false, false, false));
            widgets.Add("password", new WidgetTextBox(272, 236, 320, 32, "", false, false, true));
            widgets.Add("submit",   new WidgetButton(464, 320, 128, 32, "Submit", OnSubmit));
            widgets.Add("exit",     new WidgetButton(272, 320, 128, 32, "Exit", OnExit));
            Form1.formgen.AddPage("logon.page", widgets);
        }


        // ----------------------------------------------------------------- //
        // This method is invoked on display, now we perform any last second //
        // adjustments to the controls associated with this page.            //
        // ----------------------------------------------------------------- //
        public override void OnLoadPage() {
            attempts = 3;
        }


        // ----------------------------------------------------------------- //
        // This method gets invoked when the user presses the exit button.   //
        // ----------------------------------------------------------------- //
        public void OnExit(object sender, EventArgs e) {
            Form1.form.OnExit();
        }


        // ----------------------------------------------------------------- //
        // This method gets invoked when the user presses the submit button. //
        // Log on credentials are checked and access is granted or revoked.  //
        // ----------------------------------------------------------------- //
        public void OnSubmit(object sender, EventArgs e) {
            if (--attempts == 0) {
                MessageBox.Show("Logon attempts exceeded!");
                Form1.form.OnExit();
            }

            string message = "Incorrect username and password\n";
            message += string.Format("You have {0} attempt{1} remaining",
                                     attempts, (attempts > 1) ? "s" : "");

            TextBox username = Form1.formgen.GetControl("logon.page", "username") as TextBox;
            TextBox password = Form1.formgen.GetControl("logon.page", "password") as TextBox;
            Label   nickname = Form1.formgen.GetControl("header.page", "logged") as Label;

            User logged = Form1.context.GetUser(username.Text);
            if (logged == null || logged.GetUserName() != username.Text) {
                MessageBox.Show(message);
                return;
            }

            if (logged.GetPassWord() != password.Text) {
                MessageBox.Show(message);
                return;
            }
 
            nickname.Text = username.Text;
            Form1.context.SetLogged(username.Text);
            Form1.formgen.BuildPage("game.list");
            attempts = 3;
        }
    }
    #endregion
}