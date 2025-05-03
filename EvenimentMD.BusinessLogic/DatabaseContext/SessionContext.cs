using System.Data.Entity;
using EvenimentMD.Domain.Models.Session;

namespace EvenimentMD.BusinessLogic.DatabaseContext
{
    public class SessionContext : DbContext
    {
        public SessionContext() : base("name=EVENIMENTMD")
        {
            
        }

        public virtual DbSet<USessionDbTable> Sessions { get; set; }
    }
}
