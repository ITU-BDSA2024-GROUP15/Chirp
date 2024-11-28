using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Core;
using Chirp.Infrastructure.Chirp.Services;
using Microsoft.AspNetCore.Authentication;

using Microsoft.Build.Framework;

namespace Chirp.Web.Pages;

public class UserTimelineModel : PageModel
{
    
    private readonly ICheepService _service;
    public int PageNumber { get; set; }
    [BindProperty]
    [Required]
    public string? CheepMessage { get; set; }
    
    [BindProperty]
    public string? FollowsName { get; set; }
    
    public UserTimelineModel(ICheepService service)
    {
        _service = service;
    }
    
    
    public List<CheepDto>? Cheeps { get; set; }

    //TODO: ensure you cannot follow/ unfollow self
    //TODO: ensure you can unfollow others
    
    
    public async Task<ActionResult> OnGet([FromQuery] int page, string author)
    {
        //Add so get cheeps from author also gets the cheeps that the author is following
        if (User.Identity != null && User.Identity.Name == author)
        {
            Cheeps = await _service.GetCheepsForTimeline(author, page); 
        }
        else
        {
            Cheeps = await _service.GetCheepsFromAuthor(page, author);
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
    
    
    public async Task<IActionResult> OnPost()
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
        var author = await _service.GetAuthorDtoByName(authorName);
        if ( author == null )
        {
            return Page();
        }

        if (CheepMessage != null) await _service.AddCheep(CheepMessage, author.Username, author.Email);

        return RedirectToPage("UserTimeline");
    }
    
    
    
    
    public IActionResult OnGetLogin()
    {
        if ( User.Identity != null && User.Identity.IsAuthenticated )
        {
            return Redirect("/");
        }
        
        var authenticationProperties = new AuthenticationProperties
        {
            RedirectUri = Url.Page("/") // Redirect back to the home page after successful login
        };
        return Challenge(authenticationProperties, "GitHub");
    }
    
    public async Task<IActionResult> OnPostFollow()
    {
        Console.WriteLine("Followed");
        
        var authorName = User.Identity?.Name;
        
        Console.WriteLine("waaB: " + authorName + FollowsName);
        if (authorName != null)
            if (FollowsName != null)
                await _service.AddFollowing(authorName, FollowsName);

        return RedirectToPage("UserTimeline");
    }
    
    public async Task<IActionResult> OnPostUnfollow()
    {
        Console.WriteLine("Followed");
         
        var authorName = User.Identity?.Name;


        if (authorName != null)
            if (FollowsName != null)
                await _service.RemoveFollowing(authorName, FollowsName);

        return RedirectToPage("UserTimeline");
    }
    
}
