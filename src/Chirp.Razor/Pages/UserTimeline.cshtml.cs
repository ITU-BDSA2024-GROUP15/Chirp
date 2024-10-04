using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepService _service;
    public List<CheepDTO> Cheeps { get; set; }

    public UserTimelineModel(ICheepService service)
    {
        _service = service;
    }
    
    
    public ActionResult OnGet([FromQuery] int page, string author)
    {
        Func<Task> _ = async () =>
        {
            Cheeps = await _service.GetCheepsFromAuthor(page, author);
        };
        return Page();
    }
}
