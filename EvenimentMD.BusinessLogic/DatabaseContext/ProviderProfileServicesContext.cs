using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EvenimentMD.Domain.Models.Provider;

namespace EvenimentMD.BusinessLogic.DatabaseContext
{
    public class ProviderProfileServicesContext : DbContext
    {
        public ProviderProfileServicesContext() : base("name=EVENIMENTMD")
        {

        }

        public virtual DbSet<ProviderServicesDbTable> Services { get; set; }
    }
}
