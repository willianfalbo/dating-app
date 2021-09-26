## Dating App - Backend

The backend project was initially built using .NET Core 2 and it has been updated to DotNet Core 5.0


### Stack

-   DotNet Core 5.0 (C#)
-   [Clean Code Architecture / DDD](https://docs.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures#clean-architecture)
-   RESTFul APIs
-   SQL Server database & EF Core
-   JWT authentication
  - Slack Notifications
  - Redis Cache
-   Swagger


### Quick Start

1. Install [.NET Core 5](https://dotnet.microsoft.com/download) on your machine.

2. Run `docker-compose up --build` to spin up a new database. _\*\* You can skip this step if you don't want to use Docker._

    > **IMPORTANT:** Before running above command, make sure you have [docker](https://docs.docker.com/engine/install/) and [docker-compose](https://docs.docker.com/compose/install/) installed. \* It runs the **docker-compose.yml** file.

3. Make the proper changes in the configuration file "DatingApp.Api/appsettings.json".

    > For **uploading/downloading photos**, you must create an account in [Cloudinary](https://cloudinary.com/). Then, navigate to "Settings > Security Tab > Access Keys" menu, generate a new pair of key `ApiKey/ApiSecret`, and change your config file using the generated tokens.

    ```json
    "Cloudinary": {
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


### Slack Notifications

This project sends [Slack](https://slack.com/) notifications photos are approved/rejected.

To set up a new [Slack App](https://api.slack.com/apps) we need to follow the steps:

1. Create a [new app](https://api.slack.com/apps?new_granular_bot_app=1) at the API website.

2. Get a token to use the Web API. Navigate to **OAuth & Permissions** and scroll down to the section for scopes. Use the dropdown under the "Bot Token Scopes" header and add the scope `chat:write`.

3. Install the app to your workspace. Scroll up to the top of the page and click the "Install to Workspace" button. Go ahead and click "Allow", this will generate the token.

4. Copy the **Bot User OAuth Access Token** (it should begin with `xoxb`). Treat this value like a password and keep it safe.

5. Create a channel to send messages. Go to the **slack workspace** linked to your **app** and **create a new channel**.

6. Invite the app to the channel. Send a message like `/invite @your-app-name`. This will allow the **slack app** to send messages in the channel.

7. Replace the environment variables with the configurations created above.

    ```json
    "Slack": {
        "Token": "xoxb-YourTokenFromSlack",
        "BaseUrl": "https://slack.com/api",
        "Channels": {
            "RejectedPhotos": "#rejected-photos"
        }
    }
    ```

Reference: https://slack.dev/node-slack-sdk/getting-started


## Windows WSL (Windows Subsystem For Linux)

If you are using WSL, you might need to follow the steps below. Otherwise, your terminal may not find the EF installation. \* NOTE: Linux users might not face this problem.

-   `cd /home/yourUserName`

-   Edit the file `.zshrc` and add this line `export PATH="$PATH:$HOME/.dotnet/tools/"`.

-   Restart your terminal

References: [Stack Overflow - Cannot find command 'dotnet ef'](https://stackoverflow.com/questions/56862089/cannot-find-command-dotnet-ef)
