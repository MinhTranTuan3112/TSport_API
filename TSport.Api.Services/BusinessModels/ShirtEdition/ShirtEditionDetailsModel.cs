using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSport.Api.Models.ResponseModels.Account;
using TSport.Api.Services.BusinessModels.Season;
using TSport.Api.Services.BusinessModels.Shirt;

namespace TSport.Api.Services.BusinessModels.ShirtEdition
{
    public class ShirtEditionDetailsModel
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

        public GetAccountResponse CreatedAccount { get; set; } = null!;

        public GetAccountResponse? ModifiedAccount { get; set; }

        public GetSeasonModel Season { get; set; } = null!;

        public ICollection<GetShirtModel> Shirts { get; set; } = [];
    }
}