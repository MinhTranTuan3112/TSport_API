using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSport.Api.Models.RequestModels.SeasonPLayer;
using TSport.Api.Services.BusinessModels.SeasonPlayer;

namespace TSport.Api.Services.Interfaces
{
    public interface ISeasonPlayerService
    {
       Task<List<SeasonPlayerWithSeasonAndClubModel>> GetSeasonPlayers();

        Task AddSeasonPlayer(SeasonPlayerRequest request);

    }
}