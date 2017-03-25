using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Flierefluiter.Domain.Entities
{
    public class Veld
    {
        
        [HiddenInput(DisplayValue = false)]
        [Key]
        public int? VeldID { get; set; }
        public String Naam { get; set; }
        public String TypePlaats { get; set; }
        public int? Oppervlak { get; set; }
        public String Amp { get; set; }
        public Boolean Wifi { get; set; }
        public Boolean Water { get; set; }
        public Boolean Riool { get; set; }
        public Boolean CAI { get; set; }
        public int? PrijsPerDag { get; set; }

    }

}
