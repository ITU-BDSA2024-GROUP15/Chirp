using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepService _service;
    public List<Cheep> Cheeps { get; set; }

    public UserTimelineModel(ICheepService service)
    {
        _service = service;
    }
    
    
    public ActionResult OnGet([FromQuery] int page, string author)
    {
        Cheeps = _service.GetCheepsFromAuthor(author, page);
        foreach ( var cheep in Cheeps)
        {
            Console.WriteLine(cheep);
        }
        return Page();
    }
}
