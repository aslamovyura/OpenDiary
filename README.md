# OpenDiary

![.NET Core](https://github.com/aslamovyura/OpenDiary/workflows/.NET%20Core/badge.svg)

The main idea of the application is to develop web-based diary (like a blog) to create, read & comment posts. Each new user is invited to create his account and fill out information about himself. Then the user can write their posts and share them with other authors. The application also allows users to comment on each other's posts.

## Getting Started

The application is free-to-use, so just login to start creating amaizing posts!

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

## Add Heroku PorgreSQL database 

For the web-site correct work, a database is required. To add PortgreQSL database on [Heroku](https://heroku.com/), you should run the following command:

```
heroku addons:create heroku-postgresql:hobby-dev
```

## Deployment of Docker container on Heroku

To start the entire infrastructure, you should run the following commands from the project folder:

```
docker build -t application_name .
docker tag application_name registry.heroku.com/application_name/web
heroku container:push web -a application_name
heroku container:release web -a application_name
```

## Built with

- [ASP.NET Core 3.1](https://docs.microsoft.com/en-us/aspnet/core/)
- [Clean architecture](https://docs.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures)
- [Heroku](https://heroku.com/)
- [Docker](https://www.docker.com/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [Fluent Validation](https://fluentvalidation.net/)
- [CQRS](https://docs.microsoft.com/en-us/azure/architecture/patterns/cqrs)
- [MediatR](https://github.com/jbogard/MediatR)
- [Automapper](https://automapper.org/)
- [MimeKit](http://www.mimekit.net/)
- [Health check](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-3.1)
- [xUnit](https://xunit.net/)
- [Moq](https://github.com/Moq/moq4/wiki/Quickstart)
- [Shouldly](https://github.com/shouldly/shouldly)

## Author

[Yury Aslamov](https://aslamovyura.github.io/)

## License

This project is under the MIT License - see the [LICENSE.md](https://github.com/aslamovyura/OpenDiary/blob/master/LICENSE) file for details.
