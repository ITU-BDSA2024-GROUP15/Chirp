using Chirp.Infrastructure.Chirp.Services;
using Chirp.Web.Pages.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Chirp.Web.Pages;

public class PublicModel : TimelineModel
{

    public PublicModel(IChirpService service) : base(service)
    {
        
    }
    
    public async Task<ActionResult> OnGet([FromQuery] int page)
    {
        
        var authorName = User.Identity?.Name;
        if ( authorName == null )
        {
            Cheeps = await Service.GetCheeps(page);
        }
        else
        {
            Cheeps = await Service.GetCheeps(page, authorName);
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
