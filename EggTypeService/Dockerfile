FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

COPY . ./
RUN dotnet publish EggTypeService -c Release -o out

#bovenstaande stuk is alleen om te builden
#hieronder pakken we een andere image om de app te packagen
#onderstaande (runtime) image is een stuk kleiner

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build-env /app/out .

COPY /EggTypeService/eggtypeservice-api.pfx /https/
#ENV vars voor ssl
ENV ASPNETCORE_ENVIRONMENT=Development \                                   
 ASPNETCORE_URLS="https://+:443;http://+:80" \
 ASPNETCORE_Kestrel__Certificates__Default__Password=Geheim123 \
 ASPNETCORE_Kestrel__Certificates__Default__Path="/https/eggtypeservice-api.pfx"

ENTRYPOINT [ "dotnet", "EggTypeService.dll" ]


#meer info: https://docs.docker.com/samples/dotnetcore/
