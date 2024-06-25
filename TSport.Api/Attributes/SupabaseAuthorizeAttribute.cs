using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using TSport.Api.Services.Interfaces;
using TSport.Api.Shared.Exceptions;

namespace TSport.Api.Attributes
{
    public class SupabaseAuthorizeAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
    {
        public new string[] Roles { get; set; } = [];

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            var supabaseId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(supabaseId))
            {
                context.Result = new Microsoft.AspNetCore.Mvc.UnauthorizedResult();

                throw new ForbiddenMethodException("You don't have permission to access this resource");
            }

            var accountService = context.HttpContext.RequestServices.GetRequiredService<IAccountService>();

            var account = await accountService.GetAccountBySupabaseId(supabaseId);

            context.HttpContext.Items["User"] = account;
            
            if (Roles is [])
            {
                return;
            }

            if (!Roles.Contains(account.Role))
            {
                throw new ForbiddenMethodException("You don't have permission to access this resource");
            }
            
        }
    }
}