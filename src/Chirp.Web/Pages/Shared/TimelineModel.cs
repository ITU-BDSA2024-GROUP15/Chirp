using Chirp.Core;
using Chirp.Infrastructure.Chirp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Build.Framework;

namespace Chirp.Web.Pages.Shared;

public class TimelineModel : PageModel
{
    protected readonly IChirpService Service;
    public int PageNumber { get; set; }
    [BindProperty]
    [Required]
    public string? CheepMessage { get; set; }
    
    [BindProperty]
    public string? FollowsName { get; set; }
    
    [BindProperty]
    public int? LikedCheepId { get; set; }
    
    public List<CheepDto>? Cheeps { get; set; }
    
    public string? PageName { get; set; }
    
    public TimelineModel(IChirpService service)
    {
        Service = service;
    }
    //TODO: Make an unget that handles the common page logic
    
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
        var author = await Service.GetAuthorDtoByName(authorName);
        if ( author == null )
        {
            return Page();
        }

        if (CheepMessage != null) await Service.AddCheep(CheepMessage, author.Username, author.Email);

        return RedirectToPage(PageName);
    }
    
    
    public async Task<IActionResult> OnPostFollow()
    {
        var authorName = User.Identity?.Name;
        
        if (authorName != null)
            if (FollowsName != null)
                await Service.AddFollowing(authorName, FollowsName);

        return RedirectToPage(PageName);
    }
    
    public async Task<IActionResult> OnPostUnfollow()
    {
        var authorName = User.Identity?.Name;


        if (authorName != null)
            if (FollowsName != null)
                await Service.RemoveFollowing(authorName, FollowsName);

        return RedirectToPage(PageName);
    }
    
    public async Task<IActionResult> OnPostLike()
    {
        var authorName = User.Identity?.Name;

        if (authorName != null && LikedCheepId != null)
        {
            await Service.AddLike(authorName, LikedCheepId.Value);
        }
        
        return RedirectToPage(PageName);
    }
    
    public async Task<IActionResult> OnPostUnlike()
    {
        var authorName = User.Identity?.Name;

        if (authorName != null && LikedCheepId != null)
        {
            await Service.RemoveLike(authorName, LikedCheepId.Value);
        }
        
        return RedirectToPage(PageName);
    }
}