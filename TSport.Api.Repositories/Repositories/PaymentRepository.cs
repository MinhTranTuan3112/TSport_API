using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSport.Api.Repositories.Entities;
using TSport.Api.Repositories.Interfaces;

namespace TSport.Api.Repositories.Repositories
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {

        private readonly TsportDbContext _context;

        public PaymentRepository(TsportDbContext context) : base(context)
        {
            _context = context;

        }

        public async Task<int> getAmmountPayment()
        {
            return await _context.Payments.CountAsync();
        }
    }
}
