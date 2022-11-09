using System.IO.Compression;
using MediatR;

namespace DataLibrary.Mediator;
public class UnzipFile
{
    public record Command(
        string ExtractFromFilePath,
        string ExtractToFilePath,
        string JsonFilePath) : IRequest<Unit>;
    public class Handler : IRequestHandler<Command, Unit>
    {
        public Handler()
        {
        }
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            string jsonPath =
                @"C:\Users\Jason\source\repos\GroundRentAddressGeneratorApp\GroundRentAddressGenerator\wwwroot\Data\";

            ZipFile.ExtractToDirectory(request.ExtractFromFilePath, request.ExtractToFilePath);
            if (request.ExtractFromFilePath != null)
            {
                File.Delete(request.ExtractFromFilePath);
            }
            if (request.JsonFilePath != null)
            {
                File.Delete(request.JsonFilePath);
            }
            return Unit.Value;
        }
    }
}
