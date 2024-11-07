using System.Security.Claims;
using Chirp.Core;
using Chirp.Infrastructure.Chirp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Build.Framework;

namespace Chirp.Web.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepService _service;
   
    public List<CheepDto>? Cheeps { get; set; }
    
    [BindProperty]
    [Required]
    public string CheepMessage { get; set; }
    

    public PublicModel(ICheepService service)
    {
        _service = service;
        
    }
    
    public async Task<ActionResult> OnGet([FromQuery] int page)
    {
        
        Cheeps = await _service.GetCheeps(page);
      
        return Page();
    }
    
    public async Task<IActionResult> OnPost()
    {
        //We check if any validation rules has exceeded
        if ( !ModelState.IsValid )
        {
            return Page();
        }
        
        Console.WriteLine("AAA" + CheepMessage);
        //TODO bug: User.Identity.Name gives email and not name?
        await _service.AddCheep(CheepMessage, User.Identity.Name, "Bobby@testemail.com");
        
        return RedirectToPage("Public");
    }
}
