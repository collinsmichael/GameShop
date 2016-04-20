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
    [Serializable]
    public class Transaction:Entity
    {
        private string transactionId;
        private int rentalFee;
        private int lateReturnFee;
        private int membershipFee;
        private string username;


        public int GetRentalFee() { return rentalFee; }
        public int GetLateReturnFee() { return lateReturnFee; }
        public int GetMembershipFee() { return membershipFee; }
        public string GetUsername() { return username; }
        public string GetTransactionId() { return transactionId; }

        public void SetRentalFee(int RentalFee) { rentalFee = RentalFee; }
        public void SetLateReturnFee(int LateReturnFee) { lateReturnFee = LateReturnFee; }
        public void SetMembershipFee(int MembershipFee) { membershipFee = MembershipFee; }
        public void SetTransactionId(string TransactionId) {transactionId=TransactionId;}
        public void SetUsername(string Username) { username = Username; }


        // ----------------------------------------------------------------- //
        // Default constructor.                                              //
        // ----------------------------------------------------------------- //
        public Transaction()
            : base("transaction")
        {
            transactionId = " ";
            rentalFee = 3;
            lateReturnFee = 1;//per day
            membershipFee = 20;
            username = " ";
        }


        // ----------------------------------------------------------------- //
        // Factory constructor.                                              //
        // ----------------------------------------------------------------- //
        public Transaction(int RentalFee, int LateReturnFee, int MembershipFee,string Username,string TransactionId)
            : base("transaction")
        {
            transactionId = TransactionId;
            rentalFee = RentalFee;
            lateReturnFee = LateReturnFee;
            membershipFee = MembershipFee;
            username = Username;

        }

        // ----------------------------------------------------------------- //
        // pure virtuals                                                     //
        // ----------------------------------------------------------------- //
        public override bool RegexMatch(Regex regex)
        {
            if (regex.Match(rentalFee.ToString()).Success) return true;
            if (regex.Match(lateReturnFee.ToString()).Success) return true;
            if (regex.Match(membershipFee.ToString()).Success) return true;
            if (regex.Match(username).Success) return true;
            return false;
        }
    }
}

    

