using Microsoft.AspNetCore.Mvc;
using Chirp.Infrastructure.Chirp.Services;
using Chirp.Web.Pages.Shared;

namespace Chirp.Web.Pages;

public class UserTimelineModel : TimelineModel
{

    public UserTimelineModel(IChirpService service) : base(service)
    {
        
    }
       
    public async Task<ActionResult> OnGet([FromQuery] int page, string authorName)
    {
        //Add so get cheeps from author also gets the cheeps that the author is following
        if (User.Identity != null && User.Identity.Name == authorName)
        {
            Cheeps = await Service.GetCheepsForTimeline(authorName, page); 
        }
        else
        {
            var spectatingAuthorName = User.Identity?.Name;
            if (spectatingAuthorName != null)
            {
                Cheeps = await Service.GetCheepsFromAuthor(page, authorName, spectatingAuthorName);
            }
            else
            {
                Cheeps = await Service.GetCheepsFromAuthor(page, authorName, "");
            }
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
