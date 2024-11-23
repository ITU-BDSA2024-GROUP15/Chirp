// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Chirp.Core;
using Chirp.Infrastructure.Chirp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<Author> _userManager;
        private readonly ICheepService _service;
        public List<CheepDto>? Cheeps { get; set; }

        public IndexModel(
            ICheepService service, UserManager<Author> userManager)
        {
           _service = service;
           _userManager = userManager;
        }
        
        

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            
            Cheeps = await _service.GetAllCheepsFromAuthor(_userManager.GetUserName(User) ?? throw new InvalidOperationException());

            return Page();
        }

       
    }
}
