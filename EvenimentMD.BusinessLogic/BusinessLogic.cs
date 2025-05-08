using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EvenimentMD.BusinessLogic.BLStruct;
using EvenimentMD.BusinessLogic.Interface;
using EvenimentMD.BusinessLogic.Interface.Provider;

namespace EvenimentMD.BusinessLogic
{
    public class BusinessLogic
    {
        public ISignUp GetSignUpBL()
        {
            return new SignUpBL();
        }

        public ISession GetLogInBL()
        {
            return new SessionBL();
        }
    }
}
