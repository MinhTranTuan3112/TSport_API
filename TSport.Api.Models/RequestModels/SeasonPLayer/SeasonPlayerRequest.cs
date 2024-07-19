using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSport.Api.Models.RequestModels.SeasonPLayer
{
    public class SeasonPlayerRequest
    {
      //  public required string Code { get; set; }
        public required int Seasonid { get; set; }
        public required int playerid { get; set; }
      // public required string PlayerName { get; set; }
    }
}
