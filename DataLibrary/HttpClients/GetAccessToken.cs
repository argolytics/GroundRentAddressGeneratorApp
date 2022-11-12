using DataLibrary.Helpers;
using DataLibrary.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.HttpClients;
public class GetAccessToken
{
    private readonly IHttpClientFactory _factory;
    private readonly PDFServicesSettings _options;
    public const string ClientName = "uploadPdf";
    public GetAccessToken(IHttpClientFactory _factory, AccessTokenInformation accessToken, IOptions<PDFServicesSettings> options)
    {
        this._factory = _factory;
        this._options = options.Value;
    }
    public async Task<HttpResponseMessage> GetToken()
    {
        var client = _factory.CreateClient("jwt");
        var p = new Dictionary<string, string>();
        p.Add("jwt_token", this._options.JWT);
        p.Add("client_id", this._options.ClientId);
        p.Add("client_secret", this._options.ClientSecret);
        var res = await client.PostAsync("https://ims-na1.adobelogin.com/ims/exchange/jwt", new FormUrlEncodedContent(p));
        return res;
    }

}
