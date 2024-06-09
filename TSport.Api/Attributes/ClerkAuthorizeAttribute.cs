using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using TSport.Api.Repositories.Entities;
using TSport.Api.Services.Interfaces;
using TSport.Api.Shared.Exceptions;

namespace TSport.Api.Attributes
{
    public class ClerkAuthorizeAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
    {
        public new string[] Roles { get; set; }  = [];//Ví dụ: "Customer,Staff"

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            //nhớ thêm field clerkId vào User Model
            var clerkId = context.HttpContext.User.Identity?.Name;
            if (string.IsNullOrEmpty(clerkId))
            {
                context.Result = new Microsoft.AspNetCore.Mvc.UnauthorizedResult();

                //throw 403 here
                throw new ForbiddenMethodException("You don't have permission to access this resource");
            }

            var accountService = context.HttpContext.RequestServices.GetRequiredService<IAccountService>();

            var account = await accountService.GetAccountByClerkId(clerkId);

            if (account is null)
            {
                //Throw 403
                throw new ForbiddenMethodException("Account not found");
            }

            // var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();

            // var user = userService.getUserByClerkId(clerkId); //nhớ include cả Role
            // if (user == null)
            // {
            //     throw 403 here
            //     return;
            // }

            // Store the user in the HttpContext
            // context.HttpContext.Items["User"] = user;
            context.HttpContext.Items["Account"] = account;

            if (Roles is []) {
                return; // Roles rỗng nghĩa là chỉ cần có token là OK, Role gì ko quan trọng. Chứ không phải lỗi. Chỗ này return thôi
            }

            if (!Roles.Contains(account.Role))
            {
                //throw 403 here
                throw new ForbiddenMethodException("You don't have permission to access this resource");
            }
        }
    }
}