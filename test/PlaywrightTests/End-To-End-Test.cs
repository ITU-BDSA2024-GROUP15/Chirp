namespace PlaywrightTests;


using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class EndToEnd : PageTest
{
    
    [Test]
    public async Task HasTitle()
    {
        await Page.GotoAsync("https://bdsa2024group15chirprazor.azurewebsites.net/");
        await Page.GotoAsync("https://bdsa2024group15chirprazor.azurewebsites.net/");
        await Page.Locator("p").Filter(new() { HasText = "Jacqualine Gilcoine Starbuck" }).GetByRole(AriaRole.Link).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Public Timeline" }).ClickAsync();
        await Page.Locator("body").ClickAsync();
    }

    [Test]
    public async Task CanClickRegister()
    {
        await Page.GotoAsync("https://bdsa2024group15chirprazor.azurewebsites.net/");
        
        
        await Page.GetByRole(AriaRole.Link, new() { Name = "Register" }).ClickAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Create a new account." })).ToBeVisibleAsync();
    }
    
    
    
}