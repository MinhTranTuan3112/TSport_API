using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSport.Api.Services.BusinessModels.Player;
using TSport.Api.Services.BusinessModels.Season;

namespace TSport.Api.Services.BusinessModels.Club
{
    public class GetClubDetailsModel
    {
        public int Id { get; set; }

        public string? Code { get; set; }

        public string Name { get; set; } = null!;

        public string? LogoUrl { get; set; }

        public string Status { get; set; } = null!;

        public DateTime CreatedDate { get; set; }

        public int CreatedAccountId { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedAccountId { get; set; }

        public ICollection<GetPlayerModel> Players { get; set; } = [];

        public ICollection<GetSeasonModel> Seasons { get; set; } = [];
    }
}