using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Flierefluiter.Domain.Entities
{

    public class Plaats
    {
        [HiddenInput(DisplayValue = false)]
        [Key]
        public int? PlaatsID { get; set; }
        public String NaamPlaats { get; set; }
        public int? VeldID { get; set; }
        public Boolean Bezet { get; set; }
        public virtual Veld Veld { get; set; }
        
        public double PrijsStandaard { get; set; }
    }
}
