using DataLibrary.Models;
using Dapper;
using System.Data;
using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;

public class AddressSqlDataService : IAddressDataService
{
    private readonly IUnitOfWork _unitOfWork;

    public AddressSqlDataService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task CreateAddress(AddressModel addressModel)
    {
        var parms = new
        {
            addressModel.AccountId,
            addressModel.IsRedeemed
        };
        var dynParms = new DynamicParameters(parms);
        dynParms.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
        await _unitOfWork.Connection.ExecuteAsync("spAddress_Create", dynParms,
            commandType: CommandType.StoredProcedure, transaction: _unitOfWork.Transaction);
        addressModel.Id = dynParms.Get<int>("Id");
    }
}
