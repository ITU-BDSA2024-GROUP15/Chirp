﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepService _service;
    public List<Cheep> Cheeps { get; set; }

    public PublicModel(ICheepService service)
    {
        _service = service;
    }

    /* public ActionResult OnGet()
    {
        Cheeps = _service.GetCheeps();
        return Page();
    } */
    
    public ActionResult OnGet([FromQuery] int page)
    {
        Console.WriteLine($"page: {page}");
      Cheeps = _service.GetCheeps(page);
      return Page();
    }
}
