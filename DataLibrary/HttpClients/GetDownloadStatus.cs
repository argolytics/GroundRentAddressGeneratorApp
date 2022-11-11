using DataLibrary.Models;

namespace DataLibrary.HttpClients;

public class GetDownloadStatus
{
    private readonly IHttpClientFactory _factory;
    public const string ClientName = "getDownloadStatus";
    public GetDownloadStatus(IHttpClientFactory _factory)
    {
        this._factory = _factory;
    }
    public async Task<HttpResponseMessage> GetStatus(FileModel file)
    {
        var _httpClient = _factory.CreateClient(ClientName);

        return await _httpClient.GetAsync($"https://pdf-services.adobe.io/operation/extractpdf/{file.AssetId}/status");
    }
}