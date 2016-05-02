// ========================================================================= //
// File Name : HeaderPage.cs                                                 //
// File Date : 12 April 2016                                                 //
// Author(s) : Michael Collins, Louise McKeown, Alan Rowlands                //
// File Info : Defines the ToolBar which is affixed to the top of every Page //
//             Allows for Logging Out, and Entity selection.                 //
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
    #region header
    // --------------------------------------------------------------------- //
    // LogOn is responsible for logging users into the system. If the log on //
    // attempt is successful, then the user is presented with appropriate    //
    // form pages.                                                           //
    // --------------------------------------------------------------------- //
    [Serializable]
    public class HeaderPage : Page {


        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public HeaderPage()
        : base("header", "header.page") { }


        // ----------------------------------------------------------------- //
        // This method is invoked at startup. Here we define all of the form //
        // controls associated with logging in or out.                       //
        // ----------------------------------------------------------------- //
        public override void DefinePage() {
            // header appears at the top of every form
            Dictionary<string, Widget> header = new Dictionary<string, Widget>();
            header.Add("exit", new WidgetButton(800-32-32, 16, 32, 32, "x", OnExit, -1));
            header.Add("logged", new WidgetLabel(800-64-256, 16, 128, 32,  "username"));
            header.Add("logout", new WidgetButton(800-64-128, 16, 128, 32, "Log Out", OnLogOut, -1));
            header.Add("games",  new WidgetRadio( 32, 16, 80, 32, "Games",  OnGamesClick));
            header.Add("users",  new WidgetRadio(112, 16, 100, 32, "Members",  OnUsersClick));
            header.Add("orders", new WidgetRadio(212, 16, 80, 32, "Orders", OnOrdersClick));
            header.Add("reports", new WidgetRadio(292, 16, 90, 32, "Reports", OnReportsClick));
            header.Add("staff", new WidgetRadio(382, 16, 70, 32, "Staff", OnStaffClick));

            Form1.formgen.AddPage("header.page", header);
            Label label = Form1.formgen.GetControl("header.page", "logged") as Label;
            if (label != null) label.TextAlign = ContentAlignment.MiddleRight;
            RadioButton games = Form1.formgen.GetControl("header.page", "games") as RadioButton;
            if (games != null) games.Appearance = Appearance.Button;
            if (games != null) games.Checked    = true;
            RadioButton users = Form1.formgen.GetControl("header.page", "users") as RadioButton;
            if (users != null) users.Appearance = Appearance.Button;
            RadioButton orders = Form1.formgen.GetControl("header.page", "orders") as RadioButton;
            if (orders != null) orders.Appearance = Appearance.Button;
            RadioButton reports = Form1.formgen.GetControl("header.page", "reports") as RadioButton;
            if (orders != null) reports.Appearance = Appearance.Button;
            RadioButton staff = Form1.formgen.GetControl("header.page", "staff") as RadioButton;
            if (orders != null) staff.Appearance = Appearance.Button;
        }


        // ----------------------------------------------------------------- //
        // This method gets invoked when the user presses the exit button.   //
        // ----------------------------------------------------------------- //
        public void OnExit(object sender, EventArgs e) {
            Form1.form.OnExit();
        }


        // ----------------------------------------------------------------- //
        // This method gets invoked when the user presses the logout button. //
        // Log on credentials are revoked and access is revoked.             //
        // ----------------------------------------------------------------- //
        public void OnGamesClick(object sender, EventArgs e) {
            Form1.formgen.BuildPage("game.list");
        }


        // ----------------------------------------------------------------- //
        // This method gets invoked when the user presses the logout button. //
        // Log on credentials are revoked and access is revoked.             //
        // ----------------------------------------------------------------- //
        public void OnUsersClick(object sender, EventArgs e) {
            Form1.formgen.BuildPage("user.list");
        }


        // ----------------------------------------------------------------- //
        // This method gets invoked when the user presses the logout button. //
        // Log on credentials are revoked and access is revoked.             //
        // ----------------------------------------------------------------- //
        public void OnReportsClick(object sender, EventArgs e) {
            Form1.formgen.BuildPage("report.list");
        }

        // ----------------------------------------------------------------- //
        // This method gets invoked when the user presses the logout button. //
        // Log on credentials are revoked and access is revoked.             //
        // ----------------------------------------------------------------- //
        public void OnStaffClick(object sender, EventArgs e)
        {
            Form1.formgen.BuildPage("staff.list");
        }
        // ----------------------------------------------------------------- //
        // This method gets invoked when the user presses the logout button. //
        // Log on credentials are revoked and access is revoked.             //
        // ----------------------------------------------------------------- //
        public void OnOrdersClick(object sender, EventArgs e)
        {
            Form1.formgen.BuildPage("order.list");
        }
        // ----------------------------------------------------------------- //
        // This method gets invoked when the user presses the logout button. //
        // Log on credentials are revoked and access is revoked.             //
        // ----------------------------------------------------------------- //
        public void OnLogOut(object sender, EventArgs e) {
            TextBox username = Form1.formgen.GetControl("logon.page", "username") as TextBox;
            TextBox password = Form1.formgen.GetControl("logon.page", "password") as TextBox;
            Label logged = Form1.formgen.GetControl("header.page", "logged") as Label;

            logged.Text = "";
            username.Text = "";
            password.Text = "";
            Form1.context.SetLogged("");
            Form1.formgen.BuildPage("logon.page");
        }
    }
    #endregion
}