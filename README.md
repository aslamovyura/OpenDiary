# OpenDiary [Web-site]

![.NET Core]()

The main idea of the application is to develop web-based diary (like a blog) to create, read & comment posts. Each new user is invited to create his account and fill out information about himself. Then the user can write their thoughts and share them with other authors. The application also allows users to comment on each other's posts. 

## Getting Started

<Empty>

## Application settings

For the correct deploy, it is necessary to update the [appsettnigs.json](https://github.com/aslamovyura/coffee-meet-telegram-bot/tree/master/src/Bot/appsettings.json) file in the root directory of the web project, filled in according to the template below.

```
{
  "ConnectionStrings": {
      "DefaultConnection": "Server=databaseServer;Database=databaseName;User Id=userName; Password=userPassword;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*",
}
```

## Deployment Docker container on Heroku

To start the entire infrastructure, you should run the following commands from the project folder:

```
docker build -t application_name .
docker tag coffee-meet-bot registry.heroku.com/application_name/web
heroku container:push web -a application_name
heroku container:release web -a application_name
```

## Built with

- [ASP.NET Core 3.1](https://docs.microsoft.com/en-us/aspnet/core/);
- [Docker](https://www.docker.com/);
- [Heroku](https://heroku.com/)

## Author

[Yury Aslamov]();

## License

This project is under the MIT License - see the [LICENSE.md](https://github.com/aslamovyura/coffee-meet-telegram-bot/blob/master/LICENSE) file for details.
