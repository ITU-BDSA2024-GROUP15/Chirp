using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Core;
using Chirp.Infrastructure.Chirp.Services;
using Microsoft.AspNetCore.Authentication;

using Microsoft.Build.Framework;

namespace Chirp.Web.Pages;

public class UserTimelineModel : PageModel
{
    
    private readonly IChirpService _service;
    public int PageNumber { get; set; }
    [BindProperty]
    [Required]
    public string? CheepMessage { get; set; }
    
    [BindProperty]
    public string? FollowsName { get; set; }
    
    [BindProperty]
    public int? LikedCheepId { get; set; }
    
    public UserTimelineModel(IChirpService service)
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
            Cheeps = await _service.GetCheepsFromAuthor(page, author, User.Identity.Name);
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
        var authorName = User.Identity?.Name;
        
        if (authorName != null)
            if (FollowsName != null)
                await _service.AddFollowing(authorName, FollowsName);

        return RedirectToPage("UserTimeline");
    }
    
    public async Task<IActionResult> OnPostUnfollow()
    {
        var authorName = User.Identity?.Name;


        if (authorName != null)
            if (FollowsName != null)
                await _service.RemoveFollowing(authorName, FollowsName);

        return RedirectToPage("UserTimeline");
    }
    
    public async Task<IActionResult> OnPostLike()
    {
        var authorName = User.Identity?.Name;

        if (authorName != null && LikedCheepId != null)
        {
            await _service.AddLike(authorName, LikedCheepId.Value);
        }
        
        return RedirectToPage("UserTimeline");
    }
    
    public async Task<IActionResult> OnPostUnlike()
    {
        var authorName = User.Identity?.Name;

        if (authorName != null && LikedCheepId != null)
        {
            await _service.RemoveLike(authorName, LikedCheepId.Value);
        }
        
        return RedirectToPage("UserTimeline");
    }
    
}
