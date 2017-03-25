using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Flierefluiter.Domain.Entities;

namespace Flierefluiter.Domain.Concrete
{
    public class EFDbContext : DbContext { 
        public DbSet<Reservering> Reserverings { get; set; }
        public DbSet<Veld> Velds { get; set; }
        public DbSet<Plaats> Plaatss { get; set; }
        public DbSet<Boeking> Boekings { get; set; }

    }
}
