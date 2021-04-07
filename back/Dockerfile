FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build
WORKDIR /app

COPY *.sln .

COPY FreeSSL/*.csproj ./FreeSSL/
COPY FreeSSL.Domain/*.csproj ./FreeSSL.Domain/
RUN ls
RUN ls

RUN dotnet restore

COPY FreeSSL/. ./FreeSSL/
COPY FreeSSL.Domain/. ./FreeSSL.Domain/
WORKDIR /app/FreeSSL
RUN dotnet publish -c Release -o /out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
WORKDIR /app
COPY --from=build /out ./
ENV ASPNETCORE_URLS http://*:5000
ENTRYPOINT ["dotnet", "FreeSSL.dll"]