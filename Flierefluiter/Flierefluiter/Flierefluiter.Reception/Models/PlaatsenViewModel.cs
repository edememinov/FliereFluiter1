using Flierefluiter.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flierefluiter.Reception.Models
{
    public class PlaatsenViewModel
    {
        public Plaats Plek {get; set; }
        public IEnumerable<Veld> PlaatsVeld { get; set;} 
    }
}