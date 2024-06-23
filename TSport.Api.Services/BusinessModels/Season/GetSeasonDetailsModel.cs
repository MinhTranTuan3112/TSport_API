using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSport.Api.Models.ResponseModels.Account;
using TSport.Api.Services.BusinessModels.Club;
using TSport.Api.Services.BusinessModels.SeasonPlayer;
using TSport.Api.Services.BusinessModels.ShirtEdition;

namespace TSport.Api.Services.BusinessModels.Season
{
    public class GetSeasonDetailsModel
    {
        public int Id { get; set; }

        public string? Code { get; set; }

        public string Name { get; set; } = null!;

        public int? ClubId { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CreatedAccountId { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedAccountId { get; set; }

        public string Status { get; set; } = null!;

        public GetClubModel? Club { get; set; }

        public GetAccountResponse CreatedAccount { get; set; } = null!;

        public GetAccountResponse? ModifiedAccount { get; set; }

        public ICollection<GetSeasonPlayerModel> SeasonPlayers { get; set; } = [];

        public ICollection<GetShirtEdtionModel> ShirtEditions { get; set; } = [];
    }
}