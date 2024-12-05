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
        await Page.GotoAsync("http://localhost:5221");

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("Chirp!"));
    }
    
    [Test]
    public async Task CanSeePublicTimeline()
    {
        await Page.GotoAsync("http://localhost:5221");
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Public Timeline" })).ToBeVisibleAsync();
    }
    
    [Test]
    public async Task HomePageHasRegisterButton()
    {
        await Page.GotoAsync("http://localhost:5221");
        await Expect(Page.GetByRole(AriaRole.Link, new() { Name = "Register" })).ToBeVisibleAsync();

    }
    
    [Test]
    public async Task HomePageHasLoginButton()
    {
        await Page.GotoAsync("http://localhost:5221");
        await Expect(Page.GetByRole(AriaRole.Link, new() { Name = "Login" })).ToBeVisibleAsync();
    }
    
    [Test]
    public async Task GoFromPublicTimelineToUserTimeLine()
    {
        await Page.GotoAsync("http://localhost:5221");
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Public Timeline" })).ToBeVisibleAsync();
        await Page.Locator("p").Filter(new() { HasText = "Jacqualine Gilcoine Starbuck" }).GetByRole(AriaRole.Link).ClickAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Jacqualine Gilcoine's Timeline" })).ToBeVisibleAsync();
    }
    
    
    [Test]
    public async Task GoFromUserTimeLineToPublicTimeline() 
    {
        await Page.GotoAsync("http://localhost:5221/Jacqualine%20Gilcoine");
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Jacqualine Gilcoine's Timeline" })).ToBeVisibleAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Public Timeline" }).ClickAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Public Timeline" })).ToBeVisibleAsync();
    }
    
    [Test]
    public async Task JacqualineTimelineExists()
    {
        await Page.GotoAsync("http://localhost:5221/Jacqualine%20Gilcoine");
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Jacqualine Gilcoine's Timeline" })).ToBeVisibleAsync();
    }
    
    [Test]
    public async Task JacqualineCheepAboutStarbucksExists()
    {
        await Page.GotoAsync("http://localhost:5221/Jacqualine%20Gilcoine");
        await Page.Locator("p").Filter(new() { HasText = "Jacqualine Gilcoine Starbuck" }).GetByRole(AriaRole.Link).ClickAsync();
        await Page.GetByText("Jacqualine Gilcoine Starbuck").ClickAsync();

    }
    
    [Test]
    public async Task CanClickRegisterPublicTimeLine()
    {
        await Page.GotoAsync("http://localhost:5221");
        
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Public Timeline" })).ToBeVisibleAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Register" }).ClickAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Create a new account." })).ToBeVisibleAsync();
    }
    
    [Test]
    public async Task CanClickRegisterUserTimeLine()
    {
        await Page.GotoAsync("http://localhost:5221/Jacqualine%20Gilcoine");
        
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Jacqualine Gilcoine's Timeline" })).ToBeVisibleAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Register" }).ClickAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Create a new account." })).ToBeVisibleAsync();
    }
    
    [Test]
    public async Task CanClickLoginPublicTimeline()
    {
        await Page.GotoAsync("http://localhost:5221");
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Public Timeline" })).ToBeVisibleAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Login" }).ClickAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Use a local account to log in." })).ToBeVisibleAsync();
    }
    
    
    [Test]
    public async Task CanClickLoginUserTimeLine()
    {
        await Page.GotoAsync("http://localhost:5221/Jacqualine%20Gilcoine");
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Jacqualine Gilcoine's Timeline" })).ToBeVisibleAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Login" }).ClickAsync();
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Use a local account to log in." })).ToBeVisibleAsync();
    }
    
    [Test]
    public async Task HasPage2()
    {
        await Page.GotoAsync("http://localhost:5221?Page=2");
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Public Timeline" })).ToBeVisibleAsync();
    }



        [Test] 
        public async Task CanSeeOtherUsersTimeline() 
        {
        await Page.GotoAsync("http://localhost:5221/");
        await Page.GetByRole(AriaRole.Link, new() { Name = "Mellie Yost" }).ClickAsync();
        await Page.GetByText("Mellie Yost But what was").ClickAsync();
        await Page.GetByText("Mellie Yost's Timeline Mellie").ClickAsync();
    }
    
    [Test]
    public async Task MakeTestAccount() // username = testUser    Password = Test123!    email: test@testmail.com
    {
        await Page.GotoAsync("http://localhost:5221/");
        
        await Page.GetByRole(AriaRole.Link, new() { Name = "Register", Exact = true }).ClickAsync();
        await Page.GetByPlaceholder("Username").ClickAsync();
        await Page.GetByPlaceholder("Username").FillAsync("testUser");
        await Page.GetByPlaceholder("name@example.com").ClickAsync();
        await Page.GetByPlaceholder("name@example.com").FillAsync("test@testmail.com");
        await Page.GetByLabel("Password", new() { Exact = true }).ClickAsync();
        await Page.GetByLabel("Password", new() { Exact = true }).FillAsync("Test123!");
        await Page.GetByLabel("Confirm Password").ClickAsync();
        await Page.GetByLabel("Confirm Password").FillAsync("Test123!");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Click here to confirm your" }).ClickAsync();
        
        
        await Page.GetByRole(AriaRole.Link, new() { Name = "Login" }).ClickAsync();
        await Page.GetByPlaceholder("password").ClickAsync();
        await Page.GetByPlaceholder("password").FillAsync("Test123!");
        await Page.GetByPlaceholder("Username").ClickAsync();
        await Page.GetByPlaceholder("Username").FillAsync("testUser");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
        
        await Page.GetByRole(AriaRole.Link, new() { Name = "About me" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Delete" }).ClickAsync();
        await Page.GetByPlaceholder("Please enter your password.").ClickAsync();
        await Page.GetByPlaceholder("Please enter your password.").FillAsync("Test123!");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Delete data and close my" }).ClickAsync();
        
    }
    /*
            
    [Test]
    public async Task CanCheep()
    {
        await Page.GotoAsync("http://localhost:5221/");
        
        await Page.GetByRole(AriaRole.Link, new() { Name = "Register", Exact = true }).ClickAsync();
        await Page.GetByPlaceholder("Username").ClickAsync();
        await Page.GetByPlaceholder("Username").FillAsync("testUser");
        await Page.GetByPlaceholder("name@example.com").ClickAsync();
        await Page.GetByPlaceholder("name@example.com").FillAsync("test@testmail.com");
        await Page.GetByLabel("Password", new() { Exact = true }).ClickAsync();
        await Page.GetByLabel("Password", new() { Exact = true }).FillAsync("Test123!");
        await Page.GetByLabel("Confirm Password").ClickAsync();
        await Page.GetByLabel("Confirm Password").FillAsync("Test123!");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Click here to confirm your" }).ClickAsync();
        
        
        await Page.GetByRole(AriaRole.Link, new() { Name = "Login" }).ClickAsync();
        await Page.GetByPlaceholder("password").ClickAsync();
        await Page.GetByPlaceholder("password").FillAsync("Test123!");
        await Page.GetByPlaceholder("Username").ClickAsync();
        await Page.GetByPlaceholder("Username").FillAsync("testUser");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Public Timeline" }).ClickAsync();
        
        await Page.Locator("#CheepMessage").ClickAsync();
        await Page.Locator("#CheepMessage").FillAsync("Hello chirp");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Share" }).ClickAsync();
        await Expect(Page.Locator("#messagelist")).ToContainTextAsync("testUser Hello chirp");
        
        await Page.GetByRole(AriaRole.Link, new() { Name = "About me" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Delete" }).ClickAsync();
        await Page.GetByPlaceholder("Please enter your password.").ClickAsync();
        await Page.GetByPlaceholder("Please enter your password.").FillAsync("Test123!");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Delete data and close my" }).ClickAsync();
        
    }
    */
    
    
    [Test]
    public async Task ViewOtherUsersTimelineLoggedin()
    {
        await Page.GotoAsync("http://localhost:5221/");
        
        await Page.GetByRole(AriaRole.Link, new() { Name = "Register", Exact = true }).ClickAsync();
        await Page.GetByPlaceholder("Username").ClickAsync();
        await Page.GetByPlaceholder("Username").FillAsync("testUser");
        await Page.GetByPlaceholder("Username").ClickAsync();
        await Page.GetByPlaceholder("Username").FillAsync("testUser");
        await Page.GetByLabel("Password", new() { Exact = true }).ClickAsync();
        await Page.GetByLabel("Password", new() { Exact = true }).FillAsync("Test123!");
        await Page.GetByLabel("Confirm Password").ClickAsync();
        await Page.GetByLabel("Confirm Password").FillAsync("Test123!");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Click here to confirm your" }).ClickAsync();
        
        await Page.GetByRole(AriaRole.Link, new() { Name = "Login" }).ClickAsync();
        await Page.GetByPlaceholder("password").ClickAsync();
        await Page.GetByPlaceholder("password").FillAsync("Test123!");
        await Page.GetByPlaceholder("Username").ClickAsync();
        await Page.GetByPlaceholder("Username").FillAsync("testUser");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Public Timeline" }).ClickAsync();
        
        
        // clicks on Wendell Ballans username and shows their timeline
        await Page.GetByRole(AriaRole.Link, new() { Name = "Wendell Ballan" }).ClickAsync();
        await Page.GetByRole(AriaRole.Heading, new() { Name = "Wendell Ballan's Timeline" }).ClickAsync();
        await Page.GetByText("Wendell Ballan As I turned up").ClickAsync();

                
        await Page.GetByRole(AriaRole.Link, new() { Name = "About me" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Delete" }).ClickAsync();
        await Page.GetByPlaceholder("Please enter your password.").ClickAsync();
        await Page.GetByPlaceholder("Please enter your password.").FillAsync("Test123!");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Delete data and close my" }).ClickAsync();
    }


    [Test]
    public async Task LoginAndSeeOwnTimeline()
    {
        await Page.GotoAsync("http://localhost:5221/");
        
        await Page.GetByRole(AriaRole.Link, new() { Name = "Register", Exact = true }).ClickAsync();
        await Page.GetByPlaceholder("Username").ClickAsync();
        await Page.GetByPlaceholder("Username").FillAsync("testUser");
        await Page.GetByPlaceholder("name@example.com").ClickAsync();
        await Page.GetByPlaceholder("name@example.com").FillAsync("test@testmail.com");
        await Page.GetByLabel("Password", new() { Exact = true }).ClickAsync();
        await Page.GetByLabel("Password", new() { Exact = true }).FillAsync("Test123!");
        await Page.GetByLabel("Confirm Password").ClickAsync();
        await Page.GetByLabel("Confirm Password").FillAsync("Test123!");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Click here to confirm your" }).ClickAsync();
        
        await Page.GetByRole(AriaRole.Link, new() { Name = "Login" }).ClickAsync();
        await Page.GetByPlaceholder("password").ClickAsync();
        await Page.GetByPlaceholder("password").FillAsync("Test123!");
        await Page.GetByPlaceholder("Username").ClickAsync();
        await Page.GetByPlaceholder("Username").FillAsync("testUser");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Public Timeline" }).ClickAsync();
        
        // presses my timeline and confirms it is there
        await Page.GetByRole(AriaRole.Link, new() { Name = "My Timeline" }).ClickAsync();
        await Page.GetByRole(AriaRole.Heading, new() { Name = "testUser's Timeline" }).ClickAsync();

                
        await Page.GetByRole(AriaRole.Link, new() { Name = "About me" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Delete" }).ClickAsync();
        await Page.GetByPlaceholder("Please enter your password.").ClickAsync();
        await Page.GetByPlaceholder("Please enter your password.").FillAsync("Test123!");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Delete data and close my" }).ClickAsync();
    }
    
    [Test]
    public async Task DeleteUserDeletesFollowedUsers()
    {
        await Page.GotoAsync("http://localhost:5221/");
        
        await Page.GetByRole(AriaRole.Link, new() { Name = "Register", Exact = true }).ClickAsync();
        await Page.GetByPlaceholder("Username").ClickAsync();
        await Page.GetByPlaceholder("Username").FillAsync("testUser");
        await Page.GetByPlaceholder("name@example.com").ClickAsync();
        await Page.GetByPlaceholder("name@example.com").FillAsync("test@testmail.com");
        await Page.GetByLabel("Password", new() { Exact = true }).ClickAsync();
        await Page.GetByLabel("Password", new() { Exact = true }).FillAsync("Test123!");
        await Page.GetByLabel("Confirm Password").ClickAsync();
        await Page.GetByLabel("Confirm Password").FillAsync("Test123!");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Click here to confirm your" }).ClickAsync();

        
        await Page.GetByRole(AriaRole.Link, new() { Name = "Login" }).ClickAsync();
        await Page.GetByPlaceholder("password").ClickAsync();
        await Page.GetByPlaceholder("password").FillAsync("Test123!");
        await Page.GetByPlaceholder("Username").ClickAsync();
        await Page.GetByPlaceholder("Username").FillAsync("testUser");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Public Timeline" }).ClickAsync();
        
        ///
        
        
        await Page.Locator("li").Filter(new() { HasText = "Jacqualine Gilcoine Starbuck" }).GetByRole(AriaRole.Button).ClickAsync();
        await Page.Locator("li").Filter(new() { HasText = "Mellie Yost But what was" }).GetByRole(AriaRole.Button).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "About me" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Delete" }).ClickAsync();
        await Page.GetByPlaceholder("Please enter your password.").ClickAsync();
        await Page.GetByPlaceholder("Please enter your password.").FillAsync("Test123!");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Delete data and close my" }).ClickAsync();
        
        
        await Page.GetByRole(AriaRole.Link, new() { Name = "Register", Exact = true }).ClickAsync();
        await Page.GetByPlaceholder("Username").ClickAsync();
        await Page.GetByPlaceholder("Username").FillAsync("testUser");
        await Page.GetByPlaceholder("name@example.com").ClickAsync();
        await Page.GetByPlaceholder("name@example.com").FillAsync("test@testmail.com");
        await Page.GetByLabel("Password", new() { Exact = true }).ClickAsync();
        await Page.GetByLabel("Password", new() { Exact = true }).FillAsync("Test123!");
        await Page.GetByLabel("Confirm Password").ClickAsync();
        await Page.GetByLabel("Confirm Password").FillAsync("Test123!");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Click here to confirm your" }).ClickAsync();

        
        await Page.GetByRole(AriaRole.Link, new() { Name = "Login" }).ClickAsync();
        await Page.GetByPlaceholder("password").ClickAsync();
        await Page.GetByPlaceholder("password").FillAsync("Test123!");
        await Page.GetByPlaceholder("Username").ClickAsync();
        await Page.GetByPlaceholder("Username").FillAsync("testUser");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Public Timeline" }).ClickAsync();
        
        await Page.GetByRole(AriaRole.Link, new() { Name = "About me" }).ClickAsync();
        await Expect(Page.Locator("body")).ToContainTextAsync("You're not following anyone.");
        
        await Page.GetByRole(AriaRole.Link, new() { Name = "About me" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Delete" }).ClickAsync();
        await Page.GetByPlaceholder("Please enter your password.").ClickAsync();
        await Page.GetByPlaceholder("Please enter your password.").FillAsync("Test123!");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Delete data and close my" }).ClickAsync();
    }
    /*
    
    [Test]
    public async Task CanFollowJacqualine()
    {
        await Page.GotoAsync("http://localhost:5221/");
        
        await Page.GetByRole(AriaRole.Link, new() { Name = "Register", Exact = true }).ClickAsync();
        await Page.GetByPlaceholder("Username").ClickAsync();
        await Page.GetByPlaceholder("Username").FillAsync("testUser");
        await Page.GetByPlaceholder("name@example.com").ClickAsync();
        await Page.GetByPlaceholder("name@example.com").FillAsync("test@testmail.com");
        await Page.GetByLabel("Password", new() { Exact = true }).ClickAsync();
        await Page.GetByLabel("Password", new() { Exact = true }).FillAsync("Test123!");
        await Page.GetByLabel("Confirm Password").ClickAsync();
        await Page.GetByLabel("Confirm Password").FillAsync("Test123!");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Click here to confirm your" }).ClickAsync();
        
        await Page.GetByRole(AriaRole.Link, new() { Name = "Login" }).ClickAsync();
        await Page.GetByPlaceholder("password").ClickAsync();
        await Page.GetByPlaceholder("password").FillAsync("Test123!");
        await Page.GetByPlaceholder("Username").ClickAsync();
        await Page.GetByPlaceholder("Username").FillAsync("testUser");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Public Timeline" }).ClickAsync();
        
        // asserts follow button is there and presses it 
        await Expect(Page.Locator("#messagelist")).ToContainTextAsync("Follow");        
        await Page.Locator("li").Filter(new() { HasText = "Jacqualine Gilcoine Starbuck" }).GetByRole(AriaRole.Button).ClickAsync();
        await Expect(Page.Locator("#messagelist")).ToContainTextAsync("Unfollow");
        
        await Page.GetByRole(AriaRole.Link, new() { Name = "About me" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Delete" }).ClickAsync();
        await Page.GetByPlaceholder("Please enter your password.").ClickAsync();
        await Page.GetByPlaceholder("Please enter your password.").FillAsync("Test123!");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Delete data and close my" }).ClickAsync();
    }
    
     
    [Test]
    public async Task CanFollowJacqualineAndUnfollow()
    {
        await Page.GotoAsync("http://localhost:5221/");
        
        await Page.GetByRole(AriaRole.Link, new() { Name = "Register", Exact = true }).ClickAsync();
        await Page.GetByPlaceholder("Username").ClickAsync();
        await Page.GetByPlaceholder("Username").FillAsync("testUser");
        await Page.GetByPlaceholder("name@example.com").ClickAsync();
        await Page.GetByPlaceholder("name@example.com").FillAsync("test@testmail.com");
        await Page.GetByLabel("Password", new() { Exact = true }).ClickAsync();
        await Page.GetByLabel("Password", new() { Exact = true }).FillAsync("Test123!");
        await Page.GetByLabel("Confirm Password").ClickAsync();
        await Page.GetByLabel("Confirm Password").FillAsync("Test123!");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Click here to confirm your" }).ClickAsync();
        
        
        await Page.GetByRole(AriaRole.Link, new() { Name = "Login" }).ClickAsync();
        await Page.GetByPlaceholder("password").ClickAsync();
        await Page.GetByPlaceholder("password").FillAsync("Test123!");
        await Page.GetByPlaceholder("Username").ClickAsync();
        await Page.GetByPlaceholder("Username").FillAsync("testUser");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Public Timeline" }).ClickAsync();
        
        // asserts follow button is there and presses it 
        await Expect(Page.Locator("#messagelist")).ToContainTextAsync("Follow");        
        await Page.Locator("li").Filter(new() { HasText = "Jacqualine Gilcoine Starbuck" }).GetByRole(AriaRole.Button).ClickAsync();
        // asserts unfollow button is there and presses it 
        await Expect(Page.Locator("#messagelist")).ToContainTextAsync("Unfollow");
        await Page.Locator("p").Filter(new() { HasText = "Jacqualine Gilcoine Starbuck" }).GetByRole(AriaRole.Link).ClickAsync();
        await Page.Locator("li").Filter(new() { HasText = "Jacqualine Gilcoine Starbuck" }).GetByRole(AriaRole.Button).ClickAsync();
        await Expect(Page.Locator("#messagelist")).ToContainTextAsync("Follow");
        
        await Page.GetByRole(AriaRole.Link, new() { Name = "About me" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Delete" }).ClickAsync();
        await Page.GetByPlaceholder("Please enter your password.").ClickAsync();
        await Page.GetByPlaceholder("Please enter your password.").FillAsync("Test123!");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Delete data and close my" }).ClickAsync();
    }
*/

    [Test]
    public async Task cantLoginAfterDeleteData()
    {
        await Page.GotoAsync("http://localhost:5221/");
        await Page.GetByRole(AriaRole.Link, new() { Name = "Register" }).ClickAsync();
        await Page.GetByPlaceholder("Username").ClickAsync();
        await Page.GetByPlaceholder("Username").FillAsync("tester");
        await Page.GetByPlaceholder("name@example.com").ClickAsync();
        await Page.GetByPlaceholder("name@example.com").FillAsync("test@test.dk");
        await Page.GetByLabel("Password", new() { Exact = true }).ClickAsync();
        await Page.GetByLabel("Password", new() { Exact = true }).FillAsync("123Test!");
        await Page.GetByLabel("Confirm Password").ClickAsync();
        await Page.GetByLabel("Confirm Password").FillAsync("123Test!");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Click here to confirm your" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Public Timeline" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Login" }).ClickAsync();
        await Page.GetByPlaceholder("Username").ClickAsync();
        await Page.GetByPlaceholder("Username").FillAsync("");
        await Page.GetByPlaceholder("Username").ClickAsync();
        await Page.GetByPlaceholder("Username").ClickAsync();
        await Page.GetByPlaceholder("Username").FillAsync("tester");
        await Page.GetByPlaceholder("password").ClickAsync();
        await Page.GetByPlaceholder("password").FillAsync("123Test!");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "About me" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Delete" }).ClickAsync();
        await Page.GetByPlaceholder("Please enter your password.").ClickAsync();
        await Page.GetByPlaceholder("Please enter your password.").FillAsync("123Test!");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Delete data and close my" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Login" }).ClickAsync();
        await Page.GetByPlaceholder("Username").ClickAsync();
        await Page.GetByPlaceholder("Username").FillAsync("tester");
        await Page.GetByPlaceholder("password").ClickAsync();
        await Page.GetByPlaceholder("password").FillAsync("123Test!");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
        await Expect(Page.GetByRole(AriaRole.Listitem)).ToContainTextAsync("Invalid login attempt.");
    }
    
}
