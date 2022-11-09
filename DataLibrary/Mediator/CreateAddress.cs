using DataLibrary.DbAccess;
using DataLibrary.DbServices;
using DataLibrary.Models;
using MediatR;

namespace DataLibrary.Mediator;

public static class CreateAddress
{
    public record Command(AddressModel Model) : IRequest<Unit>;
    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IDataContext _dataContext;
        private readonly IAddressDataServiceFactory _factory;
        public Handler(IDataContext dataContext, IAddressDataServiceFactory factory)
        {
            _dataContext = dataContext;
            _factory = factory;
        }
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            using (var uow = _dataContext.CreateUnitOfWork())
            {
                var dataService = _factory.CreateAddressDataService(uow);

                await dataService.CreateAddress(request.Model);
            }
            return Unit.Value;
        }
    }
}
