using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EvenimentMD.Domain.Models.User;

namespace EvenimentMD.BusinessLogic.DatabaseContext
{
    public class UserContext : DbContext
    {
        public UserContext() : base("name=EVENIMENTMD")
        {

        }

        public virtual DbSet<UDbTable> Users { get; set; }
    }
}
