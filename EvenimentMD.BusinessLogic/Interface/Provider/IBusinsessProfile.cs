using EvenimentMD.Domain.Models.Provider;

namespace EvenimentMD.BusinessLogic.Interface.Provider
{
    public interface IBusinsessProfile
    {
        bool EditBusinessProfile(BusinessProfileData data, int userId);

        BusinessProfileData GetBusinessProfile(int userId);
    }
}
