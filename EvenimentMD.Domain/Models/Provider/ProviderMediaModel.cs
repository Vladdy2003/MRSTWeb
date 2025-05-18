using EvenimentMD.Domain.Enums;

namespace EvenimentMD.Domain.Models.Provider
{
    public class ProviderMediaModel
    {
        public int Id { get; set; }
        public int providerId { get; set; }
        public MediaType mediaType { get; set; }
        public string filePath { get; set; }
        public string addedAt { get; set; }
    }
}
