# E-Commerce Angular Demo

This is a very small Angular app for demoing the ASP.NET Core API endpoints.

## Run

```powershell
npm install
npm start
```

Open:

```text
http://localhost:4200
```

With Docker Compose, run from the repository root:

```powershell
docker compose up --build
```

Then open:

```text
http://localhost:4200
```

The API and Swagger are available at:

```text
http://localhost:5112/swagger
```

The API base URL is currently:

```text
http://localhost:5112/api
```

Backend prerequisites:

- Redis on `localhost:6379`
- SQL Server using the API connection string
- `JWT_SECRET` environment variable
