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
    
    [BindProperty]
    [Required]
    public string CheepMessage { get; set; }
    
    public UserTimelineModel(ICheepService service)
    {
        _service = service;
    }
    
    
    public List<CheepDto>? Cheeps { get; set; }

    
    
    
    public async Task<ActionResult> OnGet([FromQuery] int page, string author)
    {
        Cheeps = await _service.GetCheepsFromAuthor(page, author);
        
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
        var author = await _service.GetAuthorByEmail(authorName);
        if ( author == null )
        {
            return Page();
        }
        
        await _service.AddCheep(CheepMessage, author.Name, author.Email);
        
        return RedirectToPage("UserTimeline");
    }
    
    public IActionResult OnGetLogin()
    {
        if ( User.Identity.IsAuthenticated )
        {
            return Redirect("/");
        }
        
        var authenticationProperties = new AuthenticationProperties
        {
            RedirectUri = Url.Page("/") // Redirect back to the home page after successful login
        };
        return Challenge(authenticationProperties, "GitHub");
    }
    
}
