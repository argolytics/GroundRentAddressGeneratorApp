using DataLibrary.Helpers;
using DataLibrary.Models;

namespace DataLibrary.HttpClients;

public class GetDownloadStatus
{
    private readonly IHttpClientFactory _factory;
    private readonly AccessTokenInformation _accessToken;
    private readonly GetAccessToken _getAccessToken;
    public const string ClientName = "getDownloadStatus";
    public GetDownloadStatus(IHttpClientFactory _factory, AccessTokenInformation accessToken, GetAccessToken getAccessToken)
    {
        this._factory = _factory;
        this._accessToken = accessToken;
        this._getAccessToken = getAccessToken;
    }
    public async Task<HttpResponseMessage> GetStatus(FileModel file)
    {
        if (String.IsNullOrEmpty(this._accessToken.AccessToken))
        {
            var res = await this._getAccessToken.GetToken();
            var token = System.Text.Json.JsonSerializer.Deserialize<AccessTokenInformation>(await res.Content.ReadAsStringAsync());
            this._accessToken.AccessToken = token.AccessToken;
        }
        HttpResponseMessage response = await GetStatusInternal(file);
        if (response.StatusCode == System.Net.HttpStatusCode.Forbidden || response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            var res = await this._getAccessToken.GetToken();
            var token = System.Text.Json.JsonSerializer.Deserialize<AccessTokenInformation>(await res.Content.ReadAsStringAsync());
            this._accessToken.AccessToken = token.AccessToken;
            response = await GetStatusInternal(file);
        }
        return response;
    }

    private async Task<HttpResponseMessage> GetStatusInternal(FileModel file)
    {
        var _httpClient = _factory.CreateClient(ClientName);

        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", this._accessToken.AccessToken);
        var response = await _httpClient.GetAsync(file.DownloadCheckLocation);
        return response;
    }
}