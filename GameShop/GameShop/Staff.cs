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
    public class Staff:User
    {
         public string staffId;
         public Staff(string username,string password,string firstname,string surname,string email,string address, string phoneno,string dateofbirth)
          :base(username,password,firstname,surname,email,address,phoneno,dateofbirth)
         {}

         public Staff()
         { }
             ~Staff()
           {
          
           }

             public string GetStaffId() { return staffId;}

             

             public bool SetStaffId(string StaffId)
             {
                 Regex regexphone = new Regex(@"^[0-9]{1,4}-[0-9-]{1,12}.([0-9]{1,10})$");
                 if (regexphone.Match(StaffId).Success) staffId = StaffId;
                 else MessageBox.Show("'" + StaffId + "'\nIs not a valid phone number");
                 return regexphone.Match(StaffId).Success;
             }

              






               public override bool RegexMatch(Regex regex)
             {
                 if (regex.Match(staffId).Success) return true;
                
                
                 return false;
             }

         }
    }
