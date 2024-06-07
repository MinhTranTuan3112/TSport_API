using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSport.Api.Services.BusinessModels
{
    public class SeasonPlayerModel
    {
        public int Id { get; set; }

        public int SeasonId { get; set; }

        public int PlayerId { get; set; }
    }
}
