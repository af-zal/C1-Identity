using Microsoft.AspNetCore.Identity;
using IdentityApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace IdentityApp.Authorization
{
    public class InvoiceCreatorAuthorizationHandler :
        AuthorizationHandler<OperationAuthorizationRequirement, Invoice>
    {

        //verify user if they are allowed to make operation on invoice
        //can make changes to invoice if he is the creator

        private UserManager<IdentityUser> _userManager;

        public InvoiceCreatorAuthorizationHandler(UserManager<IdentityUser> userManager) //to get the current user
        {
            _userManager = userManager;
        }

        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            OperationAuthorizationRequirement requirement, 
            Invoice invoice)
        {

            if (context.User == null || invoice == null)
                return Task.CompletedTask;


            if(requirement.Name != Constants.CreateOperationName &&
                requirement.Name != Constants.ReadOperationName &&
                requirement.Name != Constants.UpdateOperationName &&
                requirement.Name != Constants.DeleteOperationName)
            {
                return Task.CompletedTask;
            }

            //context.User is current loggedin user
            //if I'm the creator of invoice
            if (invoice.CreatorId == _userManager.GetUserId(context.User))
                context.Succeed(requirement);

            return Task.CompletedTask;

        }
    }
}
