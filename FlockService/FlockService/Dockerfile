FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build-env
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

#bovenstaande stuk is alleen om te builden
#hieronder pakken we een andere image om de app te packagen
#onderstaande (runtime) image is een stuk kleiner

FROM mcr.microsoft.com/dotnet/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT [ "dotnet", "FlockService.dll" ]


#meer info: https://docs.docker.com/samples/dotnetcore/
