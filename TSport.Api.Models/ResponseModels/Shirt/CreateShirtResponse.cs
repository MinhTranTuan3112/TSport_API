using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace TSport.Api.Models.ResponseModels.Shirt
{
    public class CreateShirtResponse
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

        public virtual Entities.Account CreatedAccount { get; set; } = null!;

        public virtual ICollection<Image> Images { get; set; } = new List<Image>();

        public virtual Entities.Account? ModifiedAccount { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        public virtual SeasonPlayer SeasonPlayer { get; set; } = null!;

        public virtual ShirtEdition ShirtEdition { get; set; } = null!;
    }
}
