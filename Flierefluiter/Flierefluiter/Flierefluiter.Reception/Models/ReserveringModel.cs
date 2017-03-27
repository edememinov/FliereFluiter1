using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flierefluiter.Domain.Entities;
using Flierefluiter.Domain.Abstract;
using Flierefluiter.Domain.Concrete;

namespace Flierefluiter.Reception.Models
{
    public class ReserveringModel
    {
        public Reservering Reservering { get; set; }
        public IEnumerable<Plaats> Plaatsen { get; set; }
        public IEnumerable<Veld> Velden { get; set; }
        public IEnumerable<Reservering> Reserveringen {get; set;}
        public Veld GekozenVeld { get; set; }
        public Plaats GekozenPlaats { get; set; }
        public Boeking Boeking { get; set; }
        public IEnumerable<Boeking> Boekingen { get; set; }
    }
}