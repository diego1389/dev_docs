## Containers

- Virtual machines look a like. 
- Phisycal machine -> Host Operating System -> Containers (a lot more lightweight than VM).
- Running one thing like API, Web App. Just one app running in each container.
- VM: multiple apps running on each VM.
- You can open a shell on a container. 
- Container use cases:
    - One primary process (your app).
    - A third party app like SQL server or rabbit mq.
    - Application's runtime.
    - Dependencies (DLLs)
    - Configuration files.
    - Environment variables.

## Images

- Container is an instance of an image (live instances of an image).
- You can create multiple containers from the same image (they will be identical). It is a template but when you modify the container it will diverge from the other containers.
- 

## Container registries and Docker Hub

- Docker Hub is a container registry (where you store your images for someone to download and create a container from it).
- Docker Hub also have a lot of third-party existing images for you to use. 
- Push and image or pull and image.

## Docker CLI

- Downloads nginx image (pull) and runs it.
```bash
docker run nginx
```
- List the containers
```bash
docker ps
```
- Show stopped containers
```bash
docker ps -a
```

- Stop image (passes id from docker ps)
```bash
docker stop 643000e4a63c
```

- Remove container:
```bash
docker rm 643000e4a63c
```
## Docker Desktop GUI

- Shows list of containers and images.
- It shows hyperlinks with logs about the image information.
- It shows stats like cpu usage and memory usage.
- It shows container's files.

## Port mapping:

- Run nginx in port 80:

```bash
docker run -p 80:80 nginx --left: port inside the container / right: port outside the container
```
- Open a browser and navigate to localhost and it will show the nginx web server welcome page. 

## Detached mode and logs: 
- So Docker doesn't take over the terminal and it runs in the background.

```bash
docker run -d -p 80:80 nginx
```
- You can still interact with the instance (for example docker ps to list the process).
- Show logs:

```bash
docker logs 5fd
```
## Shell access and makes changes inside containers

- Interactive terminal (bash)

```bash
docker exec -it fa69 bash
```

- You can install vim in a container:

```bash
apt-get update && apt-get install -y vim
```

- You only change that one container not all the containers created from the same image.

## More 3rd party images

- Install sql server

## Image tags

- List all images:

```bash
docker images
```

- Tags are pointers to images.
- You can run a container from an specific image tag:

```bash
docker run -p 5672:5672 -p 15672:15672 rabbitmq:3-management
```
- latest is the default tag.
- To explicitly tag your images:
```bash
docker tab eb4a57159180 myimage:mytag
```
- Multiple tags can point to the same image id.
- To remove image (specific tag):
```bash
docker rmi nginx:v9999
```

## Image layers and caching

- Show layers:
```bash
docker inspect nginx
```

- Image layers and base image layer are readonly files (templates).
- Most images come from a base image layer (a linux OS).


Container layer
^
Image layer
^
Image layer
^
Base image layer (typically a Debian or Ubuntu)


# Creating our demo application

## Front end

- Create Blazor WebAssembly App on VSCode (run it with dotnet run).
- Inject Http client:

- Modify Home.razor view to accept HttpClient
- Add button to get podcasts

## Backend

- Create minimal API project.
- Modify Program.cs

docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=DiegoGH#123" -p 1433:1433 mcr.microsoft.com/mssql/server:2022-latest

- Add SqlConnection and Dapper to communicate with sql (also create the tables running db on Docker).
- Now we have a web app communicating to a minimal api communicating with SQL db.

# Building our own images

## Dockerfile

- Add new Dockerfile to web api project

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["DockerCourseApi/DockerCourseApi.csproj", "DockerCourseApi/"]
RUN dotnet restore "DockerCourseApi/DockerCourseApi.csproj"
COPY . .
WORKDIR "/src/DockerCourseApi"
RUN dotnet publish "DockerCourseApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5292
EXPOSE 80
EXPOSE 443
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "DockerCourseApi.dll"]

```
- Run the following command to build the image:

```bash
docker build -f ./DockerCourseApi/Dockerfile -t api .
```

Explanation:

- FROM keyword specifies the base image.
    - creates a temporal intermediate container from that base image (dotnet/sdk in this case).
- WORKDIR: creates a directory and changes directory to it if it doesn't exist (becomes current dir).
- COPY: copies the csproj file into a directory in the container that's in the current directory wich is /src/DockerCourseApi
    - .csproj is the only file that's copied in that directory at the moment.
- RUN: just runs a command as if you did it in the terminal.
- COPY . .
    - We copy everything else from the build context so all of our project, all our code so its just means that every time we do a docker build it's not doing a docker restore every single time (only if the .csproj changes).
- WORKDIR "/src/DockerCourseApi"
    - changes directory again
- RUN dotnet publish
    - executes the dotnet publish as in a command line. 

- multi stage docker files: another from block.
    - another build in prod for example (lighter that the build stage).
- EXPOSE: just documentation not actually exposes the ports.
- COPY --from=build /app/publish
    - Copies from the first state (firs FROM build line 1 to app/publish folder).
- ENTRYPOINT:
    - same as dotnet DockerCOurseApi.dll
    - runs when you spin up your container (power up).

    ## Building our API image   

    - Generate an image from previously defined dockerfile:
    ```bash
    docker build -f ./DockerCourseApi/Dockerfile -t api .
    ```
    - Run container (give it port number):

    ```bash
    docker run api -p 1333:80
    ```
    -t: tag (image name of the tag)

    ## Building our frontend images

- Add Dockerfile on frontend project:

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["BlazorApp1/BlazorApp1.csproj", "BlazorApp1/"]
RUN dotnet restore "BlazorApp1/BlazorApp1.csproj"
COPY . .
WORKDIR "/src/BlazorApp1"
RUN dotnet publish "BlazorApp1.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM nginx:alpine
COPY --from=build /app/publish/wwwroot /usr/share/nginx/html
```
- Created base on SDK
- using nginx as base image
- copies whaterver is on app/publish/wwwroot folder into the html nginx folder (where index.html from nginx lives).

- Generate an image:
```bash
docker build -f ./BlazorApp1/Dockerfile -t frontend .
```

- Run container:
```bash
docker run -p 1235:80 frontend
```

- Open browser and navigate to localhost:1235 and it should show the fronend project. 