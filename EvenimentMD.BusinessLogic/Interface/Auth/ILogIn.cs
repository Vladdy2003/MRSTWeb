using EvenimentMD.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvenimentMD.BusinessLogic.Interface
{
    public interface ILogIn
    {
        string UserLogInLogic(UserLogInData data);
    }
}
