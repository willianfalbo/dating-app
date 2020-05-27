# Dating App - Backend

This project was initially built using .NET Core 2 and it has been updated to .NET Core 3.0.

## Database

This project uses SQL Server as the database. For setting up a quick one, please run the docker-componse file as follow:

`npm run docker:database`

** Please make sure you have docker and docker-compose installed.

## Serve

Run `npm run start` for serving the api.

Run `npm run watch` for watching changes and serving the api automatically.

## DB Migrations

This project uses Entity Framework Core for database migrations.

** From version 3.0, the .NET SDK does not include the EF tool so we should install it globally. [Breaking Changes - EF 3.0](https://docs.microsoft.com/en-gb/ef/core/what-is-new/ef-core-3.0/breaking-changes#the-ef-core-command-line-tool-dotnet-ef-is-no-longer-part-of-the-net-core-sdk)

`dotnet tool install --global dotnet-ef --version 3.0.0`
