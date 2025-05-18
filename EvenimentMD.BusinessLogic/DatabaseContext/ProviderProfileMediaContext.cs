using System.Data.Entity;
using EvenimentMD.Domain.Models.Provider;

namespace EvenimentMD.BusinessLogic.DatabaseContext
{
    public class ProviderProfileMediaContext : DbContext
    {
        public ProviderProfileMediaContext() : base("name=EVENIMENTMD")
        {

        }

        public virtual DbSet<ProviderMediaDb> Media { get; set; }
    }
}
