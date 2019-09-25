using System;
using Newtonsoft.Json;

namespace voice2midi.Models
{
    public class FileUploadModel
    {
        [JsonProperty(PropertyName = "fileId")]
        public long FileId { get; set; }

        [JsonProperty(PropertyName = "size")]
        public long Size { get; set; }

        [JsonProperty(PropertyName = "filePath")]
        public string FilePath { get; set; }
    }
}
