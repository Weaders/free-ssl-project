FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build
WORKDIR /app

COPY *.sln .

COPY FreeSSL/*.csproj ./FreeSSL/
COPY FreeSSL.Domain/*.csproj ./FreeSSL.Domain/
COPY FreeSSL.Tests/*.csproj ./FreeSSL.Tests/

RUN dotnet restore

COPY FreeSSL/. ./FreeSSL/
COPY FreeSSL.Domain/. ./FreeSSL.Domain/
COPY FreeSSL.Tests/. ./FreeSSL.Tests/

WORKDIR /app
RUN dotnet publish -c Release -o /out --no-restore

RUN cp -r /out /publish
ENV ASPNETCORE_URLS http://*:5000
ENTRYPOINT ["dotnet", "/publish/FreeSSL.dll"]