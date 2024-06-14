﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSport.Api.Models.ResponseModels.Shirt
{
    public class UpdateShirtResponse
    {
        public int Id { get; set; }

        public string? Code { get; set; }

        public string? Description { get; set; }

        public int? Quantity { get; set; }

        public string? Status { get; set; }

        public int ShirtEditionId { get; set; }

        public int SeasonPlayerId { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CreatedAccountId { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedAccountId { get; set; }

        public List<string> ImagesUrl { get; set; } = [];
    }
}
