This commit introduces the initial setup for integration testing in the Application project. It includes the following:

1. BaseIntegrationTest:
   - An abstract class that sets up a service scope and MediatR's ISender for integration tests.

2. IntegrationTestWebAppFactory:
   - A factory class for configuring the test web host and initializing an SQL Server container for the integration tests.

3. ProductTest:
   - A placeholder for product-related integration tests inheriting from BaseIntegrationTest.







