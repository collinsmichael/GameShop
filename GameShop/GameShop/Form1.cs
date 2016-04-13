// ========================================================================= //
// File Name : Form1.cs                                                      //
// File Date : 12 April 2016                                                 //
// Author(s) : Michael Collins, Louise McKeown, Alan Redding                 //
// File Info : The Form may be blank at compile time but it is truly dynamic //
//             at runtime. The Context class is the interface used to manage //
//             all the dynamic form controls.                                //
// ========================================================================= //

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace GameShop {
    public partial class Form1 : Form {
        public static Form1   form;
        public static Context context;
        public static FormGen formgen;


        #region startup_cleanup
        // ----------------------------------------------------------------- //
        // Startup prior to runtime                                          //
        // ----------------------------------------------------------------- //
        public Form1() {
            InitializeComponent();

            form = this;
            context = new Context();
            context.OnLoad();

            formgen = new FormGen();
            formgen.OnLoad();
        }


        // ----------------------------------------------------------------- //
        // Cleanup prior to shutdown                                         //
        // ----------------------------------------------------------------- //
        public void OnExit() {
            context.OnExit();
            formgen.OnExit();
            Application.Exit();
        }
        #endregion


        #region underthehood
        // ----------------------------------------------------------------- //
        // Low(ish) Level Windows Message Processing                         //
        // Allows Forms with no TitleBar to be moved by mouse control        //
        // ----------------------------------------------------------------- //
        protected override void WndProc(ref Message m) {
            const int WM_NCHITTEST  = 0x84;  // NON CLIENT HIT TEST (MouseDown)
            const int WM_ENDSESSION = 0x16;  // Dispatched when Power Off Button pressed
            const int WM_CLOSE      = 0x10;  // Dispatched when close or ALT+F4 pressed

            IntPtr HTCAPTION = (IntPtr)0x02; // Event occurred in TitleBar
            IntPtr HTCLIENT  = (IntPtr)0x01; // Event occurred in Client Area

            base.WndProc(ref m);             // Let Windows do its own thing
            switch (m.Msg) {                 // Handle messages of interest
            case WM_NCHITTEST:               // If there was a mouse event
                if (m.Result == HTCLIENT) {  // Occurring in the ClientArea
                    m.Result = HTCAPTION;    // Treat it as a TitleBar event
                }
                break;
            case WM_ENDSESSION:              // If the Power Off Button was pressed
            case WM_CLOSE:                   // If the user initiated a close
                OnExit();                    // Then shut down cleanly
                break;
            }
        }
        #endregion


    }
}