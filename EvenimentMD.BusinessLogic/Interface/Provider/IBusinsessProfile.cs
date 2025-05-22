using EvenimentMD.Domain.Enums;
using EvenimentMD.Domain.Models.Provider;
using System.Threading.Tasks;
using System.Web;
using System.Collections.Generic;

namespace EvenimentMD.BusinessLogic.Interface.Provider
{
    public interface IBusinsessProfile
    {
        bool EditBusinessProfile(BusinessProfileData data, int userId);

        BusinessProfileData GetBusinessProfile(int userId);

        int GetBusinessProfileId(int userId);

        int BusinessProfileMediaCount(int profileId, MediaType mediaType);

        Task<MediaUploadResult> ProcessAndSaveMedia(List<HttpPostedFileBase> images, List<HttpPostedFileBase> videos, int userId);

        bool DeleteMedia(int mediaId, int userId);

        List<ProviderMediaModel> GetProviderMediaList(int profileId);

        bool AddService(ProviderServicesData serviceData, int userId);

        List<ProviderServicesData> GetProviderServices(int profileId);

        bool UpdateService(ProviderServicesData serviceData, int userId);

        bool DeleteService(int serviceId, int userId);

    }
}