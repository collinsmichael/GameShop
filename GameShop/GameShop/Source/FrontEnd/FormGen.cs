// ========================================================================= //
// File Name : FormGen.cs                                                    //
// File Date : 12 April 2016                                                 //
// Author(s) : Michael Collins, Louise McKeown, Alan Rowlands                //
// File Info : The FormGen class is responsible for dynamic form creation.   //
//             The form is generated dynamically in response to the users    //
//             intentions. Access to form controls must go through FormGen.  //
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
    public class FormGen {
        // each page has its own list of controls
        public Dictionary<string, Dictionary<string, Control>> control_dict;
        public Dictionary<string, Dictionary<string, Widget>>  widgets_dict;
        public Dictionary<string, Page>  page_dict;
        private BlackList blacklist;


        #region controliio
        // ----------------------------------------------------------------- //
        // GetControl allows internal access to the contents of the controls //
        // created at runtime. To access a control you must provide the name //
        // of the containing page, and the name of the control on that page. //
        // ----------------------------------------------------------------- //
        public Control GetControl(string pagename, string key) {
            string formname = GetFormName(pagename);

            Control control = new Control();
            Dictionary<string, Control> controls;

            if (control_dict.TryGetValue(formname, out controls)
            && controls.TryGetValue(key, out control)) {
                return control;
            }

            if (control_dict.TryGetValue(pagename, out controls)
            && controls.TryGetValue(key, out control)) {
                return control;
            }

            MessageBox.Show("The '" + key + "' control was not found!");
            return control;
        }
        #endregion


        #region pageio
        // ----------------------------------------------------------------- //
        // AddPage is used to add a defined form page to the dictionary.     //
        // Once a page has been added it can later be referred to by name.   //
        // ----------------------------------------------------------------- //
        public bool AddPage(string key, Dictionary<string, Widget> widgets) {
            Dictionary<string, Control> controls = new Dictionary<string, Control>();
            foreach (KeyValuePair<string, Widget> widget in widgets) {
                if (controls.ContainsKey(widget.Key)) controls.Remove(widget.Key);
                Control control = widget.Value.Build();
                control.Name = key+"."+widget.Key;
                controls.Add(widget.Key, control);
            }

            if (widgets_dict.ContainsKey(key)) widgets_dict.Remove(key);
            widgets_dict.Add(key, widgets);

            if (control_dict.ContainsKey(key)) control_dict.Remove(key);
            control_dict.Add(key, controls);
            return true;
        }


        // ----------------------------------------------------------------- //
        // GetPage retrieves the Page manager by name.                       //
        // ----------------------------------------------------------------- //
        public Page GetPage(string pagename) {
            Page page = null;
            if (page_dict.TryGetValue(pagename, out page)) {
                return page;
            }
            return null;
        }
        #endregion


        #region build
        // ----------------------------------------------------------------- //
        // BuildHeader is responsible for building the common header form    //
        // controls that appear at the top of every page.                    //
        // ----------------------------------------------------------------- //
        public bool BuildHeader() {
            // obtain the list of associated controls
            Dictionary<string, Control> controls;
            if (!control_dict.TryGetValue("header.page", out controls)) {
                MessageBox.Show("Page 'header' does not have any controls associated with it!");
                return false;
            }

            foreach (KeyValuePair<string, Control> control in controls) {
                string fullyqualified = "header.page." + control.Key;
                if (blacklist.IsBlackListed(fullyqualified)) {
                    control.Value.Hide();
                    continue;
                }
                control.Value.Show();
                Form1.form.Controls.Add(control.Value);
            }
            return true;
        }


        public void ClearPage(string pagename) {
            string[] parts = pagename.Split(new char[] {'.'});
            if (parts[0] == "logon") Form1.form.Controls.Clear();
            else BuildHeader();
            
            List<Control> droplist = new List<Control>();
            foreach (Control control in Form1.form.Controls) {
                if (!control.Name.StartsWith("header")) droplist.Add(control);
            }
            foreach (Control control in droplist) {
                Form1.form.Controls.Remove(control);
            }
        }

        // ----------------------------------------------------------------- //
        // BuildPage is responsible for dynamic runtime form generation.     //
        // Form widgets_dict are identified by name. This name is used as a key to  //
        // an entry in a dictionary. The controls are retrieved and added to //
        // the form.                                                         //
        // ----------------------------------------------------------------- //
        public bool BuildPage(string pagename) {
            if (blacklist.IsBlackListed(pagename)) return false;
            string[] parts = pagename.Split(new char[] {'.'});
            string formname = GetFormName(pagename);

            // obtain the list of associated controls
            Dictionary<string, Control> controls;
            if (!control_dict.TryGetValue(formname, out controls)) {
                MessageBox.Show("Page '" + pagename + "' does not have any controls associated with it!");
                return false;
            }

            // is the form read only?
            foreach (KeyValuePair<string, Control> control in controls) {
                TextBox textbox = control.Value as TextBox;
                if (textbox == null) continue;

                if (parts.Count() > 1) switch (parts[1].ToLower()) {
                case "make": case "edit": textbox.ReadOnly = false; break;
                case "view": case "drop": textbox.ReadOnly = true;  break;
                }
            }

            // clear the form
            Form1.form.SuspendLayout();
            ClearPage(pagename);

            // populate the form
            foreach (KeyValuePair<string, Control> control in controls) {
                string fullyqualified = pagename + "." + control.Key;
                if (blacklist.IsBlackListed(fullyqualified)) {
                    control.Value.Hide();
                    continue;
                }
                control.Value.Show();
                Form1.form.Controls.Add(control.Value);
            }

            Dictionary<string, Control> extras;
            control_dict.TryGetValue(pagename, out extras);
            foreach (KeyValuePair<string, Control> control in extras) {
                string fullyqualified = pagename + "." + control.Key;
                if (blacklist.IsBlackListed(fullyqualified)) {
                    continue;
                }
                Form1.form.Controls.Add(control.Value);
            }

            if (parts[1] == "list") {
                ListView listview = GetControl(parts[0] + ".list", "listview") as ListView;
                //Form1.context.GetSelected(parts[0]).
                ListPage listpage = Form1.formgen.GetPage(parts[0] + ".list") as ListPage;
                listpage.ListViewMakeVisible(listview, new EventArgs());
            }

            // activate new form layout
            Page page = GetPage(pagename);
            if (page != null) page.OnLoadPage();
            Form1.form.ResumeLayout();
            return true;
        }
        #endregion


        #region internal
        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public FormGen() {
            control_dict = new Dictionary<string, Dictionary<string, Control>>();
            widgets_dict = new Dictionary<string, Dictionary<string, Widget>>();
            page_dict    = new Dictionary<string, Page>();
            blacklist    = new BlackList();
        }


        // ----------------------------------------------------------------- //
        // Startup prior to runtime                                          //
        // ----------------------------------------------------------------- //
        public void OnLoad() {
            FileStream file;
            BinaryFormatter formatter;

            try {
                file = new FileStream("forms.bin", FileMode.Open, FileAccess.Read);
                formatter = new BinaryFormatter();
                widgets_dict = formatter.Deserialize(file) as Dictionary<string, Dictionary<string, Widget>>;
                file.Close();
            } catch {
                MessageBox.Show("File not found! 'forms.bin'");
            }

            // update controls from deserialized file
            foreach (KeyValuePair<string, Dictionary<string, Widget>> page in widgets_dict) {
                Dictionary<string, Control> controls = new Dictionary<string, Control>();
                foreach (KeyValuePair<string, Widget> widget in page.Value) {
                    controls.Add(widget.Key, widget.Value.Build());
                }
                control_dict.Add(page.Key, controls);
            }

            // Initialize the Form Pages
            page_dict.Add("header.page", new HeaderPage());
            page_dict.Add("logon.page",  new LogOnPage());
            page_dict.Add("user.list",   new UserListPage());
            page_dict.Add("user.form",   new UserFormPage());
            page_dict.Add("user.view",   new UserViewPage());
            page_dict.Add("user.edit",   new UserEditPage());
            page_dict.Add("user.make",   new UserMakePage());
            page_dict.Add("user.drop",   new UserDropPage());
            page_dict.Add("staff.list",  new StaffListPage());
            page_dict.Add("staff.form",  new StaffFormPage());
            page_dict.Add("staff.view",  new StaffViewPage());
            page_dict.Add("staff.edit",  new StaffEditPage());
            page_dict.Add("staff.make",  new StaffMakePage());
            page_dict.Add("staff.drop",  new StaffDropPage());
            page_dict.Add("game.list",   new GameListPage());
            page_dict.Add("game.form",   new GameFormPage());
            page_dict.Add("game.view",   new GameViewPage());
            page_dict.Add("game.edit",   new GameEditPage());
            page_dict.Add("game.make",   new GameMakePage());
            page_dict.Add("game.drop",   new GameDropPage());
            page_dict.Add("order.list",  new OrderListPage());
            page_dict.Add("order.form",  new OrderFormPage());
            page_dict.Add("order.view",  new OrderViewPage());
            page_dict.Add("order.edit",  new OrderEditPage());
            page_dict.Add("order.make",  new OrderMakePage());
            page_dict.Add("report.list", new ReportListPage());
            page_dict.Add("report.form", new ReportFormPage());
            page_dict.Add("report.view", new ReportViewPage());
            page_dict.Add("report.edit", new ReportEditPage());
            page_dict.Add("report.make", new ReportMakePage());
           

            foreach (KeyValuePair<string, Page> page in page_dict) {
                page.Value.DefinePage();
            }
            BuildPage("logon.page");
        }


        // ----------------------------------------------------------------- //
        // Cleanup prior to shutdown                                         //
        // ----------------------------------------------------------------- //
        public void OnExit() {
            FileStream file;
            BinaryFormatter formatter;

            file = new FileStream("forms.bin", FileMode.OpenOrCreate, FileAccess.Write);
            formatter = new BinaryFormatter();
            formatter.Serialize(file, widgets_dict);
            file.Close();
        }


        // ----------------------------------------------------------------- //
        // Converts pagenames to formnames which are used as dictionary keys //
        // ----------------------------------------------------------------- //
        private string GetFormName(string pagename) {
            string[] parts = pagename.Split(new char[] {'.'});
            if (parts.Count() > 1) switch (parts[1].ToLower()) {
            case "list": return parts[0] + ".list";
            case "view": return parts[0] + ".form";
            case "edit": return parts[0] + ".form";
            case "make": return parts[0] + ".form";
            case "drop": return parts[0] + ".form";
            case "form": return parts[0] + ".form";
            }
            return pagename;
        }
        #endregion
    }
}