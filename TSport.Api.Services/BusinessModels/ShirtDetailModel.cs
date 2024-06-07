using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSport.Api.Models.Entities;
using TSport.Api.Models.ResponseModels.Account;

namespace TSport.Api.Services.BusinessModels
{
    public class ShirtDetailModel
    {
        public int Id { get; set; }

        public string? Code { get; set; }

        public string? Description { get; set; }

        public int? Quantity { get; set; }

        public string? Status { get; set; }

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

        public SeasonPlayerModel? SeasonPlayer { get; set; }

        public ShirtEditionModel? ShirtEdition { get; set; }
    }
}
