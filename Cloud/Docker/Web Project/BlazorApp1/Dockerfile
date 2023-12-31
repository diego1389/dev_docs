FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["BlazorApp1/BlazorApp1.csproj", "BlazorApp1/"]
RUN dotnet restore "BlazorApp1/BlazorApp1.csproj"
COPY . .
WORKDIR "/src/BlazorApp1"
RUN dotnet publish "BlazorApp1.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM nginx:alpine
COPY --from=build /app/publish/wwwroot /usr/share/nginx/html

