using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EvenimentMD.BusinessLogic.BLStruct;
using EvenimentMD.BusinessLogic.Interface;

namespace EvenimentMD.BusinessLogic
{
    public class BusinessLogic
    {
        public ISignUp GetSignUpBL()
        {
            return new SignUpBL();
        }
    }
}
