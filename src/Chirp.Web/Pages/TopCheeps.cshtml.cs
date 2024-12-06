using Chirp.Core;
using Chirp.Infrastructure.Chirp.Services;
using Chirp.Web.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class TopCheeps : TimelineModel
{
    
    
    public TopCheeps(IChirpService service) : base(service)
    {
        
    }
    
    public async Task<ActionResult> OnGet([FromQuery] int page)
    {
        var authorname = User.Identity?.Name;
        if (authorname != null)
        {
            Cheeps = await Service.GetTopLikedCheeps(authorname, page);     
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
    
}