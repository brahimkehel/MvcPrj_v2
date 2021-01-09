using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mini_Prj_.Models
{
    public class offre
    {
        public int id { get; set; }
        public DateTime? date_arriver { get; set; }
        public DateTime? date_depart { get; set; }
        public string arriver { get; set; }
        public string depart { get; set; }
        public string typeDeVehicule { get; set; }
        public double? tarif { get; set; }
    }
}