// ========================================================================= //
// File Name : Report.cs                                                //
// File Date : 12 April 2016                                                 //
// Author(s) : Michael Collins, Louise McKeown, Alan Rowlands                //
// File Info : The report class responsible for payment fees and such.  //
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
    [Serializable]
    public class Report : Entity {
        private string reportid;
        private string username;
        private string reportdate;
        private int rentalfees;
        private int latefees;
        private int memberfees;


        public string GetReportId() { return reportid; }
        public string GetUsername() { return username; }
        public string GetReportDate() { return reportdate; }
        public int GetRentalFees() { return rentalfees; }
        public int GetLateFees() { return latefees; }
        public int GetMemberFees() { return memberfees; }

        public void SetReportId(string ReportId)     { reportid   = ReportId;}
        public void SetUsername(string Username)     { username   = Username; }
        public void SetReportDate(string ReportDate) { reportdate = ReportDate; }
        public void SetRentalFee(int RentalFees)     { rentalfees = RentalFees; }
        public void SetLateFees(int LateFees)        { latefees   = LateFees; }
        public void SetMemberFees(int MemberFees)    { memberfees = MemberFees; }


        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public Report()
        : base("report") {
            reportid   = "";
            username   = "";
            reportdate = "";
            rentalfees = 3;
            latefees   = 1; //per day
            memberfees = 20;
        }


        // ----------------------------------------------------------------- //
        // Factory constructor.                                              //
        // ----------------------------------------------------------------- //
        public Report(string ReportId, string UserName, string ReportDate,
                      int RentalFees, int LateFees, int MemberFees)
        : base("report") {
            reportid   = ReportId;
            username   = UserName;
            reportdate = ReportDate;
            rentalfees = RentalFees;
            latefees   = LateFees;
            memberfees = MemberFees;
        }
        

        public override string Read() {
            string text = "\n Transaction";
            text = text + "\n ReportId    = "+reportid.ToString();
            text = text + "\n UserName    = "+username.ToString();
            text = text + "\n ReportDate  = "+reportdate.ToString();
            text = text + "\n Rental Fees = "+rentalfees.ToString();
            text = text + "\n Late Fees   = "+latefees.ToString();
            text = text + "\n Member Fees = "+memberfees.ToString();
            return text + "\n";
        }


        // ----------------------------------------------------------------- //
        // pure virtuals                                                     //
        // ----------------------------------------------------------------- //
        public override bool RegexMatch(Regex regex) {
            if (regex.Match(reportid).Success) return true;
            if (regex.Match(username).Success) return true;
            if (regex.Match(reportdate).Success) return true;
            if (regex.Match(rentalfees.ToString()).Success) return true;
            if (regex.Match(latefees.ToString()).Success) return true;
            if (regex.Match(memberfees.ToString()).Success) return true;
            return false;
        }
    }
}