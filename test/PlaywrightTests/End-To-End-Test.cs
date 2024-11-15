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
    
    [Test]
    public async Task NameCannotHaveSlash()
    {
        await Page.GotoAsync("http://localhost:5221/");
        await Page.GetByRole(AriaRole.Link, new() { Name = "Register" }).ClickAsync();
        await Page.GetByPlaceholder("Username").ClickAsync();
        await Page.GetByPlaceholder("Username").FillAsync("/illegaluser");
        await Page.GetByPlaceholder("name@example.com").ClickAsync();
        await Page.GetByPlaceholder("name@example.com").FillAsync("testemail@gmail.com");
        await Page.GetByLabel("Password", new() { Exact = true }).ClickAsync();
        await Page.GetByLabel("Password", new() { Exact = true }).FillAsync("Testkode0!");
        await Page.GetByLabel("Confirm Password").ClickAsync();
        await Page.GetByLabel("Confirm Password").FillAsync("Testkode0!");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();
        await Expect(Page.GetByText("The field Username must match")).ToBeVisibleAsync();
    }
    
    [Test]
    public async Task TestForXSSAttack()
    {
        await Page.GotoAsync("http://localhost:5221/");
        await Page.GetByRole(AriaRole.Link, new() { Name = "Register" }).ClickAsync();
        await Page.GetByPlaceholder("Username").ClickAsync();
        await Page.GetByPlaceholder("Username").FillAsync("Hacker");
        await Page.GetByPlaceholder("name@example.com").ClickAsync();
        await Page.GetByPlaceholder("name@example.com").FillAsync("hacker@gmail.com");
        await Page.GetByLabel("Password", new() { Exact = true }).ClickAsync();
        await Page.GetByLabel("Password", new() { Exact = true }).FillAsync("Testkode0!");
        await Page.GetByLabel("Confirm Password").ClickAsync();
        await Page.GetByLabel("Confirm Password").FillAsync("Testkode0!");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Click here to confirm your" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Login" }).ClickAsync();
        await Page.GetByPlaceholder("name@example.com").ClickAsync();
        await Page.GetByPlaceholder("name@example.com").ClickAsync();
        await Page.GetByPlaceholder("name@example.com").FillAsync("hacker@gmail.com");
        await Page.GetByPlaceholder("password").ClickAsync();
        await Page.GetByPlaceholder("password").FillAsync("Testkode0!");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
        await Page.Locator("#CheepMessage").ClickAsync();
        await Page.Locator("#CheepMessage").FillAsync("Muhaha XSS!<script>alert('Hacked');</script>");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Share" }).ClickAsync();
        //You can't click your home page if u need to click alert()
        await Page.GetByRole(AriaRole.Link, new() { Name = "Hello Hacker!" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Personal data" }).ClickAsync();
        await Page.GetByRole(AriaRole.Button, new() { Name = "Delete" }).ClickAsync();
        await Page.GetByPlaceholder("Please enter your password.").ClickAsync();
        await Page.GetByPlaceholder("Please enter your password.").FillAsync("Testkode0!");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Delete data and close my" }).ClickAsync();
    }
    
    
    
}