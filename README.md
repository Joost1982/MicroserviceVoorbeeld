# MicroserviceVoorbeeld [Dapr]

Losjes gebaseerd op ".NET Microservices – Full Course" van Les Jackson (https://www.youtube.com/watch?v=DgVjEo3OGBI).

**[versie inclusief pub/sub, service invocation en state management componenten van Dapr]**
(<a href="https://github.com/Joost1982/MicroserviceVoorbeeld/tree/master">klik hier voor de versie zonder Dapr</a>)

Afwijkingen t.o.v. de cursus:
- Andere Models: Platform -> EggType en Command -> Flock en eentje extra: Product
- Geen Kubernetes, maar Docker Compose
- EggType Service gebruikt een MongoDb (de vergelijkbare service heeft in de tutorial een MS-SQL db)
- Flock Service gebruikt een MS-SQL database (de vergelijkbare service heeft in de tutorial een inMem db)
- De Flock Service is gebaseerd op Les Jacksons ".NET Core 3.1 MVC REST API - Full Course" (https://www.youtube.com/watch?v=fmvcAzHpsk8)
- ConnectionStrings niet in appsettings.json maar als env vars

Dapr toevoegingen:
- Pub/sub: code bevat geen enkele verwijzing meer naar RabbitMq (kan daardoor makkelijk vervangen worden door bijvoorbeeld Redis als dat nodig is)
- Service invocation: Flock Service bevat een endpoint (/api/f/products/{id}) waarin d.m.v. Dapr Service invocation een service van Product Service wordt aangeroepen via de eigen sidecar
- State management: bij een POST in de EggType Service wordt er een teller bijgehouden in een redis key-value database die uitgelezen kan worden via /api/eggtypes/state/bij . 
Ook hier geldt dat er in de code geen verwijzing is naar de (Redis) implementatie dankzij Dapr, waardoor switchen (naar bijvoorbeeld Cosmos Db) vrij makkelijk gaat.

*Overzicht*

<img src="https://github.com/Joost1982/MicroserviceVoorbeeld/blob/dapr/overzicht_rabbitMq.png" width="500">

Bij het opstarten van de Flock Service en de Product Service worden de op dat moment bekende EggTypes via gRPC binnengehaald vanuit de EggType Service.
Elk nieuw aangemaakte EggType in de EggType Service wordt gepublished naar de MessageBus en door de Flock Service en de Product Service opgepikt die ze daarna ook aanmaken in hun eigen databases.

*Models*

Voorbeeld EggType:
```json
{
	"Id": 1,
	"Description": "Omega-3 braun",
	"EggTypeGroupParameterCode": "0",
	"EggColorParameterCode": "1"
}
```

Voorbeeld Flock:
```json
{
    "Id": 1,
    "FlockCode": "1111-22",
    "Description": "Bennekom Stall",
    "EggType" : "Bio weiss/braun"
}
```

Voorbeeld Product:
```json
{
    "Id": 2,
    "ProductCode": "1112-3234 Eier braun",
    "isActive": "1",
    "EggType" : "Bio weiss/braun"
}
```

*Endpoints*

Van EggType Service:
- **GET**		/api/eggtypes	
- **GET** 		/api/eggtypes/{id}		
- **POST**		/api/eggtypes		(verwijst via pubsub door naar "/api/f/eggtypes" van Flock Service + houdt teller bij via state management in redis)
- **GET** 		/api/eggtypes/state/bij [voor Dapr State management test: endpoint haalt bovengenoemde teller op]	

Van Flock Service:
- **GET**		/api/flocks
- **POST**		/api/flocks
- **GET**		/api/flocks/{id}
- **PUT**		/api/flocks/{id}
- **PATCH**		/api/flocks/{id}
- **DELETE**	/api/flocks/{id}
- **GET** 		/api/f/eggtypes	 [toont de voor de Flock Service beschikbare EggTypes] 
- **POST** 		/api/f/eggtypes	 [wordt aangeroepen via pubsub vanuit "/api/eggtypes" POST van EggType Service]
- **GET** 		/api/f/eggtypes/{eggTypeId}/flocks	
- **GET** 		/api/f/eggtypes/{eggTypeId}/flocks/{flockId}
- **POST** 		/api/f/eggtypes/{eggTypeId}/flocks
- **GET**		/api/f/products/{id} [voor Dapr Service invocation voorbeeld: roept endpoint van Product Service aan via eigen sidecar]

Van Product Service:
- **GET**		/api/products
- **POST**		/api/products
- **GET**		/api/products/{id}
- **PUT**		/api/products/{id}
- **PATCH**		/api/products/{id}
- **DELETE**	/api/products/{id}
- **GET** 		/api/p/eggtypes	 [toont de voor de Product Service beschikbare EggTypes] 
- **POST** 		/api/p/eggtypes	 [wordt aangeroepen via pubsub vanuit "/api/eggtypes" POST van EggType Service]
- **GET** 		/api/p/eggtypes/{eggTypeId}/products	
- **GET** 		/api/p/eggtypes/{eggTypeId}/products/{productId}
- **POST** 		/api/p/eggtypes/{eggTypeId}/products
