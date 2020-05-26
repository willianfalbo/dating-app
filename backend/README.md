# Dating App - Backend

## Description

This project was initially built using .NetCore 2 and upgraded to .NetCore 3.

## Database

This project uses SQL Server as the database. For setting up a quick one, please run the docker-componse file as follow:

`npm run docker:database`

** Please make sure you have docker/docker compose installed.

## Serve

Run `npm run start` for serving the api.

Run `npm run watch` for watching changes and serving the api automatically.

## DB Migrations

From version 3.0, the .NET SDK does not include the dotnet ef tool so we should install Entity Framework globally. [Breaking Changes - EF 3.0](https://docs.microsoft.com/en-gb/ef/core/what-is-new/ef-core-3.0/breaking-changes#the-ef-core-command-line-tool-dotnet-ef-is-no-longer-part-of-the-net-core-sdk)

`dotnet tool install --global dotnet-ef --version 3.0.0`
