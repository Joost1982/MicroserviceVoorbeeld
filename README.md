# MicroserviceVoorbeeld

Gebaseerd op ".NET Microservices – Full Course" van Les Jackson (https://www.youtube.com/watch?v=DgVjEo3OGBI).

*Endpoints*

Van Platform Service:
- **GET**		/api/platforms		
- **POST**		/api/platforms		(verwijst ook door naar "/api/c/platforms" van Command Service)
- **GET** 		/api/platforms/{id}	

Voorbeeld Platform:
```json
{
	"id": 1,
	"name": "Dot net",
	"publisher": "Microsoft",
	"cost": "Free"
}
```

Van Command Service:
- **GET**		/api/commands
- **GET**		/api/commands/{id}
- **PUT**		/api/commands/{id}
- **PATCH**		/api/commands/{id}
- **DELETE**	/api/commands/{id}
- **POST** 		/api/c/platforms	

Voorbeeld Command:
```json
{
    "id": 3,
    "howTo": "migrations doorvoeren",
    "line": "dotnet ef database update",
    "platform" : "Dot net"
}
```

*Afwijkingen bovengenoemde cursus*

- Geen Kubernetes, maar Docker Compose
- Voor de Commands Service heb ik geput uit Les Jacksons ".NET Core 3.1 MVC REST API - Full Course" (https://www.youtube.com/watch?v=fmvcAzHpsk8)
- MS-SQL server (nodig voor de door mij gebruikte versie van de Commands Service) draait in een aparte docker container 
- ConnectionStrings niet in appsettings.json maar als env vars

*Overzicht*

![alt text](https://github.com/Joost1982/MicroserviceVoorbeeld/blob/master/overzicht.png)
