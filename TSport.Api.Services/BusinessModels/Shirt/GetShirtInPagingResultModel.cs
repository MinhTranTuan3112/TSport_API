using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSport.Api.Services.BusinessModels.Image;
using TSport.Api.Services.BusinessModels.ShirtEdition;

namespace TSport.Api.Services.BusinessModels.Shirt
{
    public class GetShirtInPagingResultModel
    {
        public int Id { get; set; }

        public string Code { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public int? Quantity { get; set; }

        public string Status { get; set; } = null!;

        public int ShirtEditionId { get; set; }

        public int SeasonPlayerId { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CreatedAccountId { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedAccountId { get; set; }

        public ICollection<GetImageModel> Images { get; set; } = [];

        public GetShirtEdtionModel ShirtEdition { get; set; } = null!;
    }
}