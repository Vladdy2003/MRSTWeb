using EvenimentMD.BusinessLogic.Core;
using EvenimentMD.BusinessLogic.Interface.Provider;
using EvenimentMD.Domain.Enums;
using EvenimentMD.Domain.Models.Provider;
using System.Collections.Generic;
using System.Web;
using System.Threading.Tasks;

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

        public int GetBusinessProfileId(int userId)
        {
            return GetBusinessProfileIdLogic(userId);
        }

        public int BusinessProfileMediaCount(int profileId, MediaType mediaType)
        {
            return BusinessProfileMediaCountLogic(profileId, mediaType);
        }

        public Task<MediaUploadResult> ProcessAndSaveMedia(List<HttpPostedFileBase> images, List<HttpPostedFileBase> videos, int userId)
        {
            return ProcessAndSaveMediaLogic(images, videos, userId);
        }

        public bool DeleteMedia(int mediaId, int userId)
        {
            return DeleteMediaLogic(mediaId, userId);
        }
    }
}