using Adobe.PDFServicesSDK.auth;
using Adobe.PDFServicesSDK.io;
using Adobe.PDFServicesSDK.options.extractpdf;
using Adobe.PDFServicesSDK.pdfops;
using ExecutionContext = Adobe.PDFServicesSDK.ExecutionContext;
using MediatR;

namespace DataLibrary.Mediator;
public class ExtractPdf
{
    public record Command(
        string ExtractFromFilePath,
        string ExtractToFilePath) : IRequest<Unit>;
    public class Handler : IRequestHandler<Command, Unit>
    {
        public Handler()
        {
            
        }
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            Credentials credentials = Credentials.ServiceAccountCredentialsBuilder()
            .FromFile(Directory.GetCurrentDirectory() + "pdfservices-api-credentials.json")
            .Build();

            ExecutionContext executionContext = ExecutionContext.Create(credentials);
            ExtractPDFOperation extractPdfOperation = ExtractPDFOperation.CreateNew();

            FileRef sourceFileRef = FileRef.CreateFromLocalFile(request.ExtractFromFilePath);
            extractPdfOperation.SetInputFile(sourceFileRef);

            ExtractPDFOptions extractPdfOptions = ExtractPDFOptions.ExtractPDFOptionsBuilder()
                .AddCharsInfo(false)
                .AddGetStylingInfo(false)
                .AddElementsToExtract(new List<ExtractElementType>(new[] { ExtractElementType.TABLES }))
                .AddTableStructureFormat(TableStructureType.CSV)
                .Build();
            extractPdfOperation.SetOptions(extractPdfOptions);
            FileRef result = extractPdfOperation.Execute(executionContext);

            Guid guid = Guid.NewGuid();
            var guidString = guid.ToString();
            result.SaveAs(request.ExtractToFilePath + $"{guidString}.zip");

            return Unit.Value;
        }
    }
}
