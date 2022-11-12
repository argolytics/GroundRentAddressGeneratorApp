using DataLibrary.Helpers;
using DataLibrary.Models;
using System.Net.Http.Json;

namespace DataLibrary.HttpClients;
public class ExtractPdf
{
    private readonly IHttpClientFactory _factory;
    private readonly AccessTokenInformation _accessToken;
    private readonly GetAccessToken _getAccessToken;
    public const string ClientName = "extractPdf";
    public ExtractPdf(IHttpClientFactory _factory, AccessTokenInformation accessToken, GetAccessToken getAccessToken)
    {
        this._factory = _factory;
        this._accessToken = accessToken;
        this._getAccessToken = getAccessToken;
    }
    public async Task<HttpResponseMessage> Extract(FileModel file)
    {
        if (String.IsNullOrEmpty(this._accessToken.AccessToken))
        {
            var res = await this._getAccessToken.GetToken();
            var token = System.Text.Json.JsonSerializer.Deserialize<AccessTokenInformation>(await res.Content.ReadAsStringAsync());
            this._accessToken.AccessToken = token.AccessToken;
        }
        HttpResponseMessage response = await ExtractInternal(file);
        if (response.StatusCode == System.Net.HttpStatusCode.Forbidden || response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            var res = await this._getAccessToken.GetToken();
            var token = System.Text.Json.JsonSerializer.Deserialize<AccessTokenInformation>(await res.Content.ReadAsStringAsync());
            this._accessToken.AccessToken = token.AccessToken;
            response = await ExtractInternal(file);
        }
        return response;
    }

    private async Task<HttpResponseMessage> ExtractInternal(FileModel file)
    {
        var _httpClient = _factory.CreateClient(ClientName);
        ExtractPdfModel extractPdfBody = new()
        {
            AssetId = file.AssetId,
            RenditionsToExtract = new string[] { "tables" },
            ElementsToExtract = new string[] { "text", "tables" },
            TableOutputFormat = "csv",
        };
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", this._accessToken.AccessToken);
        var response = await _httpClient.PostAsJsonAsync("https://pdf-services.adobe.io/operation/extractpdf", extractPdfBody);
        return response;
    }
}
