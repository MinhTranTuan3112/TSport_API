using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TSport.Api.Models.RequestModels.ShirtEdition
{
    public class CreateShirtEditionRequest
    {
        [Required]
        public string? Code { get; set; }
        [Required]
        public string Size { get; set; } = null!;


        public bool HasSignature { get; set; }
        
        public decimal StockPrice { get; set; }

        public decimal? DiscountPrice { get; set; }

        public string? Color { get; set; }

        public string? Origin { get; set; }

        public int Quantity { get; set; }

        public string? Material { get; set; }

        public int SeasonId { get; set; }
    }
}