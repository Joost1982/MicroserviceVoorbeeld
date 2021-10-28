# MicroserviceVoorbeeld

Losjes gebaseerd op ".NET Microservices – Full Course" van Les Jackson (https://www.youtube.com/watch?v=DgVjEo3OGBI).

Afwijkingen t.o.v. de cursus:
- Andere Models: Platform -> EggType en Command -> Flock en eentje extra: Product
- Geen Kubernetes, maar Docker Compose
- EggType Service gebruikt een MongoDb (de vergelijkbare service heeft in de tutorial een MS-SQL db)
- Flock Service gebruikt een MS-SQL database (de vergelijkbare service heeft in de tutorial een inMem db)
- De Flock Service is gebaseerd op Les Jacksons ".NET Core 3.1 MVC REST API - Full Course" (https://www.youtube.com/watch?v=fmvcAzHpsk8)
- ConnectionStrings niet in appsettings.json maar als env vars

*Overzicht*

<img src="https://github.com/Joost1982/MicroserviceVoorbeeld/blob/master/overzicht_rabbitMq.png" width="500">

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
- **POST**		/api/eggtypes		(verwijst ook door naar "/api/f/eggtypes" van Flock Service)
- **GET** 		/api/eggtypes/{id}	

Van Flock Service:
- **GET**		/api/flocks
- **GET**		/api/flocks/{id}
- **PUT**		/api/flocks/{id}
- **PATCH**		/api/flocks/{id}
- **DELETE**	/api/flocks/{id}
- **POST** 		/api/f/eggtypes	
- **GET** 		/api/f/eggtypes/{eggTypeId}/flocks	
- **GET** 		/api/f/eggtypes/{eggTypeId}/flocks/{flockId}
- **POST** 		/api/f/eggtypes/{eggTypeId}/flocks

Van Product Service:
- ...
- ...
