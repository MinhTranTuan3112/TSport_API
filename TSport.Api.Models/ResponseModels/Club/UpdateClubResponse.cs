using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSport.Api.Models.ResponseModels.Club
{
    public class UpdateClubResponse
    {

        public int Id { get; set; }

        public string? Code { get; set; }

        public string? Name { get; set; }

        public string? LogoUrl { get; set; }

        public string? Status { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CreatedAccountId { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedAccountId { get; set; }

    }
}
