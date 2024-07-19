using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSport.Api.Models.RequestModels.SeasonPLayer
{
    public class SeasonPlayerReponse
    {
        public required string Code { get; set; }

        public required string SeasonName { get; set; }
        // table Season
        public string SeasonTableName { get; set; } = null!;
        // table Player 
        public string PlayerTableName { get; set; } = null!;

    }
}
