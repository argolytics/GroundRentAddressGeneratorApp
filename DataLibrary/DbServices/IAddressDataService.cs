using DataLibrary.Models;

namespace DataLibrary.DbServices
{
    public interface IAddressDataService
    {
        Task CreateAddress(AddressModel addressModel);
    }
}