using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TSport.Api.Services.BusinessModels.Shirt
{
    public class ShirtWithImagesAndShirtEditionModel
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

        public ICollection<ImageModel> Images { get; set; } = [];

        public ShirtEditionModel ShirtEdition { get; set; } = null!;
    }
}