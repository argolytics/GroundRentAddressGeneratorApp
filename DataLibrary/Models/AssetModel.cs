using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataLibrary.Models;
public class AssetModel
{
    [JsonPropertyName("metadata")]
    public MetadataModel Metadata { get; set; }
    [JsonPropertyName("downloadUri")]
    public string DownloadUri { get; set; }
    [JsonPropertyName("assetID")]
    public string AssetID { get; set; }
}
