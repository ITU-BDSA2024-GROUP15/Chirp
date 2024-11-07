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
    public CheepBinder _cheepBinder;
    public List<CheepDto>? Cheeps { get; set; }
    

    public PublicModel(ICheepService service)
    {
        _service = service;
        _cheepBinder = new CheepBinder();
    }
    
    public async Task<ActionResult> OnGet([FromQuery] int page)
    {
        Console.WriteLine("message " + _cheepBinder.CheepMessage);
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
        
        await _service.AddCheep(Request.Form["CheepMessage"], "bob", "Bobby@testemail.com");
        
        return RedirectToPage("Public");
    }
}
