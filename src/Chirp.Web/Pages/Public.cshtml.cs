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
        
        var authorName = User.Identity?.Name;
        if ( authorName == null )
        {
            return Page();
        }
        var author = await _service.GetAuthorByEmail(authorName);
        if ( author == null )
        {
            return Page();
        }
        
        await _service.AddCheep(CheepMessage, author.Name, author.Email);
        
        return RedirectToPage("Public");
    }
}
