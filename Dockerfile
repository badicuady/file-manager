FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS packages

WORKDIR /app

# Auth with private feed
RUN wget https://raw.githubusercontent.com/Microsoft/artifacts-credprovider/master/helpers/installcredprovider.sh \
   && chmod +x installcredprovider.sh \
   && ./installcredprovider.sh

COPY ["./FileManager.Api.sln", "./"]

COPY ./*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ${file%.*}/ && mv $file ${file%.*}/; done

RUN dotnet restore -s https://api.nuget.org/v3/index.json

FROM packages as build

COPY ./ ./

RUN dotnet build -c Release --no-restore .

FROM build AS publish

RUN dotnet publish "./FileManager.Api/FileManager.Api.csproj" \
  --no-dependencies \
  --no-restore \
  --no-build \
  --framework net5.0 \
  -c Release \
  -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine AS base
WORKDIR /app

# https://www.stevejgordon.co.uk/timezonenotfoundexception-in-alpine-based-docker-images
# if you have timezone issues with the above alpine image: then you can uncomment the line below to add the tzdata package
# Required for Time Zone database lookups
RUN apk add --no-cache tzdata
RUN apk add icu

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

FROM base AS final
WORKDIR /app
RUN mkdir dir
EXPOSE 80
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT Docker

ENTRYPOINT ["dotnet", "FileManager.Api.dll"]


