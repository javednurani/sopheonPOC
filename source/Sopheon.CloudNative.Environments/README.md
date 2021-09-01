# Running a SQL Server Docker container
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