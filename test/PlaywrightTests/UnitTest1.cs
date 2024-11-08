namespace PlaywrightTests;


using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class ExampleTest : PageTest
{
    [Test]
    public async Task HasTitle()
    {
        await Page.GotoAsync("https://bdsa2024group15chirprazor.azurewebsites.net/");

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("Chirp!"));
    }

    [Test]
    public async Task GetStartedLink()
    {
        await Page.GotoAsync("https://bdsa2024group15chirprazor.azurewebsites.net/");

        await Page.GetByRole(AriaRole.Link, new() { Name = "Register" }).ClickAsync();
        await Page.GetByRole(AriaRole.Heading, new() { Name = "Register", Exact = true }).ClickAsync();
        await Page.GetByRole(AriaRole.Heading, new() { Name = "Create a new account." }).ClickAsync();
        await Page.GetByRole(AriaRole.Heading, new() { Name = "Use another service to" }).ClickAsync();
        await Page.GetByText("Username").ClickAsync();
        await Page.GetByText("Email").ClickAsync();
        await Page.GetByText("Password", new() { Exact = true }).ClickAsync();
        await Page.GetByText("Confirm Password").ClickAsync();
    } 
}

/*
public class Tests
{
    [SetUp]
    public void Setup()
    {
    }
    
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Microsoft.Playwright;
    using Microsoft.Playwright.NUnit;
    using NUnit.Framework;

    namespace PlaywrightTests;

    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class ExampleTest : PageTest
    {
        [Test]
        public async Task HasTitle()
        {
            await Page.GotoAsync("https://playwright.dev");

            // Expect a title "to contain" a substring.
            await Expect(Page).ToHaveTitleAsync(new Regex("Playwright"));
        }

        [Test]
        public async Task GetStartedLink()
        {
            await Page.GotoAsync("https://playwright.dev");

            // Click the get started link.
            await Page.GetByRole(AriaRole.Link, new() { Name = "Get started" }).ClickAsync();

            // Expects page to have a heading with the name of Installation.
            await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Installation" })).ToBeVisibleAsync();
        } 
    }

    [Test]
    public void Test1()
    {
        Assert.Pass();
    }
}
*/
