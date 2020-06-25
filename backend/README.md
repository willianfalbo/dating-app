# Dating App - Backend

The backend project was initially built using .NET Core 2 and it has been updated to .NET Core 3.0.

## Database

This project uses SQL Server as the database. For setting up a quick one, please run the docker-componse file as follow:

`npm run docker:database`

** Please make sure you have docker and docker-compose installed.

## Serve

Run `npm run start:dev` for serving the api.

Run `npm run start:watch` for watching changes and serving the api automatically.

## DB Migrations

This project uses Entity Framework Core for database migrations.

** From version 3.0, the .NET SDK does not include the EF tool so we should install it globally. [Breaking Changes - EF 3.0](https://docs.microsoft.com/en-gb/ef/core/what-is-new/ef-core-3.0/breaking-changes#the-ef-core-command-line-tool-dotnet-ef-is-no-longer-part-of-the-net-core-sdk)

`dotnet tool install --global dotnet-ef`

**Windows WSL (Windows Subsystem For Linux)**

If you are using WSL, you might need to follow the steps below. Otherwise, your terminal may not find the ef installation. * NOTE: Linux users might not face this problem.

- `cd /home/yourUserName`

- Edit the file `.zshrc` and add this line `export PATH="$PATH:$HOME/.dotnet/tools/"`.

- Restart your terminal

References: [Stack Overflow - Cannot find command 'dotnet ef'](https://stackoverflow.com/questions/56862089/cannot-find-command-dotnet-ef)

## Cloudinary

This project uses [Cloudinary](https://cloudinary.com/) for uploading/downloading photos. You could create an account (it is for free). 

On `Settings > Security Tab > Access Keys` you can generate a new pair of `ApiKey` and `ApiSecret`. Then, replace in `appsettings.json` file as follow.

```json
"CloudinarySettings": {
  "CloudName": "YourCloudName",
  "ApiKey": "YourApiKey",
  "ApiSecret": "YourApiSecret"
}
```
