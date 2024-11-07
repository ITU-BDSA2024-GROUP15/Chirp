using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Core;
using Chirp.Infrastructure.Chirp.Services;

namespace Chirp.Web.Pages;

public class UserTimelineModel : PageModel
{
    public CheepBinder _cheepBinder;
    private readonly ICheepService _service;
    
    public UserTimelineModel(ICheepService service)
    {
        _service = service;
        _cheepBinder = new CheepBinder();
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
      
        
        await _service.AddCheep(Request.Form["CheepMessage"], "bob", "Bobby@testemail.com");
      
        return RedirectToPage("UserTimeline");
    }
}
