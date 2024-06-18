using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TSport.Api.Repositories.Entities;
using TSport.Api.Shared.Enums;

namespace TSport.Api.Repositories.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void ApplyShirtsGlobalFilter(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Shirt>().HasQueryFilter(s => s.Status != ShirtStatus.Deleted.ToString());
        }
    }
}