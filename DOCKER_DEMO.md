# Docker Demo

Run the whole demo stack from the repository root:

```powershell
docker compose up --build
```

Open the Angular demo:

```text
http://localhost:4200
```

Open Swagger:

```text
http://localhost:5112/swagger
```

Services:

- Angular frontend: `http://localhost:4200`
- ASP.NET Core API: `http://localhost:5112`
- SQL Server: `localhost,1433`
- Redis: `localhost:6379`

The compose stack uses demo settings from `ECommerceApp/appsettings.Docker.json`.

To stop the stack:

```powershell
docker compose down
```

To remove the SQL Server demo data volume too:

```powershell
docker compose down -v
```
