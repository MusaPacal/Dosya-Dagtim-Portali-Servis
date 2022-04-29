using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using dosyadagitimportalison.Models;
using dosyadagitimportalison.ViewModel;

namespace dosyadagitimportalison.Controllers
{
    public class ServisController : ApiController
    {
        DB02Entities3 db = new DB02Entities3();
        SonucModel sonuc = new SonucModel();


        #region Kategoriler

        [HttpGet]
        [Route("api/kategoriliste")]
        public List<KategorilerModel> KategoriListe()
        {
            List<KategorilerModel> liste = db.Kategoriler.Select(x => new KategorilerModel()
            {
                KategoriId = x.KategoriId,
                KategoriAd = x.KategoriAd,
            }).ToList();
            return liste;
        }

        [HttpGet]
        [Route("api/kategorilerbyid/{kategoriId}")]
        public KategorilerModel KategorilerById(int katId)
        {
            KategorilerModel kayit = db.Kategoriler.Where(s => s.KategoriId == katId).Select(x
           => new KategorilerModel()
           {
               KategoriId = x.KategoriId,
               KategoriAd = x.KategoriAd,
           }).SingleOrDefault();
            return kayit;
        }
        [HttpPost]
        [Route("api/kategorilerekle")]
        public SonucModel KategorilerEkle(KategorilerModel model)
        {
            if (db.Kategoriler.Count(s => s.KategoriAd == model.KategoriAd) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen Kategori Adı Kayıtlıdır!";
                return sonuc;
            }

            Kategoriler yeni = new Kategoriler();
            yeni.KategoriAd = model.KategoriAd;
            db.Kategoriler.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Kategori Eklendi";
            return sonuc;
        }
        [HttpPut]
        [Route("api/kategoriduzenle")]
        public SonucModel KategorilerDuzenle(KategorilerModel model)
        {
            Kategoriler kayit = db.Kategoriler.Where(s => s.KategoriId == model.KategoriId).FirstOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }
            kayit.KategoriAd = model.KategoriAd;
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Kategori Düzenlendi";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/kategorilersil/{kategoriId}")]
        public SonucModel KategorilerSil(int kategoriId)
        {
            Kategoriler kayit = db.Kategoriler.Where(s => s.KategoriId == kategoriId).FirstOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }
            if (db.Dosyalar.Count(s => s.KategoriId == kategoriId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Üzerinde Makale Kayıtlı Kategori Silinemez!";
                return sonuc;
            }
            db.Kategoriler.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Kategori Silindi";
            return sonuc;
        }
        #endregion



        #region Dosyalar

        [HttpGet]
        [Route("api/dosyalarliste")]
        public List<DosyalarModel> DosyalarListe()
        {
            List<DosyalarModel> liste = db.Dosyalar.Select(x => new DosyalarModel()
            {
                DosyaId = x.DosyaId,
                DosyaBaslik = x.DosyaBaslik,
                DosyaIcerik = x.DosyaIcerik,
                DosyaTarih = x.DosyaTarih
            }).ToList();
            return liste;
        }

        [HttpGet]
        [Route("api/dosyalarlistesoneklenenler/{s}")]
        public List<DosyalarModel> DosyalarListeSonEklenenler(int s)
        {
            List<DosyalarModel> liste = db.Dosyalar.OrderByDescending(o => o.DosyaId).Take(
           s).Select(x => new DosyalarModel()
           {
               DosyaId = x.DosyaId,
               DosyaBaslik = x.DosyaBaslik,
               DosyaIcerik = x.DosyaIcerik,
               DosyaTarih = x.DosyaTarih
           }).ToList();
            return liste;
        }

        [HttpGet]
        [Route("api/dosyalarlistebykatid/{kategoriId}")]
        public List<DosyalarModel> DosyalarListeByKategoriId(int kategoriId)
        {
            List<DosyalarModel> liste = db.Dosyalar.Where(s => s.KategoriId == kategoriId).Select
           (x => new DosyalarModel()
           {
               DosyaId = x.DosyaId,
               DosyaBaslik = x.DosyaBaslik,
               DosyaIcerik = x.DosyaIcerik,
               DosyaTarih = x.DosyaTarih
           }).ToList();
            return liste;
        }

        [HttpGet]
        [Route("api/dosyalarlistebyuyeid/{uyeId}")]
        public List<DosyalarModel> DosyalarListeByUyeId(int uyeId)
        {
            List<DosyalarModel> liste = db.Dosyalar.Where(s => s.UyeId == uyeId).Select(x =>
           new DosyalarModel()

           {
               DosyaId = x.DosyaId,
               DosyaBaslik = x.DosyaBaslik,
               DosyaIcerik = x.DosyaIcerik,
               DosyaTarih = x.DosyaTarih,
               UyeId = x.UyeId,
               UyeAdSoyad = x.Uye.UyeAdSoyad
           }).ToList();
            return liste;
        }

        [HttpGet]
        [Route("api/dosyalarbyid/{dosyaId}")]
        public DosyalarModel DosyalarById(int dosyaId)
        {
            DosyalarModel kayit = db.Dosyalar.Where(s => s.DosyaId == dosyaId).Select(x =>
           new DosyalarModel()
           {

               DosyaId = x.DosyaId,
               DosyaBaslik = x.DosyaBaslik,
               DosyaIcerik = x.DosyaIcerik,
               DosyaTarih = x.DosyaTarih,
               UyeId = x.UyeId,
               UyeAdSoyad = x.Uye.UyeAdSoyad

           }).SingleOrDefault();
            return kayit;
        }

        [HttpPost]
        [Route("api/Dosyaekle")]
        public SonucModel DosyalarEkle(DosyalarModel model)
        {
            if (db.Dosyalar.Count(s => s.DosyaBaslik == model.DosyaBaslik) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen Dosya Başlığı Kayıtlıdır!";
                return sonuc;
            }
            Dosyalar yeni = new Dosyalar();
            yeni.DosyaBaslik = model.DosyaBaslik;
            yeni.DosyaIcerik = model.DosyaIcerik;
            yeni.DosyaTarih = model.DosyaTarih;
            yeni.KategoriId = model.KategoriId;
            yeni.UyeId = model.UyeId;
            db.Dosyalar.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Dosya Eklendi";
            return sonuc;
        }

        [HttpPut]
        [Route("api/dosyaduzenle")]
        public SonucModel DosyalarDuzenle(DosyalarModel model)
        {
            Dosyalar kayit = db.Dosyalar.Where(s => s.DosyaId == model.DosyaId).SingleOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }
            kayit.DosyaBaslik = model.DosyaBaslik;
            kayit.DosyaIcerik = model.DosyaIcerik;
            kayit.DosyaTarih = model.DosyaTarih;
            kayit.KategoriId = model.KategoriId;
            kayit.UyeId = model.UyeId;
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Dosya Düzenlendi";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/dosyasil/{dosyaId}")]
        public SonucModel DosyalarSil(int dosyaId)
        {
            Dosyalar kayit = db.Dosyalar.Where(s => s.DosyaId == dosyaId).SingleOrDefault(
           );
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }
            db.Dosyalar.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Dosya Silindi";
            return sonuc;
        }

        #endregion



        #region Uye

        [HttpGet]
        [Route("api/uyeliste")]
        public List<UyeModel> UyeListe()
        {
            List<UyeModel> liste = db.Uye.Select(x => new UyeModel()
            {
                UyeId = x.UyeId,
                UyeAdSoyad = x.UyeAdSoyad,
                UyeMail = x.UyeMail,
                UyeParola = x.UyeParola,
                YetkiId = x.YetkiId
            }).ToList();
            return liste;
        }


        [HttpGet]
        [Route("api/uyebyid/{uyeId}")]
        public UyeModel UyeById(int uyeId)
        {
            UyeModel kayit = db.Uye.Where(s => s.UyeId == uyeId).Select(x => new UyeModel()
            {
                UyeId = x.UyeId,
                UyeAdSoyad = x.UyeAdSoyad,
                UyeMail = x.UyeMail,
                UyeParola = x.UyeParola,
                YetkiId = x.YetkiId
            }).SingleOrDefault();
            return kayit;
        }

        [HttpPost]
        [Route("api/uyeekle")]
        public SonucModel UyeEkle(UyeModel model)
        {
            if (db.Uye.Count(s => s.UyeAdSoyad == model.UyeAdSoyad || s.UyeMail == model.UyeMail) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen Kullanıcı Adı veya E-Posta Adresi Kayıtlıdır!";
                return sonuc;
            }
            Uye yeni = new Uye();
            yeni.UyeAdSoyad = model.UyeAdSoyad;
            yeni.UyeMail = model.UyeMail;
            yeni.UyeParola = model.UyeParola;
            yeni.YetkiId = model.YetkiId;
            db.Uye.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Üye Eklendi";
            return sonuc;
        }

        [HttpPut]
        [Route("api/uyeduzenle")]
        public SonucModel UyeDuzenle(UyeModel model)
        {
            Uye kayit = db.Uye.Where(s => s.UyeId == model.UyeId).SingleOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı";
                return sonuc;
            }
            kayit.UyeAdSoyad = model.UyeAdSoyad;
            kayit.UyeMail = model.UyeMail;
            kayit.UyeParola = model.UyeParola;
            kayit.YetkiId = model.YetkiId;
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Üye Düzenlendi";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/uyesil/{uyeId}")]
        public SonucModel UyeSil(int uyeId)
        {
            Uye kayit = db.Uye.Where(s => s.UyeId == uyeId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı";
                return sonuc;
            }
            if (db.Dosyalar.Count(s => s.UyeId == uyeId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Üzerinde Makale Kaydı Olan Üye Silinemez!";
                return sonuc;
            }
            
            db.Uye.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Üye Silindi";
            return sonuc;
        }

        
        #endregion
    }
    }

