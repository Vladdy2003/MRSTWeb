using EvenimentMD.BusinessLogic.Core;
using EvenimentMD.BusinessLogic.Interface;
using EvenimentMD.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvenimentMD.BusinessLogic.BLStruct
{
    public class LogInBL : UserAPI, ILogIn
    {
        public string UserLogInLogic(UserLogInData data)
        {
            return UserLogInLogic(data);
        }
    }
}
