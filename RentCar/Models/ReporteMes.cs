using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentCar.Models
{
    public class ReporteMes
    {
        public IEnumerable<contrato> contratos { get; set; }
        public IEnumerable<contratohistory> contratosCerrados { get; set; }
    }
}