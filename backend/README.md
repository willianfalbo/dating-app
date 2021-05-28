## Dating App - Backend

The backend project was initially built using .NET Core 2 and it has been updated to DotNet Core 5.0

### Stack

-   DotNet Core 5.0 (C#)
-   [Clean Code Architecture / DDD](https://docs.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures#clean-architecture)
-   RESTFul APIs
-   SQL Server database & EF Core
-   JWT authentication

### Quick Start

1. Install [.NET Core 5](https://dotnet.microsoft.com/download) on your machine.

2. Run `docker-compose up --build` to spin up a new database. _\*\* You can skip this step if you don't want to use Docker._

    > **IMPORTANT:** Before running above command, make sure you have [docker](https://docs.docker.com/engine/install/) and [docker-compose](https://docs.docker.com/compose/install/) installed. \* It runs the **docker-compose.yml** file.

3. Make the proper changes in the configuration file "DatingApp.Api/appsettings.json".

    > For **uploading/downloading photos**, you must create an account in [Cloudinary](https://cloudinary.com/). Then, navigate to "Settings > Security Tab > Access Keys" menu, generate a new pair of key `ApiKey/ApiSecret`, and change your config file using the generated tokens.

    ```json
    "CloudinarySettings": {
      "CloudName": "YourCloudName",
      "ApiKey": "YourApiKey",
      "ApiSecret": "YourApiSecret"
    }
    ```

4. Run `npm run start` for starting the server.

### DB Migrations

This project uses Entity Framework Core for database migrations.

> From version 3.0, the .NET SDK does not include the EF tool so we should install it globally. [Breaking Changes - EF 3.0](https://docs.microsoft.com/en-gb/ef/core/what-is-new/ef-core-3.0/breaking-changes#the-ef-core-command-line-tool-dotnet-ef-is-no-longer-part-of-the-net-core-sdk)

`dotnet tool install --global dotnet-ef`

**Windows WSL (Windows Subsystem For Linux)**

If you are using WSL, you might need to follow the steps below. Otherwise, your terminal may not find the EF installation. \* NOTE: Linux users might not face this problem.

-   `cd /home/yourUserName`

-   Edit the file `.zshrc` and add this line `export PATH="$PATH:$HOME/.dotnet/tools/"`.

-   Restart your terminal

References: [Stack Overflow - Cannot find command 'dotnet ef'](https://stackoverflow.com/questions/56862089/cannot-find-command-dotnet-ef)
