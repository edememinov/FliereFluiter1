using Flierefluiter.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flierefluiter.Reception.Models
{
    public class BoekingViewModel
    {
        public Boeking Boeking { get; set; }
        public virtual IEnumerable<Boeking> Boekings { get; set; }
        public IEnumerable<Reservering> Resveringens { get; set; }
        public Reservering Reserv { get; set; }
        public IEnumerable<Veld> Velden { get; set;}
        public IEnumerable<Plaats> Plaatsen { get; set; }
    }
}