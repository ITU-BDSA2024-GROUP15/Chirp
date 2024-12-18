---
title: _Chirp!_ Project Report
subtitle: ITU BDSA 2024 Group 15
author:
- "Kassandra Annika Wadum <kwad@itu.dk>"
- "Ida Amalie Stougaard <iast@itu.dk>"
- "Mads Wolff Christensen <mawc@itu.dk>"
- "Marius Emil Holm <marho@itu.dk>"
- "Morten Friis Bønneland <mofb@itu.dk>"
numbersections: true

geometry:
    - top=1.5cm 
    - bottom=1.5cm 
    - left=1.5cm 
    - right=1.5cm
urlcolor: blue
linkcolor: blue
---

Repository link : <https://github.com/ITU-BDSA2024-GROUP15/Chirp>


# Design and architecture


## Domain model
The domain model for our Chirp application consists of two entities represented by the classes Cheep, Author, along with the class Follow, which is a relation between two Authors. 
Author inherits from Microsoft.AspNetCore.Identity's IdentityUser. While we use object references between Cheep and Author, our "Likes" list and our Follow object instead use the name of an author to reference it.\
![](images/DomainModelChirp.drawio.png) \
*Illustration of our domain model*


## Architecture — In the small
The architecture of our Chirp application follows the onion architecture to secure loosely coupled layers and to ensure compliance with the IoC principle. 

The onion architecture is composed of 4 layers. The innermost layer consists of our domain model. 
In the second layer we have our repositories, which are responsible for interacting directly with 
the data model, along with our Data Transfer Objects. The third layer is the service layer, 
which translates the data output by the repositories into DTOs, so that it may be used on the 
fourth and outermost web layer. The fourth layer also contains our tests.

![](images/OnionArchitecture.png) \
*Illustration of our architecture*

## Architecture of deployed application 
The Chirp! application is hosted on Azure. Clients may interact with the app by HTTPS requests through the razor pages in the Chirp.Web package. The server itself communicates with github servers in order to facilitate github authentication using OAuth. 

![ClientServerArchitecture.drawio.png](images%2FClientServerArchitecture.drawio.png) \
*Illustration of our deployed Chirp applications architecture*

## User activities
The typical scenarios of a user journey, before and after they log in, are illustrated with two UML user activity diagrams. The user will in both scenarios start on the public timeline, and can from that point take the actions shown with the arrows. Users can always use the buttons from the navigation bar that are illustrated at the top left corner of both diagrams.


### UML Activity Diagram - Unauthorized user

![](images/UML activity diagram(unauthorized).png) \
*Illustration of a user journey through our Chirp! application when unauthorized*


### UML Activity Diagram - Authorized user

![](images/UML-activity-diagram(authorized).png) \
*Illustration of a user journey through our Chirp! application when Authorized*

## Sequence of functionality/calls through Chirp!

The sequence of calls and flow of data and messages that happens through the Chirp application, when an unauthorized user/author tries to access the root endpoint “/”, can be seen in the sequence diagram below. \
![](images/Sequence%20diagram%20functionality-2024-12-16-124006.png) \
*Illustration of flow of communication in Chirp, when an unauthorized user makes a call to the endpoint "`/`"*

It should be noted that:

1. We check if the user author name exists in 1.1. This determines which GetCheeps methods should be called. This is our first “check” to see if a user/author is logged in, but this is also checked using identity when the html is rendered in Public.cshtml. \
2. The method ConvertToCheepDTO calls GetFollowed.

# Process
## Build, test, release, and deployment
The chirp application is built, tested, released and deployed using three different GitHub Workflows. The build & test workflow is triggered by any pushes or pull requests to main. This ensures that any code pulled to main can be built and passes all tests.

The release workflow builds, tests and then makes a release if previous build and tests passes and the push contains a tag on the form `v*.*.*`. The release contains a windows, linux and macOS version of the application.

The deployment workflow builds, tests and deploys the Chirp application to azure if the build and tests passes.

We also have a workflow for automatically converting our report.md-file to a PDF-file upon pushing to GitHub.

![](images/Build%20&%20Test-2024-12-17-221326.png) \
*Illustration of how we build, test, release and deploy Chirp! via workflows*

## Teamwork
![](images/teamwork.png) \
*Image of our project board*

### Unresolved Tasks
**110:** This task has mostly been resolved. We have created unit tests that test each part of the deletion process, as well as a UI-tests for this functionality, but we wanted to find a way to test all parts of the process at once, including the part which comes with the identity package, directly on the database.

**170:** We wanted to change our razor pages to redirect directly to the login page after registering, rather than redirecting to the public timeline. This is a very easy thing to fix, but it would break most of our playwright tests, and we therefore chose to put it off for now.

### Flow of tasks
The following flowchart shows the activities that happen from the creation of a task until its deletion. 
![IssueFlow.png](images%2FIssueFlow.png)
*Illustration of the flow of tasks*

## How to make Chirp! work locally
Dotnet 8 and Git is needed to run this project locally.

**Step 1:** Clone the repository by opening a terminal and executing the following command in a folder of your choice:
*`"git clone https://github.com/ITU-BDSA2024-GROUP15/Chirp.git"`*

**Step 2:** Start the Jetbrains Rider application. Make sure you open the project by choosing chirp.sln from the cloned repository. \
![](images/sln.png)

**Step 3:** Open the terminal in Rider. In the terminal, run the command cd *`".\src\Chirp.Web\"`*

**Step 4:** Set the correct **local** user secrets by executing the 2 following commands one by one in the terminal:

*`"dotnet user-secrets set "authentication:github:clientId" "Ov23likvwJ8LuwxPP70k"`*

*`"dotnet user-secrets set "authentication:github:clientSecret" "39d79a4303bb6a707700bab54de74d3f37f64196"`*

*(Note: These secrets are only used for the reader to be able to run the application locally)*

After each command, the terminal should write something like: successfully saved [...] to the secret store.

**Step 5:** You should now be set to run the program. Run the program by executing the command *`"dotnet run"`* in the terminal.

**Step 6:** The command should take a little time to finish. When it’s done, the terminal should display something like this:

![](images/terminal.png)

Copy the localhost link from the terminal into a browser (preferably firefox for the best experience). The browser should now display our site, as seen in the picture below.

![](images/chirp.png)

## How to run test suite locally
In the application there are three different kind of tests:
Unit tests
Integration tests
End-to-end/UI tests. 

Unit tests focus on individual methods and their functionality in our repositories and Chirp service. In our unit tests we 
often had to set up the tests by calling methods from a sublayer, to keep it 
being a unit test rather than an integration test. E.g. if we are testing AddCheep 
in CheepRepository, we would check if the cheep is added using a direct query on our CheepDbContext.
The integration tests test different methods or classes in groups to verify that their interactions are correct.

The end-to-end tests are used to test entire user journeys. This ensures that our different components, repositories and services work together in union. 
Using playwright also tests how the user interacts with our application though the browser.

All tests are made to easier catch bugs and errors that may occur when changes are made.

### To run the Unit and Integration tests: 
**Step 1:** Open the terminal, find the project folder and navigate to *`"test\Chirp.Web.Tests"`* \
**Step 2:** Run the command *`“dotnet run”`*

### To run the end-to-end test/playwright test:
**If playwright is not installed:** Follow the steps on the [offical website](https://playwright.dev/docs/intro) to install it \
**Step 1:** Open the terminal, find the project folder and navigate to *`“Chirp\src\Chirp.Web`* \
**Step 2:** Run the command *`“dotnet run”`* \
**Step 3:** Ensure you are not logged in on the application \
**Step 4:** Open a new terminal window, find the project folder and navigate to *`"test\PlaywrightTests"`*  \
**Step 5:** Run the command *`“dotnet test”`*


# Ethics
## License
We have chosen the MIT-license. This is in line with the packages we have used in the project. 
To see the full licence agreement go to: <https://github.com/ITU-BDSA2024-GROUP15/Chirp/blob/main/LICENSE>

## LLMs, ChatGPT, CoPilot, and others
We used ChatGPT and BING-CoPilot as a “secondary TA”. We mostly used them to explain complicated stack traces, to find errors in a code snippet, to explain concepts we didn't fully understand, and for guidance when setting up our different workflows for GitHub actions.

The responses we got were mostly helpful in speeding up progress in areas of uncertainty. Sometimes, the answer we got was not entirely correct, but it still conveyed a general idea. Since the LLMs decreased the amount of time we needed for understanding key concepts, it helped us get to coding faster and avoid spending hours reading through stackoverflow to understand our errors. 
