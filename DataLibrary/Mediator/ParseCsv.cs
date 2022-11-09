using CsvHelper;
using DataLibrary.DbAccess;
using DataLibrary.DbServices;
using DataLibrary.Models;
using MediatR;
using System.Globalization;

namespace DataLibrary.Mediator;
public class ParseCsv
{
    public record Command(string FilePath) : IRequest<Unit>;
    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IDataContext _dataContext;
        private readonly IAddressDataService _addressDataService;
        public Handler(IDataContext dataContext, IAddressDataService addressDataService)
        {
            _dataContext = dataContext;
            _addressDataService = addressDataService;
        }
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            using (var streamReader = new StreamReader(request.FilePath))
            {
                using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                {
                    csvReader.Context.RegisterClassMap<AddressClassMap>();
                    while (csvReader.Read())
                    {
                        using (var uow = _dataContext.CreateUnitOfWork())
                        {
                            var record = csvReader.GetRecord<AddressModel>();
                            await _addressDataService.CreateAddress(record);
                        }
                    }
                }
            }
            return Unit.Value;
        }
    }
}
