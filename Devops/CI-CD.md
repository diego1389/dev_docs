## Continuos Integration:
    - Developers push code changes every day, multiple times a day. For every push you can create a set of scripts to build and test your pplication automatically. Each change submitted to an application, even to development branches is built and tested automatically and continously. 
## Continuous Delivery:
    - A step beyond CI. Not only is your application built and tested each time a code change is pushed but it is also deployed continuously. With continous delivery you trigger the deployments manually. 
## Continuous Deployment:
    - Another step beyond. Similar to Continous delivery but the application can be deployed automatically without human intervention. 

- Jenkins is a server-side continuous integration tool. Dev team is in charge of commiting the code to the Dev branch. When changes are adequately committed to the Dev-Branch, Jenkins can download the source code from Github and map it to a configured job for a particular role. Once a job is configured, you must ensure that continuous integration and continuous development are completed for the job/task.
- On successful completion of a job, Jenkins will fetch the code from the Github repository, and then it starts the task’s commit process. Jenkins will proceed to a new phase called the task’s construct phase.
- The task construct phase is the phase where Jenkins will compile the code and have it deployed after the DevOps team merges it to the Master branch, then the code is ready for deployment.
- The deployment process is activated once Jenkins has deployed the code. It is then deployed to the server using a Docker container.
- A docker container is essentially a set of virtual environments where we can create a server and deploy the objects to be examined. The use of Docker will enable the developer to run an entire cluster in a matter of seconds.