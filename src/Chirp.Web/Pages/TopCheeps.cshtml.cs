using Chirp.Core;
using Chirp.Infrastructure.Chirp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class TopCheeps : PageModel
{
    
    private readonly IChirpService _chirpService;
    public List<CheepDto>? Cheeps { get; set; }
    
    
    public TopCheeps(IChirpService service)
    {
        _chirpService = service;
    }
    
    public async Task<ActionResult> OnGet()
    {

        Cheeps = await _chirpService.GetTopLikedCheeps();
        Console.WriteLine("AAAA:" + Cheeps.Count);
        
        return Page();

    }
}