using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSport.Api.Services.BusinessModels
{
    public class ShirtEditionModel
    {
        public int Id { get; set; }

        public string? Code { get; set; }

        public string Size { get; set; } = null!;

        public bool HasSignature { get; set; }

        public decimal StockPrice { get; set; }

        public decimal? DiscountPrice { get; set; }

        public string? Color { get; set; }

        public string Status { get; set; } = null!;

        public string? Origin { get; set; }

        public int Quantity { get; set; }

        public string? Material { get; set; }

        public int SeasonId { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CreatedAccountId { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedAccountId { get; set; }
    }
}
