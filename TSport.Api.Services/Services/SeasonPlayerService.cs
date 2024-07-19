using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Microsoft.EntityFrameworkCore;
using TSport.Api.Models.RequestModels.SeasonPLayer;
using TSport.Api.Repositories.Entities;
using TSport.Api.Repositories.Interfaces;
using TSport.Api.Services.BusinessModels.SeasonPlayer;
using TSport.Api.Services.Interfaces;

namespace TSport.Api.Services.Services
{
    public class SeasonPlayerService : ISeasonPlayerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SeasonPlayerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddSeasonPlayer(SeasonPlayerRequest request)
        {
            var seasonPlayer = new SeasonPlayer
            {
                // = request.Code,
              SeasonId = request.Seasonid,
              PlayerId =request.playerid,

            };

            await _unitOfWork.SeasonPlayerRepository.AddAsync(seasonPlayer);

            await _unitOfWork.SaveChangesAsync();


        }

        public async Task<List<SeasonPlayerWithSeasonAndClubModel>> GetSeasonPlayers()
        {
            return await _unitOfWork.SeasonPlayerRepository.Entities.ProjectToType<SeasonPlayerWithSeasonAndClubModel>().ToListAsync();
        }
    }
}