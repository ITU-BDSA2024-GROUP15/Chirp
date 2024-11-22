using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Chirp.Core;
using Chirp.Infrastructure.Chirp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepService _service;
    public List<CheepDto>? Cheeps { get; set; }
    public int PageNumber { get; set; }
    [BindProperty]
    [Microsoft.Build.Framework.Required]
    [StringLength(160, ErrorMessage = "The message must not exceed 160 characters.", MinimumLength = 1)]
    public string CheepMessage { get; set; }
    [BindProperty]
    public string? FollowsName { get; set; }

    public PublicModel(ICheepService service)
    {
        _service = service;
    }
    
    public async Task<ActionResult> OnGet([FromQuery] int page)
    {
        
        var authorName = User.Identity?.Name;
        if ( authorName == null )
        {
            Cheeps = await _service.GetCheeps(page);
        }
        else
        {
            Cheeps = await _service.GetCheeps(page, authorName);
        }
        
        
        if ( page == 0 )
        {
            PageNumber = 1;
        }
        else
        {
            PageNumber = page;
        }
        return Page();
    }
    
    public async Task<IActionResult> OnPostSendCheep()
    {
        //We check if any validation rules has exceeded
        if ( !ModelState.IsValid )
        {
            return Page();
        }
        
        var authorName = User.Identity?.Name;
        if ( authorName == null )
        {
            return Page();
        }
        var author = await _service.GetAuthorByName(authorName);
        if ( author == null )
        {
            return Page();
        }

        if ( CheepMessage.Length > 160 )
        {
            return Page();
        }
        
        await _service.AddCheep(CheepMessage, author.Name, author.Email);
        
        return RedirectToPage("Public");
    }


    public async Task<IActionResult> OnPostFollow()
    {
        Console.WriteLine("Followed");
        
        var authorName = User.Identity?.Name;
        
        Console.WriteLine("waaB: " + authorName + FollowsName);
        await _service.AddFollowing(authorName, FollowsName);

        return RedirectToPage("Public");
    }
    
    //TODO: copy to usertimeline
    public async Task<IActionResult> OnPostUnfollow()
    {
        Console.WriteLine("Followed");
         
        var authorName = User.Identity?.Name;
       
        
        await _service.RemoveFollowing(authorName, FollowsName);
        
        return RedirectToPage("Public");
    }
}
