using System.Diagnostics.CodeAnalysis;
using Chirp.Infrastructure.Chirp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Build.Framework;

namespace Chirp.Web.Pages;

public class CheepBinder : PageModel
{
    [BindProperty]
    [Required]
    public string CheepMessage { get; set; }
}