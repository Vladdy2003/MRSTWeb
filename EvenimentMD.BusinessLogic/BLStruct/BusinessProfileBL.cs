using EvenimentMD.BusinessLogic.Core;
using EvenimentMD.BusinessLogic.Interface.Provider;
using EvenimentMD.Domain.Models.Provider;

namespace EvenimentMD.BusinessLogic.BLStruct
{
    public class BusinessProfileBL : ProviderAPI, IBusinsessProfile
    {
        public bool EditBusinessProfile(BusinessProfileData data, int userId) 
        {
            return EditBusinessProfileLogic(data, userId);
        }

        public BusinessProfileData GetBusinessProfile(int userId)
        {
            return GetBusinessProfileLogic(userId);
        }
    }
}
