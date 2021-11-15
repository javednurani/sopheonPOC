# Intro
This solution allows for management of "Environments" and other Azure resources to enable a product-led cloud-native product offering.

Major components include:
* Entity Framework model, migrations and Repositories, meant to run on SQL Server
* Azure Functions for CRUD operations against the Environments data store
* Unit tests on most of the above

# Running Locally

See also: https://pluto/display/PDP/Azure+Functions+and+Entity+Framework+Local+Dev

## Running a SQL Server Docker container locally
- Using Powershell 7,
- cd E:\dev\stratus\source\Sopheon.CloudNative.Environments
  - (or, your local path to the Sopheon.CloudNative.Environments solution, in Stratus repo)
- npm run start:db
  - this starts the container using the package.json and docker-compose.yml files in that folder
- docker ps -a
  - you should see a running container with Starting, or Healthy status
  - repeat command until Healthy status is observed
-  Note: SA_PASSWORD environment variable is required for the docker-compose config - this should already be set up.
- At this point you can treat the running container as an instance of SQL Server Engine running at 'localhost', or '.', or '127.0.0.1'
  - connect and query in SSMS, use in connection strings, etc
- npm run stop:db
  - this stops the container

## Provisioning the Environments database locally
- with a running SQL Server Docker container, choose to use SSMS, or Docker + sqlcmd. Perform the final seed data step in both cases.
- Using SSMS:
  - Create a database on the SQL Server container
    - connect to the SQL Server container in SSMS.  Use 'localhost', or '.', or '127.0.0.1' server, and sa login.
    - in SSMS Object Explorer, expand the SQL Server, right click Databases folder, create new Database.
  - Generate an EF Migration from Sopheon.CloudNative.Evironments.Data
    - see "Entity Framework local DB setup" in  https://pluto/display/PDP/Azure+Functions+and+Entity+Framework+Local+Dev
    - save the .sql file locally
  - Apply the Sopheon.CloudNative.Environments EF Migrations to the database
    - In SSMS, create a new Query against the database
    - Load the contents of the EF Migration .sql file into the Query and execute
- Using Docker + sqlcmd
  - Create a database on the SQL Server container
    - docker exec -ti db-dev /bin/bash
      - (container bash)> /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P {sa password}
        - (sqlcmd cli)> create database env
        - (sqlcmd cli)> go
        - (sqlcmd cli)> quit
      - (container bash)> Ctrl+P+Q
  - Generate an EF Migration from Sopheon.CloudNative.Evironments.Data
    - see "Entity Framework local DB setup" in  https://pluto/display/PDP/Azure+Functions+and+Entity+Framework+Local+Dev
    - save the .sql file locally (assume it is C:/migrations.sql for below)
  - Apply the Sopheon.CloudNative.Environments EF Migrations to the database
    - docker cp C:/migrations.sql db-dev:/migrations.sql
    - docker exec -ti db-dev /bin/bash
      - (container bash)> /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P {sa password} -i migrations.sql
- Finally, Run the Sopheon.CloudNative.Environments.Utility project on the database to seed test data
  - Configure the UserSecretManager.LocalDatabaseConnectionString value in User Secrets, see the below section "User Secrets"
    - Utility Project uses Functions Project Secrets. Secrets are managed on the Function App project.

## Function App config, local settings, secrets
- For the Function App deployed in Azure, Function App Settings is used to manage app config, and Azure Key Vault is used to manage secrets
- For local development, local.settings.json file is used to manage app config, and User Secrets is used to manage secrets
  - local.settings.json
    - checked into source control and contains non-secret/sensitive values only
    - if dev has latest source code, updates should not be necessary to run
  - User Secrets
    - managed through VS, exist in a local User/AppData file. are NOT source controlled
    - (Utility project uses Secrets managed on the Functions project)
    - NEED TO CONFIGURE THIS
      - In VS, right click on Sopheon.CloudNative.Environments.Functions, 'Manage User Secrets'
      - Update the secrets.json, adding the following properties:
        - UserSecretManager.LocalDatabaseConnectionString (this dot notation represents JSON structure in the file)
        - SQLCONNSTR_EnvironmentsSqlConnectionString
        - SqlServerAdminEnigma
        - AzSpClientEnigma
      - (Ask Cloud Team 1 devs for values)

## Local Azure storage emulation
- needed to support a Function's Blob Input Binding locally
- Azure Storage Emulator or Azurite can be used as the emulator
  - https://docs.microsoft.com/en-us/azure/storage/common/storage-use-emulator
  - https://docs.microsoft.com/en-us/azure/storage/common/storage-use-azurite
- Azure Storage Explorer is used to connect to a running storage emulator.  A local blob container and folder/file structure can be created on the storage emulator, mocking an Azure storage account
  - https://azure.microsoft.com/en-us/features/storage-explorer/
- Steps
  - run Azurite or Storage Emulator
  - open Storage Explorer GUI
    - open Connect dialog
      - 'Attach to a local emulator'
    - expand the Emulator node, right click on Blob Containers, 'Create Blob Container'
    - select the new blob, and use the '+ New Folder' button to create necessary folder structure
    - use 'Upload' button to upload files to the locally emulated blob

## Running Azure Functions locally
- Prerequisites:
  - above section "Running a SQL Server Docker container locally"
  - above section "Provisioning the Environments database locally"
  - above section "Function App config, local settings, secrets"
  - above section "Local Azure storage emulation"
- Steps
  - Open Powershell 7 .../stratus/source/Sopheon.CloudNative.Environments/Sopheon.CloudNative.Environments.Functions
  - Run the following command to start the local Azure Functions runtime: 'func start'
    - See: https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local
  - To verify functions are up and running, navigate to the following url in a browser
    - http://localhost:7071/Environments
  - To debug local azure functions
    - Run with the following option: func start --dotnet-isolated-debug
    - Note PID of function, and in Visual Studio, Debug > Attach To Process to "dotnet.exe" process with that PID

## Running Automated Tests Locally
 - Test Explorer contains a mix of unit and integration test projects
 - You should be safe to run all tests in Test Explorer and get meaningful results
 - Integration tests should check if their dependencies are available in order to be tested, and skip if not
 - Any test failures should represent real failures
 - OPEN TECH DESIGN ISSUE: Function Integration tests can potentially have 'real' side effects in Azure
    - For example, Azure SQL Databases may be Tagged as "Assigned" to customer by AllocateSqlDatabaseSharedByServicesToEnvironment
    - These tests should be skipped until a long-term strategy exists for mocking/stubbing out Azure dependencies and calls
    - These tests can be run, but BE AWARE OF THE SIDE AFFECTS, and ONLY POINT TO THE STRATUS-DEV ENVIRONMENT

# How To's

## Update Integration Test OpenAPI client
 - OpenAPI is specified in the form of data annotations above our REST API Functions.
 - The information provided in these data annotations is used to build out our help documentation, our integration tests, and our swagger client
 - To view the content generated from the OpenAPI annotations, you must run the project by right clicking it in solution explorer, select debug > start new instance.
   -  After the function has started it will output the endpoints for the functions and swagger docs in the console
   -  On the Swagger page (http://localhost:7071/swagger/ui) you can try out your functions to see if they perform as expected
 -  Integration tests using an OpenAPI client
    -  If you visit http://localhost:7071/openapi/1.0 (or some other port and version number) you can then save the JSON to the Functions.IntegrationTests project OpenApiDefinitions\ folder
    -  On build, Visual Studio will use this file to generate a client, which is then used bythe integation test.

# Design Decisions
TODO: flesh this out.  This section should explain any design decisions or implementation details that might throw a newly onboarded dev for a loop.
