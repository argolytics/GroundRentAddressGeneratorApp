using DataLibrary.Models;
using System.Net.Http.Json;

namespace DataLibrary.HttpClients;
public class ExtractPdf
{
    private readonly IHttpClientFactory _factory;
    public const string ClientName = "extractPdf";
    public ExtractPdf(IHttpClientFactory _factory)
    {
        this._factory = _factory;
    }
    public async Task<HttpResponseMessage> Extract(FileModel file)
    {
        var _httpClient = _factory.CreateClient(ClientName);
        ExtractPdfModel extractPdfBody = new()
        {
            AssetId = file.AssetId,
            RenditionsToExtract = new string[] { "tables" },
            ElementsToExtract = new string[] { "text", "tables" },
            TableOutputFormat = "csv",
        };
        return await _httpClient.PostAsJsonAsync("https://pdf-services.adobe.io/operation/extractpdf", extractPdfBody);
    }
}
