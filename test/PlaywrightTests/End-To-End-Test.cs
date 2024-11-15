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
        await Page.Locator("p").Filter(new() { HasText = "Jacqualine Gilcoine Starbuck" }).GetByRole(AriaRole.Link).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Public Timeline" }).ClickAsync();
        await Page.Locator("body").ClickAsync();
    }
    
    [Test]
    public async Task CanSeePublicTimeline()
    {
        await Page.GotoAsync("https://bdsa2024group15chirprazor.azurewebsites.net/");
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Public Timeline" })).ToBeVisibleAsync();
    }
    
    [Test]
    public async Task HomePageHasRegisterButton()
    {
        await Page.GotoAsync("https://bdsa2024group15chirprazor.azurewebsites.net/");
        await Expect(Page.GetByRole(AriaRole.Link, new() { Name = "Register" })).ToBeVisibleAsync();

    }
    
    [Test]
    public async Task HomePageHasLoginButton()
    {
        await Page.GotoAsync("https://bdsa2024group15chirprazor.azurewebsites.net/");
        await Expect(Page.GetByRole(AriaRole.Link, new() { Name = "Login" })).ToBeVisibleAsync();
    }
    
    [Test]
    public async Task GoFromPublicTimelineToUserTimeLine()
    {
        await Page.GotoAsync("https://bdsa2024group15chirprazor.azurewebsites.net/");
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Public Timeline" })).ToBeVisibleAsync();
        await Page.Locator("p").Filter(new() { HasText = "Jacqualine Gilcoine Starbuck" }).GetByRole(AriaRole.Link).ClickAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Jacqualine Gilcoine's Timeline" })).ToBeVisibleAsync();
    }
    
    
    [Test]
    public async Task GoFromUserTimeLineToPublicTimeline()
    {
        await Page.GotoAsync("https://bdsa2024group15chirprazor.azurewebsites.net/Jacqualine%20Gilcoine");
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Jacqualine Gilcoine's Timeline" })).ToBeVisibleAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Public Timeline" }).ClickAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Public Timeline" })).ToBeVisibleAsync();
    }
    
    [Test]
    public async Task JacqualineTimelineExists()
    {
        await Page.GotoAsync("https://bdsa2024group15chirprazor.azurewebsites.net/Jacqualine%20Gilcoine");
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Jacqualine Gilcoine's Timeline" })).ToBeVisibleAsync();
    }
    
    [Test]
    public async Task JacqualineCheepAboutStarbucksExists()
    {
        await Page.GotoAsync("https://bdsa2024group15chirprazor.azurewebsites.net/Jacqualine%20Gilcoine");
        await Expect(Page.Locator("#messagelist")).ToContainTextAsync("Jacqualine Gilcoine Starbuck now is what we hear the worst. — 08/01/2023 13:17:39");
    }
    
    [Test]
    public async Task CanClickRegisterPublicTimeLine()
    {
        await Page.GotoAsync("https://bdsa2024group15chirprazor.azurewebsites.net/");
        
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Public Timeline" })).ToBeVisibleAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Register" }).ClickAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Create a new account." })).ToBeVisibleAsync();
    }
    
    [Test]
    public async Task CanClickRegisterUserTimeLine()
    {
        await Page.GotoAsync("https://bdsa2024group15chirprazor.azurewebsites.net/Jacqualine%20Gilcoine");
        
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Jacqualine Gilcoine's Timeline" })).ToBeVisibleAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Register" }).ClickAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Create a new account." })).ToBeVisibleAsync();
    }
    
    [Test]
    public async Task CanClickLoginPublicTimeline()
    {
        await Page.GotoAsync("https://bdsa2024group15chirprazor.azurewebsites.net/");
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Public Timeline" })).ToBeVisibleAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Login" }).ClickAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Use a local account to log in." })).ToBeVisibleAsync();
    }
    
    
    [Test]
    public async Task CanClickLoginUserTimeLine()
    {
        await Page.GotoAsync("https://bdsa2024group15chirprazor.azurewebsites.net/Jacqualine%20Gilcoine");
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Jacqualine Gilcoine's Timeline" })).ToBeVisibleAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Login" }).ClickAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Use a local account to log in." })).ToBeVisibleAsync();
    }
    
    [Test]
    public async Task HasPage2()
    {
        await Page.GotoAsync("https://bdsa2024group15chirprazor.azurewebsites.net/?page=2");
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Public Timeline" })).ToBeVisibleAsync();
    }
    
}