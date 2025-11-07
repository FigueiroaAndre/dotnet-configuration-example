FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /App

COPY . ./
RUN dotnet restore
RUN dotnet publish -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
ENV ASPNETCORE_HTTP_PORTS="5000"
WORKDIR /App
COPY --from=build /App/out .
ENTRYPOINT ["dotnet", "ConfigurationExample.dll"]
