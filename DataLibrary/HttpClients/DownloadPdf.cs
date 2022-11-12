using DataLibrary.Helpers;
using DataLibrary.Models;

namespace DataLibrary.HttpClients;

public class DownloadPdf
{
    private readonly IHttpClientFactory _factory;
    private readonly AccessTokenInformation _accessToken;
    private readonly GetAccessToken _getAccessToken;
    public const string ClientName = "downloadPdf";
    public DownloadPdf(IHttpClientFactory _factory, AccessTokenInformation accessToken, GetAccessToken getAccessToken)
    {
        this._factory = _factory;
        this._accessToken = accessToken;
        this._getAccessToken = getAccessToken;
    }
    public async Task<HttpResponseMessage> Download(FileModel file)
    {
        if (String.IsNullOrEmpty(this._accessToken.AccessToken))
        {
            var res = await this._getAccessToken.GetToken();
            var token = System.Text.Json.JsonSerializer.Deserialize<AccessTokenInformation>(await res.Content.ReadAsStringAsync());
            this._accessToken.AccessToken = token.AccessToken;
        }
        HttpResponseMessage response = await DownloadInternal(file);
        
        if(response.StatusCode == System.Net.HttpStatusCode.Forbidden || response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {

            var res = await this._getAccessToken.GetToken();
            var token = System.Text.Json.JsonSerializer.Deserialize<AccessTokenInformation>(await res.Content.ReadAsStringAsync());
            this._accessToken.AccessToken = token.AccessToken;

            response = await DownloadInternal(file);
        }
        return response;
    }

    private async Task<HttpResponseMessage> DownloadInternal(FileModel file)
    {
        var _httpClient = _factory.CreateClient(ClientName);

        var response = await _httpClient.GetAsync(file.DownloadUri);
        return response;
    }
}
