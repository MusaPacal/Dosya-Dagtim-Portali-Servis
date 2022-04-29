using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dosyadagitimportalison.ViewModel
{
    public class UyeModel
    {
        public int UyeId { get; set; }
        public string UyeAdSoyad { get; set; }
        public string UyeMail { get; set; }
        public int YetkiId { get; set; }
        public System.DateTime UyeTarih { get; set; }
        public int UyeParola { get; set; }
    }
}