using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TSport.Api.Services.BusinessModels.SeasonPlayer
{
    public class GetSeasonPlayerModel
    {
        public int Id { get; set; }

        public int SeasonId { get; set; }

        public int PlayerId { get; set; }

    }
}