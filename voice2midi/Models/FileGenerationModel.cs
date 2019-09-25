﻿using System;
using Newtonsoft.Json;

namespace voice2midi.Models
{
    public class FileGenerationModel
    {
        [JsonProperty(PropertyName = "fileOutId")]
        public long FileOutId { get; set; }

        [JsonProperty(PropertyName = "filePathIn")]
        public long FilePathIn { get; set; }

        [JsonProperty(PropertyName = "filePathOut")]
        public long FilePathOut { get; set; }
    }
}
