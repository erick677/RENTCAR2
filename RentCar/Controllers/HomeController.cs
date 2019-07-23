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
        private rentcar4Entities2 db = new rentcar4Entities2();
        contrato contrato = new contrato();
        public ActionResult Index()
        {
            /*-------------START ALERT----------------------*/
            var t = (from s in db.contrato.Where(m => m.Fecha_Cierre <= DateTime.Today && m.Estatus == "Abierto") select s.ContratoId).Count();
            contrato.recargo = 0;
            contrato.Dias_Extras = 0;
            if (t != 0)
            {
               
                db.calculardiasextras();
                ViewBag.alert = "Tienes asuntos pendientes".ToString();
            }
            /*--------------------END ALERT----------------------*/
            var mes = DateTime.Now.ToString("MM");
            int fecha = Convert.ToInt32(mes);

            decimal contratosNormales = 0;
            decimal contratosCerrados = 0;


            contratosNormales = Convert.ToDecimal((from ord in db.contrato.Where(a => a.Fecha_Inicio.Value.Month == fecha || a.Fecha_Cierre.Value.Month == fecha )
                                       select ord.Total).Sum());

            contratosCerrados = Convert.ToDecimal((from ord in db.contratohistory.Where(a => a.Fecha_Inicio.Value.Month == fecha || a.Fecha_Cierre.Value.Month == fecha )
                                                   select ord.Total).Sum());

            var Total = contratosNormales + contratosCerrados;
            

            Double doubl = Math.Round((Double)Total, 2);
            ViewBag.TotalGanancias1 = doubl;

            /*------------------------------START Contadores Contratos y reservas para hoy---------------------------*/

            ViewBag.ReservasDeHoy = (from ord in db.contrato.Where(a => (a.Tipo_Renta == "Reserva" && a.Fecha_Cierre == DateTime.Today && a.Estatus == "Abierto")  )
                                select ord.ContratoId).Count();

            ViewBag.ContratosDeHoy = (from ord in db.contrato.Where(a => (a.Estatus == "Abierto" && a.Tipo_Renta == "Contrato" && a.Fecha_Cierre == DateTime.Today) )
                                select ord.ContratoId).Count();

            ViewBag.ReservasPendientes = (from ord in db.contrato.Where(a => a.Tipo_Renta == "Reserva" && a.Fecha_Cierre <= DateTime.Today && a.Estatus == "Abierto")
                                     select ord.ContratoId).Count();

            ViewBag.ContratosPendientes = (from ord in db.contrato.Where(a => a.Tipo_Renta == "Contrato" && a.Fecha_Cierre <= DateTime.Today && a.Estatus == "Abierto")
                                      select ord.ContratoId).Count();

            ViewBag.NFC = (from ord in db.NFC.Where(a => (a.Estatus == "Disponible") )
                                select ord.idNFC).Count();

            /*------------------------------END Contadores Contratos y reservas para hoy---------------------------*/

            /*------------------------------START Contadores de vehiculos---------------------------*/

            ViewBag.Disponibles = (from ord in db.Vehiculo.Where(a => (a.Estatus == "Disponible") )
                                select ord.VehiculoId).Count();

            ViewBag.Rentados = (from ord in db.Vehiculo.Where(a => (a.Estatus == "Rentado") )
                                select ord.VehiculoId).Count();

            ViewBag.Mantenimiento = (from ord in db.Vehiculo.Where(a => (a.Estatus == "Mantenimiento") )
                                select ord.VehiculoId).Count();

            /*------------------------------END Contadores de vehiculos---------------------------*/
            
            return View();
        }
        

    }
}