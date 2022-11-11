using DataLibrary.Models;

namespace DataLibrary.HttpClients;
public class UploadPdf
{
    private readonly IHttpClientFactory _factory;
    public const string ClientName = "uploadPdf";
    public UploadPdf(IHttpClientFactory _factory)
    {
        this._factory = _factory;
    }
    public async Task<HttpResponseMessage> Upload(FileModel file)
    {
        var _httpClient = _factory.CreateClient(ClientName);
        byte[] fileData = File.ReadAllBytes(file.UploadPath);
        var stream = new MemoryStream(fileData);
        HttpContent content = new StreamContent(stream);

        return await _httpClient.PutAsync(file.UploadUri, content);
    }
}
