using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvenimentMD.Domain.Enums
{
    public enum URole
    {
        none = 0,
        banned = 1,
        fraudletUser = 2,
        serviceProvider = 100,
        organizer = 200,
        administrator = 300,

    }
}
