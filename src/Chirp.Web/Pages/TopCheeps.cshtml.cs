using Chirp.Infrastructure.Chirp.Services;
using Chirp.Web.Pages.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Chirp.Web.Pages;

public class TopCheeps : TimelineModel
{
    
    
    public TopCheeps(IChirpService service) : base(service)
    {
        IsTopList = true;
    }
    
    public async Task<ActionResult> OnGet([FromQuery] int page)
    {
        var authorname = User.Identity?.Name;
        PageNumber = page;
        await HandlePageNumber();
        if (authorname != null)
        {
            Cheeps = await Service.GetTopLikedCheeps(authorname, PageNumber);     
        }
        
        return Page();
    }
    
}