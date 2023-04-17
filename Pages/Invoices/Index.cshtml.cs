using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using IdentityApp.Data;
using IdentityApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using IdentityApp.Authorization;

namespace IdentityApp.Pages.Invoices
{
    //Make this page avbl for everyone
    [AllowAnonymous]
    public class IndexModel : DI_BasePageModel
    {
        //private readonly IdentityApp.Data.ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager)//(IdentityApp.Data.ApplicationDbContext context)
            : base(context, authorizationService, userManager)
        {
        }

        public IList<Invoice> Invoice { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (Context.Invoice != null)
            {
                //user/accountant see invoices created by him
                //Manager should see every invoice

                var invoices = from i in Context.Invoice
                               select i;


                var isManager = User.IsInRole(Constants.InvoiceManagersRole);
                var isAdmin = User.IsInRole(Constants.InvoiceAdminRole);

                var currentUserId = UserManager.GetUserId(User);

                //Invoice = await Context.Invoice.Where(i =>i.CreatorId == currentUserId).ToListAsync();
                //Invoice = await Context.Invoice.ToListAsync();

                if (isManager == false && isAdmin == false)
                {
                    invoices = invoices.Where(i => i.CreatorId == currentUserId);
                }

                Invoice = await invoices.ToListAsync();
            }
        }
    }
}
