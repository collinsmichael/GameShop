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
         public Staff(string username,string password,string firstname,string surname,string email,string address, string phoneno,string dateofbirth)
          :base(username,password,firstname,surname,email,address,phoneno,dateofbirth)
         {}


             ~Staff()
           {
          
           }






         }
    }
