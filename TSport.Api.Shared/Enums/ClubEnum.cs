using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSport.Api.Shared.Enums
{
    public enum ClubEnum
    {
        Active,
        Deleted
    }
    public class ClubStatus
    {
        public static string Active = ShirtEnum.Active.ToString();
        public static string Deleted = ShirtEnum.Deleted.ToString();
    }
}
