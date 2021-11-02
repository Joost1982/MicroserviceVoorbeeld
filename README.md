# MicroserviceVoorbeeld [Dapr]

Losjes gebaseerd op ".NET Microservices – Full Course" van Les Jackson (https://www.youtube.com/watch?v=DgVjEo3OGBI).

**[versie inclusief pub/sub component van Dapr]**
(<a href="https://github.com/Joost1982/MicroserviceVoorbeeld/tree/master">klik hier voor de versie zonder Dapr</a>)

Afwijkingen t.o.v. de cursus:
- Andere Models: Platform -> EggType en Command -> Flock en eentje extra: Product
- Geen Kubernetes, maar Docker Compose
- EggType Service gebruikt een MongoDb (de vergelijkbare service heeft in de tutorial een MS-SQL db)
- Flock Service gebruikt een MS-SQL database (de vergelijkbare service heeft in de tutorial een inMem db)
- De Flock Service is gebaseerd op Les Jacksons ".NET Core 3.1 MVC REST API - Full Course" (https://www.youtube.com/watch?v=fmvcAzHpsk8)
- ConnectionStrings niet in appsettings.json maar als env vars
- **deze versie maakt gebruikt van Dapr waardoor er in de code geen enkele verwijzing meer naar RabbitMq nodig is (en daardoor makkelijk vervangen kan worden door bijvoorbeeld Redis als dat nodig is) **

*Overzicht*

<img src="https://github.com/Joost1982/MicroserviceVoorbeeld/blob/master/overzicht_rabbitMq.png" width="500">

Bij het opstarten van de Flock Service en de Product Service worden de op dat moment bekende EggTypes via gRPC binnengehaald vanuit de EggType Service.
Elk nieuw aangemaakte EggType in de EggType Service wordt gepublished naar de MessageBus en via listeners opgepikt door de Flock Service en de Product Service die ze daarna ook aanmaken in hun eigen databases.

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
- **POST**		/api/eggtypes		(verwijst via pubsub door naar "/api/f/eggtypes" van Flock Service)

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
