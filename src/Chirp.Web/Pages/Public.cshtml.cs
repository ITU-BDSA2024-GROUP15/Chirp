using System.ComponentModel.DataAnnotations;
using Chirp.Core;
using Chirp.Infrastructure.Chirp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class PublicModel : PageModel
{
    private readonly IChirpService _service;
    public List<CheepDto>? Cheeps { get; set; }
    public int PageNumber { get; set; }
    [BindProperty]
    [Microsoft.Build.Framework.Required]
    [StringLength(160, ErrorMessage = "The message must not exceed 160 characters.", MinimumLength = 1)]
    public string? CheepMessage { get; set; }
    [BindProperty]
    public string? FollowsName { get; set; }
    
    [BindProperty]
    public int? LikedCheepId { get; set; }

    public PublicModel(IChirpService service)
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
        var author = await _service.GetAuthorDtoByName(authorName);
        if ( author == null )
        {
            return Page();
        }

        if ( CheepMessage != null && CheepMessage.Length > 160 )
        {
            return Page();
        }

        if (CheepMessage != null) await _service.AddCheep(CheepMessage, author.Username, author.Email);

        return RedirectToPage("Public");
    }


    public async Task<IActionResult> OnPostFollow()
    {
        
        var authorName = User.Identity?.Name;
        
        if (authorName != null)
            if (FollowsName != null)
                await _service.AddFollowing(authorName, FollowsName);

        return RedirectToPage("Public");
    }
    
    public async Task<IActionResult> OnPostUnfollow()
    {
         
        var authorName = User.Identity?.Name;


        if (authorName != null)
            if (FollowsName != null)
                await _service.RemoveFollowing(authorName, FollowsName);

        return RedirectToPage("Public");
    }
    
    public async Task<IActionResult> OnPostLike()
    {
         
        var authorName = User.Identity?.Name;

        if (authorName != null && LikedCheepId != null)
        {
            await _service.AddLike(authorName, LikedCheepId.Value);
        }
        
        return RedirectToPage("Public");
    }
    
    public async Task<IActionResult> OnPostUnlike()
    {
         
        var authorName = User.Identity?.Name;

        if (authorName != null && LikedCheepId != null)
        {
            await _service.RemoveLike(authorName, LikedCheepId.Value);
        }
        
        return RedirectToPage("Public");
    }
}
