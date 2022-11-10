namespace DataLibrary.HttpClients;
public class UploadPdf
{
    private readonly IHttpClientFactory _factory;
    public const string ClientName = "uploadPdf";
    public UploadPdf(IHttpClientFactory _factory)
    {
        this._factory = _factory;
    }
    public async Task<HttpResponseMessage> Upload(string uploadUri, string uploadPath)
    {
        var _httpClient = _factory.CreateClient(ClientName);
        FileStream fileStream = File.Create(uploadPath);
        var size = fileStream.Length;
        HttpContent content = new StreamContent(fileStream);

        return await _httpClient.PutAsync(uploadUri, content);
    }
}
