# Intro
This solution allows for management of "Environments" and other Azure resources to enable a product-led cloud-native product offering.

Major components include:
* Entity Framework model, migrations and Repositories, meant to run on SQL Server
* Azure Functions for CRUD operations against the Environments data store
* Unit tests on most of the above

# Running Locally

## Running a SQL Server Docker container locally
- TODO: clean this up
Docker Desktop > Add Folder Sharing > E:\dev\envm
- (or, your local path to the 'Sopheon Environment Management' repo folder)
- this will enable Docker shared volumes between host and container, in the E:\dev\envm\dockervolumes folder.  
- If you don't share the folder, Docker Desktop will just prompt you on container start
cd E:\dev\envm
- (or, your local path to the 'Sopheon Environment Management' repo folder)
npm run start:db
- this starts the container
docker ps -a
- you should see a running container with Healthy status
-    SA_PASSWORD environment variable is required - this should already be set up.
- container may take 15-30 sec to start up, but should be faster
At this point you can treat the running container as a localhost/./127.0.0.1 running SQL Server
- connect to SSMS, use in connection strings, etc
npm run stop:db
- this stops the container
npm run prune
- this removes stopped containers and untagged images
- shouldn't be necessary for this simple container, but containers with more intermediate build steps will require this for dev environment neatness

## Running the Environments database locally
- TODO: flesh this out
- In "Package Manager Console", ensure "Data" project is selected
- Runt he following commmand:  Script-Migration -Idempotent

## Running Azure Functions locally
- TODO: flesh this out
- In "Developer Powershell", navigate to the "Sopheon.CloudNative.Environments.Functions" folder
- Run the following command: func start
- To verify functions are up and running, you navigate to the following url in a browser
  - http://localhost:7071/Environments
- To debug local azure functions
  - Run with the following option: func start --dotnet-isolated-debug
  - Note PID of function, and attach to "dotnet.exe" process with that PID

# Design Decisions
TODO: flesh this out.  This section should explain any design decisions or implementation details that might throw a newly onboarded dev for a loop.