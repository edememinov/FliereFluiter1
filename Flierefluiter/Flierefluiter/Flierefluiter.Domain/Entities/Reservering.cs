using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Flierefluiter.Domain.Entities
{
    public class Reservering
    {

        [Key]
        public int ReserveringID { get; set; }
        public String Naam { get; set; }
        public String Email { get; set; }
        public int? Telnr { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? BeginDatum { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EindDatum { get; set; }

        public virtual Plaats Plaats {get;set;}
        public int? PlaatsId { get; set; }
        public virtual Veld Veld { get; set; }
        public int? VeldID { get; set; }
        public int AantalPersonen { get; set; }

    }
}
