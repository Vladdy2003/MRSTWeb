using System.Collections.Generic;
using EvenimentMD.BusinessLogic.Core;
using EvenimentMD.BusinessLogic.Interface.Services;
using EvenimentMD.Domain.Models.Provider;

namespace EvenimentMD.BusinessLogic.BLStruct
{
    public class ServicesBL : UserAPI, IServices
    {
        public List<ProviderDbTable> GetAllProviders()
        {
            return GetAllProvidersLogic();
        }

        public Dictionary<int, string> GetProviderFirstImages(List<ProviderDbTable> providers)
        {
            return GetProviderFirstImagesLogic(providers);
        }

        public ProviderDbTable GetProviderById(int id)
        {
            return GetProviderByIdLogic(id);
        }

        public List<ProviderMediaModel> GetProviderImages(int providerId)
        {
            return GetProviderImagesLogic(providerId);
        }
    }
}
