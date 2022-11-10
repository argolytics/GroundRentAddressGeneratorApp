using DataLibrary.Models;
using System.Text.Json;

namespace DataLibrary.HttpClients;
public class GetUploadUri
{
    private readonly IHttpClientFactory _factory;
    public const string ClientName = "getUploadUri";
    public GetUploadUri(IHttpClientFactory _factory)
    {
        this._factory = _factory;
    }
    public async Task<HttpResponseMessage> Upload() // This method gets Adobe's uploadUri and assetId
    {
        var _httpClient = _factory.CreateClient(ClientName);
        MediaTypeModel mediaType = new()
        {
            mediaType = "application/json"
        };
        var jsonBody = JsonSerializer.Serialize(mediaType);
        HttpContent content = new StringContent(jsonBody);

        return await _httpClient.PostAsync("https://pdf-services.adobe.io/assets", content);
    }
}
