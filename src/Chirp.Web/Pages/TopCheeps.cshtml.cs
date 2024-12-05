using Chirp.Core;
using Chirp.Infrastructure.Chirp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class TopCheeps : PageModel
{
    
    private readonly IChirpService _service;
    public List<CheepDto>? Cheeps { get; set; }
    [BindProperty]
    public string? FollowsName { get; set; }
    
    [BindProperty]
    public int? LikedCheepId { get; set; }
    public int PageNumber { get; set; }
    
    public TopCheeps(IChirpService service)
    {
        _service = service;
    }
    
    public async Task<ActionResult> OnGet([FromQuery] int page)
    {

        Cheeps = await _service.GetTopLikedCheeps(User.Identity.Name, page); 
       
        
        if ( page == 0  || page < 0)
        {
            PageNumber = 1;
        }
        else
        {
            PageNumber = page;
        }
        
        return Page();

    }
    
    public async Task<IActionResult> OnPostFollow()
    {
        var authorName = User.Identity?.Name;
        
        if (authorName != null)
            if (FollowsName != null)
                await _service.AddFollowing(authorName, FollowsName);

        return RedirectToPage("TopCheeps");
    }
    
    public async Task<IActionResult> OnPostUnfollow()
    {
        var authorName = User.Identity?.Name;


        if (authorName != null)
            if (FollowsName != null)
                await _service.RemoveFollowing(authorName, FollowsName);

        return RedirectToPage("TopCheeps");
    }
    
    public async Task<IActionResult> OnPostLike()
    {
        var authorName = User.Identity?.Name;

        if (authorName != null && LikedCheepId != null)
        {
            await _service.AddLike(authorName, LikedCheepId.Value);
        }
        
        return RedirectToPage("TopCheeps");
    }
    
    public async Task<IActionResult> OnPostUnlike()
    {
        var authorName = User.Identity?.Name;

        if (authorName != null && LikedCheepId != null)
        {
            await _service.RemoveLike(authorName, LikedCheepId.Value);
        }
        
        return RedirectToPage("TopCheeps");
    }
}