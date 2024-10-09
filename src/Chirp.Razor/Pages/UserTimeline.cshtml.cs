using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

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
}
