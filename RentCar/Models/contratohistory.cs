//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RentCar.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class contratohistory
    {
        public int Historialid { get; set; }
        public string ContratoId { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Pais { get; set; }
        public string Ciudad { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Licencia { get; set; }
        public string FK_Vehiculo { get; set; }
        public string idClase { get; set; }
        public string Combustible_Salida { get; set; }
        public string Combustible_Entrada { get; set; }
        public string Kilometraje_Salida { get; set; }
        public string Kilometraje_Entrada { get; set; }
        public string costo_dia { get; set; }
        public string Cantidad_Dias { get; set; }
        public string costo_diasextras { get; set; }
        public string Dias_Extras { get; set; }
        public string recargo { get; set; }
        public string Descuento_Comision { get; set; }
        public string subtotal { get; set; }
        public Nullable<decimal> Total { get; set; }
        public string FormaPago { get; set; }
        public string Contrato { get; set; }
        public Nullable<System.DateTime> Fecha_Inicio { get; set; }
        public Nullable<System.DateTime> Fecha_Cierre { get; set; }
        public string Condiciones { get; set; }
        public string Tipo_Renta { get; set; }
        public string Modelo { get; set; }
        public string Referido { get; set; }
    }
}
