using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Flierefluiter.Domain.Entities
{
    public class Boeking
    {
        [HiddenInput(DisplayValue = false)]
        [Key]
        public int? BoekingID { get; set; }
        public String Adres { get; set; }
        public String Postcode { get; set; }
        public String Woonplaats { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Geboortedatum { get; set; }
        public int? PaspoortID { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? BeginDatum { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EindDatum { get; set; }
        public virtual Reservering Reservering {get; set;}
        public virtual Plaats Plaats { get; set; }

    }
}
