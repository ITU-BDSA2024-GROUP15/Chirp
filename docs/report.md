# Introduction
Link to repository https://github.com/ITU-BDSA2024-GROUP15/Chirp


# Design and architecture


## Domain model
The domain model for our Chirp application consists of the classes Cheep, Author, Follow and the relations between them, can be seen in the figure below. Author inherits from Microsoft.AspNetCore.Identity's IdentityUser.
![](images/DomainModelChirp.drawio.png)



## Architecture — In the small
Our onion architecture is composed of 4 layers. The innermost layer consists of our domain model. In the second layer we have our repositories, which are responsible for interacting directly with the data model, along with our Data Transfer Objects. The third layer is the service layer, which translates the data output by the repositories into DTOs, so that it may be used on the fourth and outermost web layer. The fourth layer also contains our tests.


![](images/OnionArchitecture.png)
## Architecture of deployed application

## User activities

## Sequence of functionality/calls through Chirp!

The sequence of calls that happens through Chirp when an unauthorized user/author tries to access the root endpoint “/” can be seen in the sequence diagram (figure):
````mermaid
sequenceDiagram
  participant Client as :Client
  participant Chirp as Chirp:Public.cshtml.cs
  participant ChirpService as Chirp:ChirpService
  participant cheepRepo as Chirp:CheepRepository
  participant followRepo as Chirp:FollowRepository
  participant db as :Sqlite Database



  Client ->>+ Chirp: 1. GET ("/")
  Chirp ->> Chirp: 1.1 Check for author name
  Chirp ->> Chirp: 1.2 HandlePageNumber()
  Chirp ->>+ ChirpService: 1.3 GetCheeps(pageNumber, authorName?)
  ChirpService->>+cheepRepo: 1.4 GetCheeps(pageNumber)
  cheepRepo->>+db: QUERY: 1.5 Cheeps
  db-->>-cheepRepo: RESULT: 1.6 Cheep OBJECTS
  cheepRepo-->>-ChirpService: 1.7 List<Cheep>
  ChirpService ->> ChirpService: 1.8 ConvertToCheepDTO
  ChirpService->>+followRepo: 1.9 GetFollowed(authorname)
  followRepo->>+db: 1.10 QUERY: Follows
  db -->>- followRepo: 1.11 RESULT: Follow OBJECTS
  followRepo -->>-ChirpService: 1.12 List<Follow>
  ChirpService -->>- Chirp: 1.13 List<CheepDTO>
  Chirp ->> Chirp: 1.14 Is User Authenticated
  Chirp -->>- Client: 1.15 RESPONSE: PageResult

````
It should be noted that:
1. We check if the user author name exists in 1.1. This determines which GetCheeps methods should be called. This is our first “check” to see if a user/author is logged in, but this is also checked using identity when the html is rendered in Public.cshtml.
2. The method ConvertToCheepDTO calls GetFollowed.

# Process
## Build, test, release, and deployment
The chirp application is built & tested, released and deployed using three different Github Workflows. The build & test workflow is triggered by any pushes or pull requests to main. This ensures that any code pulled to main can be build and passes all test.

The release workflow builts, tests and then makes a release if previous built and test passes and a commit contains a tag on the form “v*.*.*”. The release contains a windows, linux and macOS version of the application.

The deployment workflow builds, tests and deploys the Chirp application to azure.

````mermaid
flowchart TD
    subgraph Build & Test Workflow
        A(( )):::blackNode --> B[Setup .NET]
        B --> Q[Build]

        Q --> C[Test]
        
        C --> H(( )):::redCircle
    end

    subgraph Release Workflow
        D(( )):::blackNode --> E[Setup .NET]
        E --> F[Build]

        F --> G[Test]

        G --> J[Build Linux]
        J --> K[Build MacOS]
        K --> L[Build Windows]
        L --> M[Release]
        
        M --> I(( )):::redCircle
    end

    subgraph Deployment Workflow
        1(( )):::blackNode --> 2[Setup .NET]
        2 --> 3[Build Application]

        3 --> 4[Test]
        4 --> 5[Publish Application]
        5 --> 6[Deploy to Azure]
        
        
        6 --> 11(( )):::redCircle
    end
        classDef blackNode fill:#000,stroke:#000,color:#fff;
        classDef redCircle stroke:#f00,stroke-width:2px, fill:#000;

````
## Teamwork
### Unresolved Tasks
![](images/teamwork.png)


**110:** This task has been mostly resolved. We have created unit tests which test each part of the deletion process, as well as a UI-tests for this functionality, but we wanted to find a way to test all parts of the process at once, including the part which comes with the identity package, directly on the database.

**170:** We wanted to change our razor pages to redirect directly to the login page after registering, rather than redirecting to the public timeline. This is a very easy thing to fix, but it would break most of our playwright tests, and we therefore chose to put it off for now.

### Flow of tasks
The following flowchart shows the activities that happen from the creation of a task until its deletion.
````mermaid
---
config:
  theme: mc
  look: classic
---
flowchart TD
    Issue(Issue created) --> Assign(Developers are assigned)
    Assign --> Branch(New branch created)
    Branch --> Work(Developers work on issue)
    Work --> Acceptance{Acceptance criteria met}
    Acceptance --> |No| Work
    Acceptance --> |Yes| Pull(Pull request is created)
    Pull --> Tests(Tests are run automatically)
    Pull --> Review(Other Developers review request)
    Tests --> PullAccept{Checks pass}
    Review --> PullAccept
    PullAccept --> |No| Work
    PullAccept --> |Yes| Merge(Branch is merged)
    Merge --> Complete(Issue is marked as complete)
    Complete --> Delete(Branch is deleted)

````
## How to make Chirp! work locally
Dotnet 8 and Git is needed to run this project locally.


**Step 1:** Clone the repository by opening a terminal and executing the following command in a folder of your choice:
`git clone https://github.com/ITU-BDSA2024-GROUP15/Chirp.git`

**Step 2:** Start the Jetbrains Rider application. Make sure you open the project by choosing chirp.sln from the cloned the repository.
![](images/sln.png)

**Step 3:** Open the terminal in Rider. In the terminal, write the command cd `.\src\Chirp.Web\`

**Step 4:** Set the correct user secrets by executing the 2 following commands one by one in the terminal:

`dotnet user-secrets set "authentication:github:clientId" "Ov23likvwJ8LuwxPP70k"`

`dotnet user-secrets set "authentication:github:clientSecret" "39d79a4303bb6a707700bab54de74d3f37f64196"`

After each command, the terminal should write something like: successfully saved [...] to the secret store.

**Step 5:** You should now be set to run the program. Run the program by executing the command `dotnet run` in the terminal.

**Step 6:** The command should take a little time to finish. When it’s done, the terminal should display something like this:

![](images/terminal.png)

Copy the localhost link from the terminal into a browser (preferably firefox for the best experience). The browser should now display our site, as seen in the picture below.

![](images/chirp.png)

## How to run test suite locally
In the application there are three different kind of tests:
Unit tests
Integration test
End to end test / UI test
Unit tests focus on individual methods and their functionality in our repositories and Chirp service. In our unit test we often had to call another method to setup the test, meaning to keep it being a unit test and not an integration test, we ensured that the call to setup was on a sublayer. If we are testing AddCheep in CheepRepository, we would check if the cheep is added using a direct query on our CheepDbContext.
The integration tests test different parts / methods as a group and their interaction between each other.

The end to end tests are used to test entire user journeys. This ensures that our different components, repositories and services work together in union. Using playwright also tests how the user interacts with our application though the browser.

All tests are made to easier catch bugs and errors that may occur when changes are made.

### To run the Unit and Integration tests:
**Step 1:** Open the terminal, find the project folder and navigate to “*test\Chirp.Web.Tests*” \
**Step 2:** Run the command “*dotnet test*”

### To run the end to end test / playwright test:
**If playwright is not installed:** Follow the steps on the [offical website](https://playwright.dev/docs/intro) to install it \
**Step 1:** Open the terminal, find the project folder and navigate to “*\Chirp\src\Chirp.Web*” \
**Step 2:** Run the command “*dotnet run*” \
**Step 3:** Ensure you are not logged in on the application \
**Step 4:** Open a new terminal window, find the project folder and navigate to “*test\PlaywrightTests*” \
**Step 5:** Run the command “*dotnet test*” 


# Ethics
## License
We have chosen the MIT-license. This is in line with the packages we have used in the project. 
To see the full licence agreement go to: https://github.com/ITU-BDSA2024-GROUP15/Chirp/blob/main/LICENSE

## LLMs, ChatGPT, CoPilot, and others
We used ChatGPT and BING-CoPilot as a “secondary TA”, by having it explain possible reasons for us getting a particular error or explaining concepts we didn’t understand fully. We mostly used it for explaining complicated stack traces, or finding errors in a code snippet, but no prompt was ever given to the LLMs for generating the code from scratch.

The responses we got were mostly helpful and hastened our process of understanding our code and concept we were uncertain on. Sometimes, the answer we got was not entirely true, or had some faults, but those could be ironed out. Since the LLMs decreased the amount of time we needed for understanding key concepts, it helped us get to the code part faster and faster handling of errors, than if we had decided not to use it. 
