using Adobe.PDFServicesSDK.auth;
using Adobe.PDFServicesSDK;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Adobe.PDFServicesSDK.pdfops;
using Adobe.PDFServicesSDK.io;
using Adobe.PDFServicesSDK.options.extractpdf;

namespace GroundRentAddressGenerator.Pages
{
    [IgnoreAntiforgeryToken(Order = 1001)]
    public class FileExtractTestModel : PageModel
    {
        public string Message;

        public void OnGet()
        {
        }

        public void OnPost()
        {

            try
            {
                Credentials credentials = Credentials.ServiceAccountCredentialsBuilder()
                    .FromFile(Directory.GetCurrentDirectory() + @"\wwwroot\" + "pdfservices-api-credentials.json")
                    .Build();

                Adobe.PDFServicesSDK.ExecutionContext executionContext = Adobe.PDFServicesSDK.ExecutionContext.Create(credentials);
                    ExtractPDFOperation extractPdfOperation = ExtractPDFOperation.CreateNew();

                var path = @"wwwroot\Data\extractPDFInput.pdf";
                FileRef sourceFileRef = FileRef.CreateFromLocalFile(path);
                extractPdfOperation.SetInputFile(sourceFileRef);

                ExtractPDFOptions extractPdfOptions = ExtractPDFOptions.ExtractPDFOptionsBuilder()
                    .AddCharsInfo(false)
                    .AddGetStylingInfo(false)
                    .AddElementsToExtract(new List<ExtractElementType>(new[] { ExtractElementType.TABLES }))
                    .AddTableStructureFormat(TableStructureType.CSV)
                    .Build();

                extractPdfOperation.SetOptions(extractPdfOptions);
                
                FileRef result = extractPdfOperation.Execute(executionContext);

                var extPath = path.Replace("extract", "result") + Guid.NewGuid().ToString() + ".zip";
                result.SaveAs(extPath);
                Message = $"Output path = {extPath}";

            }
            catch(Exception e)
            {
                Message = e.ToString();
                Console.WriteLine(e);
            }
        }
    }
}
