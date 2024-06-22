using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TSport.Api.Services.BusinessModels.Season
{
    public class GetSeasonModel
    {
        public int Id { get; set; }

        public string? Code { get; set; }

        public string? Name { get; set; }

        public int? ClubId { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CreatedAccountId { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedAccountId { get; set; }
    }
}