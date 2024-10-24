using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Chirp.Web.Pages;

public class login : PageModel
{
    public IActionResult OnGet()
    {   
        //Chatgpt var lige en champ med Challenge.
        var authenticationProperties = new AuthenticationProperties
        {
            RedirectUri = Url.Page("/signin-github") // Redirect back to the home page after successful login
        };

        return Challenge(authenticationProperties, "GitHub");
    }
}