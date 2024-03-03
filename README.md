# LoginApp - Web Application
<p align="center">
 <b>Developed with DotNet 7.0 x Bootstrap 5 x Angular 14</b>
 <img src="https://res.cloudinary.com/dhpo94oka/image/upload/v1709474256/Github/Description/b9ksoex1o7ynsnfcesvq.png" alt="Tech Stacks">
</p>
 
 ## Description
This application was designed with a focus on user experience and functionality. The client-side of the application was built using Angular 14, 
while the server-side was developed using Microsoft .NET Core 7. The design of the application was brought together using Bootstrap 5, creating 
a cohesive and modern look. Additionally, the development process of this project emphasizes test-driven development and includes unit tests for 
each component. These tests are automated, ensuring that the application is free of bugs throughout the development process. The testing libraries 
used in this project include <b>NUnit</b> on .NET Core and <b>Jasmine</b> on Angular, providing comprehensive and reliable testing.

Currently, there is an SQLite Database attached as well.  

The password for all users is Pa$$w0rd  
admin username: admin  
regular username: chris
 
 ## What can this application do?
This application allows users to register for a new account by providing their username, password, and email through the use of reactive forms. 
These forms will inform the user of the required details needed for registration. Once registered, users can login to the application securely by 
utilizing a JSON Web Token (JWT), which is used to securely transfer information between the dotnet server and the client web application. Additionally, 
the application also includes admin features, which allow designated admins to delete users.

## How to setup
Pull the repository and store it in the designated location

### Restore Project Files: 
<ul>
<li> Angular project files can be restored by going into the <b>client folder</b> then running <b>npm install</b> </li> 
<li> Dotnet project files can be restored by going into the <b>api folder</b> then running <b>dotnet restore</b> </li> 
<li> The dotnet tests is a separate project and also needs to be restored. This can be done by going into the <b>api folder</b> then going into the <b>Tests folder</b>
then running <b>dotnet restore</b> </li>
</ul>

### Running Tests:
<ul> 
<li>Angular tests can be executed with Jasmine by going into the <b>client folder</b> then running <b>ng test</b></li> 
<li>Dotnet tests can be executed with NUnit by going into the <b>api folder then the tests folder within the api folder</b> then running <b>dotnet test</b></li> 
</ul> 

### Running the project locally
<ul>
<li> Angular project can be served locally by going into the <b>client folder</b> then running <b>ng serve</b>. 
The default port should be 4200, so navigate to https://localhost:4200. </li>
<li> Dotnet server can be served locally by going into the <b>api folder</b> then running <b>dotnet run</b>. The default port should be 5001 </li> 
</ul> 

## Current Test Coverage

Areas of the project that currently has automated tests
### Client
<ul> 
<li> Register Component </li> 
<li> Account Service </li> 
</ul>

### API
<ul> 
<li> Account Controller </li> 
</ul>

## Screenshots

| Jasmine Angular Tests  | NUnit .NET Core Tests |
| ------------- | ------------- |
| <img width="500" src="https://res.cloudinary.com/diywkaahn/image/upload/v1675032329/Github/LoginApp/Jasmine_Tests_g9tmpg.png" alt="Jasmine Angular Tests">  | <img width="500" src="https://res.cloudinary.com/diywkaahn/image/upload/v1675032325/Github/LoginApp/Nunit_Tests_ttv2qf.png" alt="NUnit .Net Core Tests">  |
