using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EvenimentMD.Domain.Models.Provider;
using EvenimentMD.Domain.Models.Session;

namespace EvenimentMD.BusinessLogic.DatabaseContext
{
    public class ProviderProfileContext : DbContext
    {
        public ProviderProfileContext() : base("name=EVENIMENTMD")
        {

        }

        public virtual DbSet<ProviderDbTable> Providers { get; set; }
    }
}

