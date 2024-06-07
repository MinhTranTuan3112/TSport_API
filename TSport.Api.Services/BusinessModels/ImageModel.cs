using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSport.Api.Services.BusinessModels
{
    public class ImageModel
    {
        public int Id { get; set; }

        public string? Url { get; set; }

        public int ShirtId { get; set; }
    }
}
