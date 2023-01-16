using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OgrenciNotMvc.Models.EntityFramework;
using OgrenciNotMvc.Models;
using System.Data.SqlTypes;

namespace OgrenciNotMvc.Controllers
{
    public class NotlarController : Controller
    {
        // GET: Notlar
        DbMvcOkulEntities db = new DbMvcOkulEntities();
        public ActionResult Index()
        {
            var SinavNotlar = db.TBLNOTLAR.ToList();
            return View(SinavNotlar);
        }
        [HttpGet]
        public ActionResult yeniSinav()
        {
            List<SelectListItem> degerler1 = (from i in db.TBLOGRENCILER.ToList()
                                              select new SelectListItem
                                              {
                                                  Text = i.OGRAD + " " + i.OGRSOYAD,
                                                  Value = i.OGRENCIID.ToString()
                                              }).ToList();
            ViewBag.dgr = degerler1;
            List<SelectListItem> degerler2 = (from i in db.TBLDERSLER.ToList()
                                              select new SelectListItem
                                              {
                                                  Text = i.DERSAD,
                                                  Value = i.DERSID.ToString()
                                              }).ToList();
            ViewBag.dgr1 = degerler2;
            return View();
        }
        [HttpPost]
        public ActionResult yeniSinav(TBLNOTLAR tbn)
        {
            var klp = db.TBLOGRENCILER.Where(m => m.OGRENCIID == tbn.TBLOGRENCILER.OGRENCIID).FirstOrDefault();
            var klp1 = db.TBLDERSLER.Where(m => m.DERSID == tbn.TBLDERSLER.DERSID).FirstOrDefault();
            tbn.TBLOGRENCILER = klp;
            tbn.TBLDERSLER = klp1;
            db.TBLNOTLAR.Add(tbn);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult NotGetir(int id)
        {
            List<SelectListItem> degerler1 = (from i in db.TBLOGRENCILER.ToList()
                                              select new SelectListItem
                                              {
                                                  Text = i.OGRAD + " " + i.OGRSOYAD,
                                                  Value = i.OGRENCIID.ToString()
                                              }).ToList();
            ViewBag.dgr = degerler1;
            List<SelectListItem> degerler2 = (from i in db.TBLDERSLER.ToList()
                                              select new SelectListItem
                                              {
                                                  Text = i.DERSAD,
                                                  Value = i.DERSID.ToString()
                                              }).ToList();
            ViewBag.dgr1 = degerler2;

            var notlar = db.TBLNOTLAR.Find(id);
            return View("NotGetir", notlar);
        }
        [HttpPost]
        public ActionResult NotGetir(Class1 model, TBLNOTLAR p, int SINAV1 = 0, int SINAV2 = 0, int SINAV3 = 0, int PROJE = 0)
        {


            List<SelectListItem> degerler1 = (from i in db.TBLOGRENCILER.ToList()
                                              select new SelectListItem
                                              {
                                                  Text = i.OGRAD + " " + i.OGRSOYAD,
                                                  Value = i.OGRENCIID.ToString()
                                              }).ToList();
            ViewBag.dgr = degerler1;

            List<SelectListItem> degerler2 = (from i in db.TBLDERSLER.ToList()
                                              select new SelectListItem
                                              {
                                                  Text = i.DERSAD,
                                                  Value = i.DERSID.ToString()
                                              }).ToList();
            ViewBag.dgr1 = degerler2;
            if (model.islem == "HESAPLA")
            {
                //islem 1
                int ORTALAMA = (SINAV1 + SINAV2 + SINAV3 + PROJE) / 4;
                ViewBag.ort = ORTALAMA;
                if (ORTALAMA <= 49)
                {
                    ViewBag.sonuc = "Kalır";
                }
                else
                {
                    ViewBag.sonuc = "Geçer";
                }

            }
            if (model.islem == "NOTGUNCELLE")
            {

                var snv = db.TBLNOTLAR.Find(p.NOTID);
                snv.DERSID = p.TBLDERSLER.DERSID;
                snv.OGRID = p.TBLOGRENCILER.OGRENCIID;
                snv.SINAV1 = p.SINAV1;
                snv.SINAV2 = p.SINAV2;
                snv.SINAV3 = p.SINAV3;
                snv.PROJE = p.PROJE;
                snv.ORTALAMA = p.ORTALAMA;
                snv.DURUM = p.DURUM;
                db.SaveChanges();
                return RedirectToAction("Index", "Notlar");
            }
            return View("NotGetir");
        }
    }
}