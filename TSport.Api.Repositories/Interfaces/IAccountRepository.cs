using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSport.Api.Repositories.Entities;

namespace TSport.Api.Repositories.Interfaces
{
    public interface IAccountRepository : IGenericRepository<Account>
    {
        Task<List<Account>> GetAllAcountCustomer();

        Task<Account?> GetCustomerDetailsInfo(int id);

        Task<Account?> GetCustomerAccountWithOrderInfo(string supabaseId);

    }
}