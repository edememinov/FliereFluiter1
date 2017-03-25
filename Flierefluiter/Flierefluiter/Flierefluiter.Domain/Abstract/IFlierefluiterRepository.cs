using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flierefluiter.Domain.Entities;

namespace Flierefluiter.Domain.Abstract
{
    public interface IFlierefluiterRepository
    {
        IEnumerable<Reservering> Reserverings { get; }
        IEnumerable<Veld> Velds { get; }
        IEnumerable<Plaats> Plaatss { get; }
        IEnumerable<Boeking> Boekings { get; }
    }
}
