using System;
using System.Collections;
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
    #region widget
    // --------------------------------------------------------------------- //
    // Widgets are an internal representation of the Form Controls that make //
    // up a form. This class is used to define the structure of a page.      //
    // In this manner forms are created dynamically at runtime.              //
    // --------------------------------------------------------------------- //
    [Serializable]
    public abstract class Widget {
        public int posx;
        public int posy;
        public int wide;
        public int high;

        public Widget(int PosX, int PosY, int Wide, int High) {
            posx = PosX;
            posy = PosY;
            wide = Wide;
            high = High;
        }

        public abstract Control Build();
    }
    #endregion


    #region titlewidgets
    // --------------------------------------------------------------------- //
    // Widget Title Label Mockup class                                       //
    // --------------------------------------------------------------------- //
    [Serializable]
    public class WidgetTitle : Widget {
        public string text;

        public WidgetTitle(string Text)
        : base(Form1.form.Width/2, 80, 176, 32) {
            text = Text;
        }


        // build a form control from the widget mockup
        public override Control Build() {
            Label label    = new Label();
            label.Text     = text;
            label.Font     = new Font("Verdana", 18.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            //label.Location = new Point(posx, posy);
            label.Location = new Point(posx - (int)label.Font.SizeInPoints*text.Length/2, posy);
            label.Size     = new Size(wide, high);
            return label as Control;
        }
    }
    #endregion


    #region label
    // --------------------------------------------------------------------- //
    // Widget Label Mockup class                                             //
    // --------------------------------------------------------------------- //
    [Serializable]
    public class WidgetLabel : Widget {
        public string text;

        public WidgetLabel(int PosX, int PosY, int Wide, int High, string Text)
        : base(PosX, PosY, Wide, High) {
            text = Text;
        }


        // build a form control from the widget mockup
        public override Control Build() {
            Label label    = new Label();
            label.Text     = text;
            label.Font     = new Font("Verdana", 12.75F);
            label.Location = new Point(posx, posy);
            label.Size     = new Size(wide, high);
            return label as Control;
        }
    }
    #endregion


    #region button
    // --------------------------------------------------------------------- //
    // Widget Button Mockup class                                            //
    // --------------------------------------------------------------------- //
    [Serializable]
    public class WidgetButton : Widget {
        public string text;
        public Action<object, EventArgs> method;

        public WidgetButton(int PosX, int PosY, int Wide, int High,
                            string Text, Action<object, EventArgs> Method)
        : base(PosX, PosY, Wide, High) {
            text   = Text;
            method = Method;
        }


        // build a form control from the widget mockup
        public override Control Build() {
            Button button   = new Button();
            button.Text     = text;
            button.Font     = new Font("Verdana", 12.75F);
            button.Location = new Point(posx, posy);
            button.Size     = new Size(wide, high);
            button.Click   += new System.EventHandler(method);
            return button as Control;
        }
    }
    #endregion


    #region radio
    // --------------------------------------------------------------------- //
    // Widget Button Mockup class                                            //
    // --------------------------------------------------------------------- //
    [Serializable]
    public class WidgetRadio : Widget {
        public string text;
        public Action<object, EventArgs> method;

        public WidgetRadio(int PosX, int PosY, int Wide, int High,
                           string Text, Action<object, EventArgs> Method)
        : base(PosX, PosY, Wide, High) {
            text   = Text;
            method = Method;
        }


        // build a form control from the widget mockup
        public override Control Build() {
            RadioButton radio = new RadioButton();
            radio.Text        = text;
            radio.Font        = new Font("Verdana", 12.75F);
            radio.Location    = new Point(posx, posy);
            radio.Size        = new Size(wide, high);
            radio.Click      += new System.EventHandler(method);
            return radio as Control;
        }
    }
    #endregion


    #region textbox
    // --------------------------------------------------------------------- //
    // Widget TextBox Mockup class                                           //
    // --------------------------------------------------------------------- //
    [Serializable]
    public class WidgetTextBox : Widget {
        public string text;
        public bool   read_only;
        public bool   multiline;
        public bool   password;

        public WidgetTextBox(int PosX, int PosY, int Wide, int High,
                             string Text, bool ReadOnly, bool MultiLine,
                             bool Password)
        : base(PosX, PosY, Wide, High) {
            text      = Text;
            read_only = ReadOnly;
            multiline = MultiLine;
            password  = Password;
        }


        // build a form control from the widget mockup
        public override Control Build() {
            TextBox textbox   = new TextBox();
            textbox.Text      = text;
            textbox.Font      = new Font("Verdana", 12.75F);
            textbox.Location  = new Point(posx, posy);
            textbox.Size      = new Size(wide, high);
            textbox.ReadOnly  = read_only;
            textbox.Multiline = multiline;
            if (password) textbox.PasswordChar = '*';
            return textbox as Control;
        }
    }
    #endregion


    #region listivew
    // --------------------------------------------------------------------- //
    // Widget ListView Mockup class                                          //
    // --------------------------------------------------------------------- //
    [Serializable]
    public class WidgetListView : Widget {
        public Action<object, EventArgs> onclickrecord;
        public Action<object, EventArgs> onmakevisible;

        public WidgetListView(int PosX, int PosY, int Wide, int High,
                              Action<object, EventArgs> OnClickRecord,
                              Action<object, EventArgs> OnMakeVisible)
        : base(PosX, PosY, Wide, High) {
            onclickrecord = OnClickRecord;
            onmakevisible = OnMakeVisible;
        }


        // build a form control from the widget mockup
        public override Control Build() {
            ListView listview  = new ListView();
            listview.Font      = new Font("Verdana", 12.75F);
            listview.Location  = new Point(posx, posy);
            listview.Size      = new Size(wide, high);

            listview.UseCompatibleStateImageBehavior = false;
            listview.View            = View.Details;
          //listview.Dock            = DockStyle.Fill;
            listview.FullRowSelect   = true;
            listview.GridLines       = true;
            listview.ColumnClick    += OnClickColumn;
            listview.ItemActivate   += new System.EventHandler(onclickrecord);
            listview.VisibleChanged += new EventHandler(onmakevisible);
            listview.ListViewItemSorter = new ListComparer(0, listview.Sorting);
            return listview as Control;
        }

        // sort the list view according to the column selected
        public void OnClickColumn(object sender, ColumnClickEventArgs e) {
            ListView listview = sender as ListView;

            // ascending or descending?
            if (listview.Sorting == SortOrder.Ascending) {
                listview.Sorting = SortOrder.Descending;
            } else {
                listview.Sorting = SortOrder.Ascending;
            }

            // sort by appropriate column
            ListComparer sorter = listview.ListViewItemSorter as ListComparer;
            if (sorter == null) {
                sorter = new ListComparer(e.Column, listview.Sorting);
                listview.ListViewItemSorter = sorter;
            } else {
                sorter.column = e.Column;
                sorter.order  = listview.Sorting;
            }

            listview.Sort();
        }
    }


    public class ListComparer : IComparer {
        public int column;
        public SortOrder order;

        public ListComparer(int Column, SortOrder Order) {
            column = Column;
            order  = Order;
        }


        public int Compare(object a, object b) {
            int result;
            ListViewItem itema = a as ListViewItem;
            ListViewItem itemb = b as ListViewItem;
            if (itema == null && itemb == null) {
                result = 0;
            } else if (itema == null) {
                result = -1;
            } else if (itemb == null) {
                result = 1;
            }
            if (itema == itemb) {
                result = 0;
            }
            //alphabetic comparison
            result = String.Compare(itema.SubItems[column].Text, itemb.SubItems[column].Text);
            return (order == SortOrder.Ascending) ? result : -result;
        }
    }
    #endregion


    #region listbox
    // --------------------------------------------------------------------- //
    // Widget ListBoxMockup class                                            //
    // --------------------------------------------------------------------- //
    [Serializable]
    public class WidgetListBox : Widget {
        public Action<object, EventArgs> onclickrecord;
        public string[] records;

        public WidgetListBox(int PosX, int PosY, int Wide, int High,
                             Action<object, EventArgs> OnClickRecord,
                             string[] Records)
        : base(PosX, PosY, Wide, High) {
            onclickrecord = OnClickRecord;
            records = Records;
        }


        // build a form control from the widget mockup
        public override Control Build() {
            ListBox listbox = new ListBox();
            listbox.Font      = new Font("Verdana", 12.75F);
            listbox.Location  = new Point(posx, posy);
            listbox.Size      = new Size(wide, high);

            listbox.Items.AddRange(records);
            listbox.FormattingEnabled    = true;
            listbox.SelectedIndex        = 0;
            listbox.SelectedIndexChanged += new System.EventHandler(onclickrecord);

            return listbox as Control;
        }
    }
    #endregion


}