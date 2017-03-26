using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flierefluiter.Domain.Entities;
using Flierefluiter.Domain.Abstract;

namespace Flierefluiter.Domain.Concrete
{
    public class EFFlierefluiterRepository : IFlierefluiterRepository
    {
        private DefaultConnection context = new DefaultConnection();
        public IEnumerable<Reservering> Reserverings { 
            get { return context.Reserverings; } }

        public IEnumerable<Veld> Velds
        {
            get { return context.Velds; }
        }

        public IEnumerable<Plaats> Plaatss
        {
            get { return context.Plaatss; }
        }
        public IEnumerable<Boeking> Boekings
        {
            get { return context.Boekings; }
        }

    } 


}
