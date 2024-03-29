﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RentCar.Models;
using Rotativa;

namespace RentCar.Controllers
{
    public class contratoesController : Controller
    {
        private rentcar4Entities2 db = new rentcar4Entities2();
        private datosparaimprimir dat = new datosparaimprimir();
        int? idgeneral;
        // GET: contratoes
        public ActionResult Index()
        {
            var contrato = db.contrato.Include(c => c.Clasevehiculo).Include(c => c.Vehiculo);
            return View(contrato.ToList());
        }
        /*-----------------Contratos----------------------------------------*/
        public ActionResult ImprimirContrato(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            contrato contrato = db.contrato.Find(id);
            if (contrato == null)
            {
                return HttpNotFound();
            }
            return View(contrato);
        }
        
        public ActionResult Pdf(int? id)
        {
            return new ActionAsPdf("ImprimirContrato", new { id = id });
        }
        /*--------------Facturas-----------------------------------------*/
        public ActionResult ImprimirFactura(int? id)
        {
            dat.Id = id;
            contrato contrato2 = db.contrato.Find(id);


            var subtotal = Convert.ToDecimal(contrato2.Total);
            var itbis = Convert.ToDecimal(1.18);

            var total = subtotal * itbis;

            Double doubl = Math.Round((Double)total, 2);

            ViewBag.Total = doubl;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            contrato contrato = db.contrato.Find(id);
            if (contrato == null )
            {
                return HttpNotFound();
            }

            return View(contrato);
        }
        
        public ActionResult PdfFactura(int? id)
        {
            Facturas factura = new Facturas();
            dat.Id = id;
            var actionPDF = new Rotativa.ActionAsPdf("ImprimirFactura", new { id = id }) //some route values) 
            {
                //FileName = "TestView.pdf", 


            };
            byte[] applicationPDFData = actionPDF.BuildFile(this.ControllerContext);
            factura.Factura = applicationPDFData;
            db.Facturas.Add(factura);
            
            return actionPDF;
        }
        public ActionResult Imprimir1(int? id)
        {
            Facturas factura = new Facturas();
            dat.Id = id;
            var actionPDF = new Rotativa.ActionAsPdf("ImprimirFactura", new { id = id }) //some route values) 
            {
                //FileName = "TestView.pdf", 


            };
            byte[] applicationPDFData = actionPDF.BuildFile(this.ControllerContext);
            factura.Factura = applicationPDFData;
            db.Facturas.Add(factura);
            db.SaveChanges();
            return actionPDF;
            

        }


        public ActionResult MostrarFacturas()
        {
            var contrato = db.Facturas;

            return View(contrato.ToList());
        }

        public ActionResult DatosComprobante(int? id)
        {
            ViewBag.FK_Vehiculo = new SelectList(db.NFC.Where(a => a.Estatus == "Disponible"));

            return PartialView(new { id = id });
        }

        [HttpPost]
        public ActionResult DatosComprobante(string NFC, string rnc, string nombreEmpresa, string direccionEmpresa )
        {

            ViewBag.FK_Vehiculo = new SelectList(db.NFC.Where(a => a.Estatus == "Disponible"));
            return View();
        }



        // GET: contratoes Contratos
        public ActionResult Contratos()
        {
            var contrato = db.contrato.Where(a => a.Tipo_Renta == "Contrato").Include(c => c.Clasevehiculo).Include(c => c.Vehiculo);
            return View(contrato.ToList());
        }
        // GET: contratoes Reservas
        public ActionResult Reservas()
        {
            var contrato = db.contrato.Where(a => a.Tipo_Renta == "Reserva").Include(c => c.Clasevehiculo).Include(c => c.Vehiculo);
            return View(contrato.ToList());
        }
        public ActionResult contratoscerrados()
        {
            var contrato = (from s in db.contratohistory select s);
            return View(contrato.ToList());
        }
        /*-------------------Contratos que cumplieron su limite--------------*/
        public ActionResult ContratosQueHanCumplido()
        {
            var contrato = db.contrato.Where(a => a.Tipo_Renta == "Contrato").Include(c => c.Clasevehiculo).Include(c => c.Vehiculo).Where(m => m.Fecha_Cierre <= DateTime.Today && m.Estatus == "Abierto");
            return View(contrato.ToList());
        }
        /*-------------------Reservas que cumplieron su limite--------------*/
        public ActionResult ReservasQueHanCumplido()
        {
            var contrato = db.contrato.Where(a => a.Tipo_Renta == "Reserva").Include(c => c.Clasevehiculo).Include(c => c.Vehiculo).Where(m => m.Fecha_Cierre <= DateTime.Today && m.Estatus == "Abierto");
            return View(contrato.ToList());
        }
        public ActionResult detalle(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Facturas contrato = db.Facturas.Find(id);
            if (contrato == null)
            {
                return HttpNotFound();
            }
            return File(contrato.Factura, "application/pdf");
        }

        // GET: contratoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            contrato contrato = db.contrato.Find(id);
            if (contrato == null)
            {
                return HttpNotFound();
            }
            return View(contrato);
        }

        // GET: contratoes/Create
        public ActionResult Create()
        {

            ViewBag.FK_kilometraje = new SelectList(db.Vehiculo, "id", "Kilometraje");
            ViewBag.FK_Combustible = new SelectList(db.Vehiculo, "id", "Combustible");

            ViewBag.idClase = new SelectList(db.Clasevehiculo, "id", "Clase");
            ViewBag.FK_Vehiculo = new SelectList(db.Vehiculo.Where(a => a.Estatus == "Disponible"), "VehiculoId", "Marca");
            return View();
        }

        public JsonResult GetFk_Vehiculo(int VehiculoId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Vehiculo> vehiculos = db.Vehiculo.Where(m => m.idClase == VehiculoId && m.Estatus == "Disponible").ToList();

            //ViewBag.FK_Vehiculo = new SelectList(db.Vehiculo.Where(a => a.Clase == Clase && a.Estatus == "Disponible"), "VehiculoId", "Marca");
            return Json(vehiculos, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetFk_Kilmetraje(int VehiculoId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Vehiculo> vehiculos = db.Vehiculo.Where(m => m.VehiculoId == VehiculoId).ToList();

            //ViewBag.FK_Vehiculo = new SelectList(db.Vehiculo.Where(a => a.Clase == Clase && a.Estatus == "Disponible"), "VehiculoId", "Marca");
            return Json(vehiculos, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFk_Combustible(int VehiculoId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Vehiculo> vehiculos = db.Vehiculo.Where(m => m.VehiculoId == VehiculoId).ToList();

            //ViewBag.FK_Vehiculo = new SelectList(db.Vehiculo.Where(a => a.Clase == Clase && a.Estatus == "Disponible"), "VehiculoId", "Marca");
            return Json(vehiculos, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetFk_Costodia(int VehiculoId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Vehiculo> vehiculos = db.Vehiculo.Where(m => m.VehiculoId == VehiculoId).ToList();

            //ViewBag.FK_Vehiculo = new SelectList(db.Vehiculo.Where(a => a.Clase == Clase && a.Estatus == "Disponible"), "VehiculoId", "Marca");
            return Json(vehiculos, JsonRequestBehavior.AllowGet);
        }
        // POST: contratoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ContratoId,Nombre,Apellido,Pais,Ciudad,Direccion,Telefono,Email,Licencia,FK_Vehiculo,idClase,Combustible_Salida,Combustible_Entrada,Kilometraje_Salida,Kilometraje_Entrada,costo_dia,Cantidad_Dias,costo_diasextras,Dias_Extras,Descuento_Comision,FormaPago,Contrato1,Fecha_Inicio,Fecha_Cierre,Condiciones,Tipo_Renta,Estatus,Referido,subtotal,total,recargo")] contrato contrato)
        {
            if (contrato.Tipo_Renta == "Contrato")
            {
                int x = Convert.ToInt32(contrato.FK_Vehiculo);
                var q = (from a in db.Vehiculo where a.VehiculoId == x select a).First();
                q.Estatus = "Rentado";
                db.SaveChanges();
            }

            if (ModelState.IsValid)
            {
                db.contrato.Add(contrato);
                contrato.Estatus = "Abierto";
                contrato.recargo = 0;
                contrato.Dias_Extras = 0;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idClase = new SelectList(db.Clasevehiculo, "id", "Clase", contrato.idClase);
            ViewBag.FK_Vehiculo = new SelectList(db.Vehiculo.Where(a => a.Estatus == "Disponible"), "VehiculoId", "Marca", contrato.FK_Vehiculo);
            return View(contrato);
        }


        // GET: contratoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            contrato contrato = db.contrato.Find(id);
            if (contrato == null)
            {
                return HttpNotFound();
            }
            ViewBag.idClase = new SelectList(db.Clasevehiculo, "id", "Clase", contrato.idClase);
            ViewBag.FK_Vehiculo = new SelectList(db.Vehiculo, "VehiculoId", "Marca", contrato.FK_Vehiculo);
            return View(contrato);
        }

        // POST: contratoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ContratoId, Nombre, Apellido, Pais, Ciudad, Direccion, Telefono, Email, Licencia, FK_Vehiculo, idClase, Combustible_Salida, Combustible_Entrada, Kilometraje_Salida, Kilometraje_Entrada, costo_dia, Cantidad_Dias, costo_diasextras, Dias_Extras, Descuento_Comision, FormaPago, Contrato1, Fecha_Inicio, Fecha_Cierre, Condiciones, Tipo_Renta, Estatus, Referido, subtotal, Total, recargo")] contrato contrato)
        {
            if (contrato.Tipo_Renta == "Contrato")
            {
                int x = Convert.ToInt32(contrato.FK_Vehiculo);
                var q = (from a in db.Vehiculo where a.VehiculoId == x select a).First();
                q.Estatus = "Rentado";
                db.SaveChanges();
            }

            if (ModelState.IsValid)
            {
                db.Entry(contrato).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idClase = new SelectList(db.Clasevehiculo, "id", "Clase", contrato.idClase);
            ViewBag.FK_Vehiculo = new SelectList(db.Vehiculo, "VehiculoId", "Marca", contrato.FK_Vehiculo);
            return View(contrato);
        }
        public ActionResult CerrarContrato(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            contrato contrato = db.contrato.Find(id);
            if (contrato == null)
            {
                return HttpNotFound();
            }
            ViewBag.idClase = new SelectList(db.Clasevehiculo, "id", "Clase", contrato.idClase);
            ViewBag.FK_Vehiculo = new SelectList(db.Vehiculo, "VehiculoId", "Marca", contrato.FK_Vehiculo);
            return View(contrato);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CerrarContrato([Bind(Include = "ContratoId, Nombre, Apellido, Pais, Ciudad, Direccion, Telefono, Email, Licencia, FK_Vehiculo, idClase, Combustible_Salida, Combustible_Entrada, Kilometraje_Salida, Kilometraje_Entrada, costo_dia, Cantidad_Dias, costo_diasextras, Dias_Extras, Descuento_Comision, Total, FormaPago, Contrato1, Fecha_Inicio, Fecha_Cierre, Condiciones, Tipo_Renta, Estatus, Referido, subtotal, Total, recargo")] contrato contrato)
        {
            int m = Convert.ToInt32(contrato.FK_Vehiculo);
            var id = (from s in db.Vehiculo where s.VehiculoId == m select s).First();


            if (ModelState.IsValid)
            {
                db.Entry(contrato).State = EntityState.Modified;
                contrato.Estatus = "Cerrado";
                id.Kilometraje = Convert.ToInt32(contrato.Kilometraje_Entrada);
                id.Combustible = contrato.Combustible_Entrada;
                id.Estatus = "Disponible";
                db.SaveChanges();
                db.cerrarcontratos();
                return RedirectToAction("index");
            }
            ViewBag.idClase = new SelectList(db.Clasevehiculo, "id", "Clase", contrato.idClase);
            ViewBag.FK_Vehiculo = new SelectList(db.Vehiculo, "VehiculoId", "Marca", contrato.FK_Vehiculo);
            return View(contrato);
        }
        // GET: contratoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            contrato contrato = db.contrato.Find(id);
            if (contrato == null)
            {
                return HttpNotFound();
            }
            return View(contrato);
        }

        // POST: contratoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            contrato contrato = db.contrato.Find(id);
            int m = Convert.ToInt32(contrato.FK_Vehiculo);
            var vehiculo = (from s in db.Vehiculo where s.VehiculoId == m select s).First();
            db.contrato.Remove(contrato);
            vehiculo.Estatus = "Disponible";
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
