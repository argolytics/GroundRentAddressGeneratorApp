using DataLibrary.Helpers;
using DataLibrary.Models;
using DataLibrary.Settings;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using RestSharp.Serialization.Json;

namespace DataLibrary.HttpClients;
public class UploadPdf
{
    private readonly IHttpClientFactory _factory;
    private readonly AccessTokenInformation _accessToken;
    private readonly GetAccessToken _getAccessToken;
    public const string ClientName = "uploadPdf";
    public UploadPdf(IHttpClientFactory _factory, GetAccessToken getAccessToken, AccessTokenInformation accessToken)
    {
        this._factory = _factory;
        this._accessToken = accessToken;
        this._getAccessToken = getAccessToken;
    }
    public async Task<HttpResponseMessage> Upload(FileModel file)
    {
        if (String.IsNullOrEmpty(this._accessToken.AccessToken))
        {
            var res = await this._getAccessToken.GetToken();
            var token = System.Text.Json.JsonSerializer.Deserialize<AccessTokenInformation>(await res.Content.ReadAsStringAsync());
            this._accessToken.AccessToken = token.AccessToken;
        }
        HttpResponseMessage response = await UploadInternal(file);
        if (response.StatusCode == System.Net.HttpStatusCode.Forbidden || response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            var res = await this._getAccessToken.GetToken();
            var token = System.Text.Json.JsonSerializer.Deserialize<AccessTokenInformation>(await res.Content.ReadAsStringAsync());
            this._accessToken.AccessToken = token.AccessToken;
            response = await UploadInternal(file);
        }

        return response;
    }

    private async Task<HttpResponseMessage> UploadInternal(FileModel file)
    {
        var _httpClient = _factory.CreateClient(ClientName);
        byte[] fileData = File.ReadAllBytes(file.UploadPath);
        HttpContent content = new ByteArrayContent(fileData);
        content.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/pdf");
        //_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", this._accessToken.AccessToken);
        var response = await _httpClient.PutAsync(file.UploadUri, content);
        return response;
    }
}
