using Microsoft.AspNetCore.Components.Forms;

namespace DataLibrary.Models;
public class FileModel
{
    public IBrowserFile? File { get; set; }
    public string? AssetId { get; set; }
    public string? FileId { get; } = Guid.NewGuid().ToString();
    public string? UploadPath { get; set; }
    public string? ExtractPath { get; set; }
    public string? UploadUri { get; set; }
}
