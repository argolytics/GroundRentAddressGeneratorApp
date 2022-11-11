using DataLibrary.Models;

namespace DataLibrary.HttpClients;

public class DownloadPdf
{
    private readonly IHttpClientFactory _factory;
    public const string ClientName = "downloadPdf";
    public DownloadPdf(IHttpClientFactory _factory)
    {
        this._factory = _factory;
    }
    public async Task<HttpResponseMessage> Download(FileModel file)
    {
        var _httpClient = _factory.CreateClient(ClientName);
        
        return await _httpClient.GetAsync(file.DownloadUri);
    }
}
