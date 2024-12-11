This commit introduces the initial setup for integration testing in the Application project. It includes the following:

1. BaseIntegrationTest:
   - An abstract class that sets up a service scope and MediatR's ISender for integration tests.

2. IntegrationTestWebAppFactory:
   - A factory class for configuring the test web host and initializing an SQL Server container for the integration tests.

3. ProductTest:
   - A placeholder for product-related integration tests inheriting from BaseIntegrationTest.



--Setup for SQL Server and Docker--
Steps to Set Up and Run SQL Server in Docker

//Download the SQL Server image
docker pull mcr.microsoft.com/mssql/server:2019-latest

//Create and start the container
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=(TU_PASSWORD)" -p (PORTS):() --name sqlserver-test -d mcr.microsoft.com/mssql/server:2019-latest

//Verify container status
docker ps

//Stop the container (optional)
docker stop sqlserver-test

//Start the container again (if it was stopped)
docker start sqlserver-test

//Edit appsettings.json to Add the Connection String

"ConnectionStrings": {
 "DefaultConnection": "Data Source=(local)\\SQLEXPRESS;Initial Catalog=DBEcommerce;Trusted_Connection=True;TrustServerCertificate=True;", 
"TestConnection": "Data Source=localhost,1435;Initial Catalog=TestDb;User //ADD ID=sa;Password=Password123!;TrustServerCertificate=True;" }

//Modify Program.cs to select the appropriate connection string

var connectionString = builder.Environment.IsEnvironment("Test")
    ? builder.Configuration.GetConnectionString("TestConnection")
    : builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));


//Run the application in test mode
dotnet run --environment Test

//Create migrations
dotnet ef migrations add InitialMigration 

//Update the database
dotnet ef database update 



