using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSport.Api.Models.Payment
{
    public class PaymentInformationModel
    {
        [NotMapped]

        public string OrderType { get; set; }
        public double Amount { get; set; }
        public string OrderDescription { get; set; }
        public string Name { get; set; }
        public int AccountId { get; set; }


    }
}
