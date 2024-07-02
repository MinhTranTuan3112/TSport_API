using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSport.Api.Services.BusinessModels.Player;
using TSport.Api.Services.BusinessModels.Season;

namespace TSport.Api.Services.BusinessModels.SeasonPlayer
{
    public class SeasonPlayerWithSeasonAndClubModel
    {
        public int Id { get; set; }

        public int SeasonId { get; set; }

        public int PlayerId { get; set; }

        public GetPlayerModel Player { get; set; } = null!;

        public SeasonWithClubModel Season { get; set; } = null!;
    }
}