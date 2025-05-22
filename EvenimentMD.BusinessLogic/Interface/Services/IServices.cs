using EvenimentMD.Domain.Models.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvenimentMD.BusinessLogic.Interface.Services
{
    public interface IServices
    {
        ProviderDbTable GetProviderById(int id);
        List<ProviderMediaModel> GetProviderImages(int providerId);

        List<ProviderDbTable> GetAllProviders();
        Dictionary<int, string> GetProviderFirstImages(List<ProviderDbTable> providers);

        List<ProviderServicesData> GetProviderServices(int providerId);
    }
}
