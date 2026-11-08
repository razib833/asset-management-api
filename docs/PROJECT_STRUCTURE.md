# API project structure

The API uses feature-oriented folders for application contracts and services:

- `Controllers/`: HTTP endpoints.
- `Models/<Feature>/`: request, response, detail, and transport models grouped by feature.
- `Models/Common/`: shared API envelopes and stored-procedure result models.
- `Services/<Feature>/`: a feature's service and service interface kept together.
- `Extensions/`: dependency registration and application-pipeline extensions.
- `Utilities/`: reusable utility classes.
- `Authentication/`, `Configuration/`, `Middleware/`: cross-cutting infrastructure.
- `Data/`, `Repositories/`: stored-procedure execution infrastructure and database repositories.

The `Data` and `Repositories` folders are deliberately retained because all database operations follow the project's stored-procedure-only architecture. Moving files did not change public API routes, dependency registrations, or runtime behavior.
