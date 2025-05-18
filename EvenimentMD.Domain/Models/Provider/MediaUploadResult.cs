using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvenimentMD.Domain.Models.Provider
{
    public class MediaUploadResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int ImagesProcessed { get; set; }
        public int VideosProcessed { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
