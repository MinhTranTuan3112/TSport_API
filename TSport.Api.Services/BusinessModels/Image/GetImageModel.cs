using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TSport.Api.Services.BusinessModels.Image
{
    public class GetImageModel
    {
        public int Id { get; set; }

        public string? Url { get; set; }

        public int ShirtId { get; set; }
    }
}