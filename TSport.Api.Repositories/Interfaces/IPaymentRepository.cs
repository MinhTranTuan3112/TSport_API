using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSport.Api.Repositories.Entities;

namespace TSport.Api.Repositories.Interfaces
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {

        Task<int> getAmmountPayment();
    }
}
