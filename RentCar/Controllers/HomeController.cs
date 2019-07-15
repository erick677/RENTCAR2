using RentCar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentCar.Controllers
{
    public class HomeController : Controller
    {
        private rentcar4Entities db = new rentcar4Entities();
        contrato contrato = new contrato();
        public ActionResult Index()
        {
            var t = (from s in db.contrato.Where(m => m.Fecha_Cierre <= DateTime.Today && m.Estatus == "Abierto") select s.ContratoId).Count();
            contrato.recargo = 0;
            contrato.Dias_Extras = 0;
            if (t != 0)
            {
                db.calculardiasextras();
                ViewBag.alert = "Tienes asuntos pendientes".ToString();
            }
            return View();
        }

    }
}