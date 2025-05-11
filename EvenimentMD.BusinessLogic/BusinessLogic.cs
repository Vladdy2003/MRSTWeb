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

        public ISession GetSessionBL()
        {
            return new SessionBL();
        }
        public IBusinsessProfile GetBusinessProfileBL()
        {
            return new BusinessProfileBL();
        }
    }
}
