using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Core;
using Chirp.Infrastructure.Chirp.Services;
using Microsoft.AspNetCore.Authentication;


namespace Chirp.Web.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepService _service;
    public List<CheepDto>? Cheeps { get; set; }

    public UserTimelineModel(ICheepService service)
    {
        _service = service;
    }
    
    
    public async Task<ActionResult> OnGet([FromQuery] int page, string author)
    {
        Cheeps = await _service.GetCheepsFromAuthor(page, author);
        
        return Page();
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
