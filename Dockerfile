FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
WORKDIR /

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /
COPY DotNetAPI.sln ./
COPY DotNetAPI.API2/DotNetAPI2/*.csproj ./DotNetAPI.API2/DotNetAPI2/
COPY DotNetAPI.Core/*.csproj ./DotNetAPI.Core/
COPY DotNetAPI.Domain/*.csproj ./DotNetAPI.Domain/
COPY DotNetAPI.Infrastructure.Database/*.csproj ./DotNetAPI.Infrastructure.Database/
COPY DotNetAPI.Worker.EmailCampaignHandler/*.csproj ./DotNetAPI.Worker.EmailCampaignHandler/
COPY DotNetAPI.Worker.Quartz/*.csproj ./DotNetAPI.Worker.Quartz/
COPY . ./

RUN dotnet restore
COPY . .
WORKDIR /DotNetAPI.API2/DotNetAPI2
RUN dotnet publish -c Release -o /app

WORKDIR /DotNetAPI.Core
RUN dotnet build -c Release -o /app

WORKDIR /DotNetAPI.Domain
RUN dotnet build -c Release -o /app

WORKDIR /DotNetAPI.Infrastructure.Database
RUN dotnet build -c Release -o /app

WORKDIR /DotNetAPI.Worker.EmailCampaignHandler
RUN dotnet build -c Release -o /app

WORKDIR /DotNetAPI.Worker.Quartz
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
#COPY --from=build ./ScarabeoStudioCore/wwwroot/UserFiles /app/wwwroot/UserFiles

RUN apt-get update \
    && apt-get install unzip \
    && curl -sSL https://aka.ms/getvsdbgsh | /bin/sh /dev/stdin -v latest -l /vsdbg

EXPOSE 5232
ENV ASPNETCORE_URLS=http://*:5232
ENV ASPNETCORE_ENVIRONMENT=Development
ENTRYPOINT ["dotnet", "DotNetAPI2.dll", "--environment=Development"]