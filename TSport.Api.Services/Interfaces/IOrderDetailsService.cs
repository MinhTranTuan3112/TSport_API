using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSport.Api.Services.Interfaces
{
    public interface IOrderDetailsService
    {
         Task AddToCart(int Userid, int shirtid, int quantity);

    }
}
