using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TSport.Api.Services.BusinessModels.Payment
{
    public class PaymentModel
    {
        public int Id { get; set; }

        public string? Code { get; set; }

        public string? PaymentMethod { get; set; }

        public string? PaymentName { get; set; }

        public decimal Amount { get; set; }

        public string Status { get; set; } = null!;

        public int? OrderId { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CreatedAccountId { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedAccountId { get; set; }
    }
}