# OpenDiary

![.NET Core](https://github.com/aslamovyura/OpenDiary/workflows/.NET%20Core/badge.svg)

The application is a web diary for creating, reading and commenting on posts.

Each new user is invited to create his account and fill out information about himself. After that, user can write posts, share them or comment on posts of other authors.

## Getting Started

The application is free-to-use, so just login to start creating amaizing posts!

If you have problems completing the registration (heroku may have problems sending emails via smtp.gmail.com), you can use the following login credentials:
```
login: user@gmail.com
pass:  ctyjdfkbnh
```

## Application settings

For the correct deploy, it is necessary to update the [appsettings.json](https://github.com/aslamovyura/OpenDiary/blob/master/src/WebUI/appsettings.json) in the project WebUI directory according to the template below.

```
{
  "ConnectionStrings": {
      "DefaultConnection": "Server=databaseServer;Database=databaseName;User Id=sa; Password=userPassword;"
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

## Deployment to Heroku

After you install the Heroku CLI, run the following `login` commands:

```
heroku login
heroku container:login
```

For the application (HEROKU_APP) correct work, a database is required. To add PortgreSQL database on [Heroku](https://heroku.com/), use the following command:

```
heroku addons:create heroku-postgresql:hobby-dev --app:HEROKU_APP
```

To start the deployment to Heroku, run the following commands from the project folder:

```
docker build -t HEROKU_APP .
docker tag HEROKU_APP registry.heroku.com/HEROKU_APP/web
heroku container:push web -a HEROKU_APP
heroku container:release web -a HEROKU_APP
```

When deploying from Linux, use `sudo` for the commands above.

## Built with

- [ASP.NET Core 3.1](https://docs.microsoft.com/en-us/aspnet/core/),
- [Clean architecture](https://docs.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures),
- Deployment to [Heroku](https://heroku.com/) with support of [Docker](https://www.docker.com/),
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/),
- [CQRS](https://docs.microsoft.com/en-us/azure/architecture/patterns/cqrs) with [Fluent Validation](https://fluentvalidation.net/),
- [MediatR](https://github.com/jbogard/MediatR),
- [Automapper](https://automapper.org/),
- [MimeKit](http://www.mimekit.net/),
- [Health check](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-3.1),
- Unit tests with [xUnit](https://xunit.net/), [Moq](https://github.com/Moq/moq4/wiki/Quickstart) and [Shouldly](https://github.com/shouldly/shouldly)

## Author

[Yury Aslamov](https://aslamovyura.github.io/) - Software Developer, Ph.D.

## License

This project is under the MIT License - see the [LICENSE.md](https://github.com/aslamovyura/OpenDiary/blob/master/LICENSE) file for details.