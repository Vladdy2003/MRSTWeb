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
        admin = 1,
        serviceProvider = 100,
        serviceProviderRecomanded = 101,
        organizer = 200,
    }
}
