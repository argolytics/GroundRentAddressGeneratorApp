using DataLibrary.Helpers;
using DataLibrary.Models;
using System.Net.Http.Json;

namespace DataLibrary.HttpClients;
public class GetUploadUri
{
    private readonly IHttpClientFactory _factory;
    private readonly AccessTokenInformation _accessToken;
    private readonly GetAccessToken _getAccessToken;
    public const string ClientName = "getUploadUri";
    public GetUploadUri(IHttpClientFactory _factory, AccessTokenInformation accessToken, GetAccessToken getAccessToken)
    {
        this._factory = _factory;
        this._accessToken = accessToken;
        this._getAccessToken = getAccessToken;
    }
    public async Task<HttpResponseMessage> Upload()
    {
        if (String.IsNullOrEmpty(this._accessToken.AccessToken))
        {
            var res = await this._getAccessToken.GetToken();
            var token = System.Text.Json.JsonSerializer.Deserialize<AccessTokenInformation>(await res.Content.ReadAsStringAsync());
            this._accessToken.AccessToken = token.AccessToken;
        }
        HttpResponseMessage response = await UploadInternal();
        if (response.StatusCode == System.Net.HttpStatusCode.Forbidden || response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            var res = await this._getAccessToken.GetToken();
            var token = System.Text.Json.JsonSerializer.Deserialize<AccessTokenInformation>(await res.Content.ReadAsStringAsync());
            this._accessToken.AccessToken = token.AccessToken;
            response = await UploadInternal();
        }
        return response;
    }

    private async Task<HttpResponseMessage> UploadInternal()
    {
        var _httpClient = _factory.CreateClient(ClientName);
        MediaTypeModel mediaType = new()
        {
            mediaType = "application/pdf"
        };
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", this._accessToken.AccessToken);
        var response = await _httpClient.PostAsJsonAsync("https://pdf-services.adobe.io/assets", mediaType);
        return response;
    }
}
