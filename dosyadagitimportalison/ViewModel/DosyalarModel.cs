using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dosyadagitimportalison.ViewModel
{
    public class DosyalarModel
    {
        public int DosyaId { get; set; }
        public string DosyaBaslik { get; set; }
        public string DosyaIcerik { get; set; }
        public string DosyaKategorilerAdi  { get; set; }
        public System.DateTime DosyaTarih { get; set; }
        public int UyeId { get; internal set; }
        public string UyeAdSoyad { get; internal set; }
        public int KategoriId { get; internal set; }
    }
}