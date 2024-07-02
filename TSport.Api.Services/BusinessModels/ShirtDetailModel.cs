using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSport.Api.Models.ResponseModels.Account;
using TSport.Api.Services.BusinessModels.SeasonPlayer;

namespace TSport.Api.Services.BusinessModels
{
    public class ShirtDetailModel
    {
        public int Id { get; set; }

        public string Code { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public int? Quantity { get; set; }

        public string Status { get; set; } = null!;

        public int ShirtEditionId { get; set; }

        public int SeasonPlayerId { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CreatedAccountId { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedAccountId { get; set; }

        public GetAccountResponse CreatedAccount { get; set; } = null!;

        public ICollection<ImageModel> Images { get; set; } = [];

        public virtual GetAccountResponse? ModifiedAccount { get; set; } = null;

        public ICollection<OrderDetailModel> OrderDetails { get; set; } = [];

        public SeasonPlayerWithSeasonAndClubModel? SeasonPlayer { get; set; }

        public ShirtEditionModel? ShirtEdition { get; set; }
    }
}
