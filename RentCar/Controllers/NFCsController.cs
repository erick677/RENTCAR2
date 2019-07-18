using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RentCar.Models;

namespace RentCar.Controllers
{
    public class NFCsController : Controller
    {
        private rentcar4Entities db = new rentcar4Entities();

        // GET: NFCs
        public ActionResult Index()
        {
            return View(db.NFC.ToList());
        }

        // GET: NFCs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NFC nFC = db.NFC.Find(id);
            if (nFC == null)
            {
                return HttpNotFound();
            }
            return View(nFC);
        }

        // GET: NFCs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NFCs/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idNFC,NFC1,Estatus")] NFC nFC)
        {
            if (ModelState.IsValid)
            {
                db.NFC.Add(nFC);
                nFC.Estatus = "Disponible";
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nFC);
        }

        // GET: NFCs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NFC nFC = db.NFC.Find(id);
            if (nFC == null)
            {
                return HttpNotFound();
            }
            return View(nFC);
        }

        // POST: NFCs/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idNFC,NFC1,Estatus")] NFC nFC)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nFC).State = EntityState.Modified;
                nFC.Estatus = "Disponible";
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nFC);
        }

        // GET: NFCs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NFC nFC = db.NFC.Find(id);
            if (nFC == null)
            {
                return HttpNotFound();
            }
            return View(nFC);
        }

        // POST: NFCs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NFC nFC = db.NFC.Find(id);
            db.NFC.Remove(nFC);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
