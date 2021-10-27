# MicroserviceVoorbeeld

Gebaseerd op ".NET Microservices – Full Course" van Les Jackson (https://www.youtube.com/watch?v=DgVjEo3OGBI).

*Overzicht*

![alt text](https://github.com/Joost1982/MicroserviceVoorbeeld/blob/master/overzicht.png)

To do:

![alt text](https://github.com/Joost1982/MicroserviceVoorbeeld/blob/master/overzicht_rabbitMq.png)

*Endpoints*

Van EggType Service:
- **GET**		/api/eggtypes		
- **POST**		/api/eggtypes		(verwijst ook door naar "/api/f/eggtypes" van Flock Service)
- **GET** 		/api/eggtypes/{id}	

Voorbeeld EggType:
```json
{
	"Id": 1,
	"Description": "Omega-3 braun",
	"EggTypeGroupParameterCode": "0",
	"EggColorParameterCode": "1"
}
```

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

Voorbeeld Flock:
```json
{
    "Id": 3,
    "FlockCode": "1111-22",
    "Description": "Bennekom Stall",
    "EggType" : "Bio weiss/braun"
}
```

*Afwijkingen bovengenoemde cursus*

- Andere Models: Platform -> EggType en Command -> Flock
- Geen Kubernetes, maar Docker Compose
- De Flock Service is gebaseerd op Les Jacksons ".NET Core 3.1 MVC REST API - Full Course" (https://www.youtube.com/watch?v=fmvcAzHpsk8)
- ConnectionStrings niet in appsettings.json maar als env vars

*To do*

De Message Bus maken.


