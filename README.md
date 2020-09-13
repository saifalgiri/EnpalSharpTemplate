# Enpal C# Template

This project provide a template based on ASP NET Core 3.x to extend the global search by an own written search skill. 


# Getting Started

## Installation of Tools

To follow the tutorial you should have following tools or similiar tools installed (based on Windows PC) : 

- [Visual Studio Community Edition](https://visualstudio.microsoft.com/de/downloads/) 
- [Git for Windows](https://gitforwindows.org/)
- [Tortoise Git](https://tortoisegit.org/)

Follow these steps : 
- Clone the repository
- Open the project in visual studio
- run the project

A Swagger page should be openend where you can directly try out some calls using basic authorization as listed 
later in this document.


## Swagger (Swashbuckle)

[Swashbuckle](https://docs.microsoft.com/de-de/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-3.1&tabs=visual-studio) for Asp Net Core provide 

Swashbuckle can be used by installing the following nuget packages : 

- Swashbuckle.AspNetCore
- Swashbuckle.AspNetCore.Annotations
- YamlDotNet


## Authorization

The template support two authorization methods : 

- Enpal Identity Server (OpenIDConnect/OAuth)
- Basic Authorization

By default the "Enpal Identity Server" option is activated. Basic Authorization is 
commented out.

### Identity Server Authorization

The template is configured to work against the test identity server of enpal. 

The respective configuration is done inside appsettings.json
````json
 "IdentityServer": {
    "BaseUrl": "https://is4enpal-dev.azurewebsites.net",
    "Audience": "templateApi"
  }, 
````

Inside startup.cs the Authtentication Mode is defined.

````csharp
services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = Configuration["IdentityServer:BaseUrl"];
        options.RequireHttpsMetadata = true;

        options.Audience = Configuration["IdentityServer:Audience"];
    });
````


#### How to get an JWT Token for Test?

The project also include an project called "tokenGet". This is a simple comand line tool which can be directly executed.
It will request an token and print it to shell like : 

````json
{
  "access_token": "eyJhbGciOiJSUzI1NiIsImtpZCI6ImR0Yk96MGFjLVNOMnI3bWF6VHQwN0EiLCJ0eXAiOiJhdCtqd3QifQ.eyJuYmYiOjE1OTE1MzE5NTgsImV4cCI6MTU5MTUzNTU1OCwiaXNzIjoiaHR0cHM6Ly9pczRlbnBhbC1kZXYuYXp1cmV3ZWJzaXRlcy5uZXQiLCJhdWQiOiJ0ZW1wbGF0ZUFwaSIsImNsaWVudF9pZCI6ImNsaWVudCIsInNjb3BlIjpbInRlbXBsYXRlQXBpIl19.Vnwcg4US2XnTkS0w5qt0lCRicM-9DLN_lpUONCHzIi744SqtUm7xKx32JfuPFkzPzmhl5TiaCkidUCeQspo_CQTgdVaToocC0o6Yt5DIuVsE-j_z2aIYfRBkTyySVrlAMmEI6TVff-bPpdm0d75B4HDeAJ7xuYY2dJgKxPmHFrfc5pdJ9Io1XE8Uo4liHMIYAvIMO1sWITFkysaL0Mejy9IFKdrmNQICssm4Ltsz_pmgumpJwU0Df_QzanfMtermlQ7pCD71w_l-tauvVrBh7vyrXtkcbBDQQFDF12B1rMgemRRmSpkF72sm_0uhc8pkNwgdKEj_bK9JkcrdLz-Q7w",
  "expires_in": 3600,
  "token_type": "Bearer",
  "scope": "templateApi"
}
````




### Basic Authorization

The basic Authroization is disabled by default. Inside the startup.cs file one has 
to uncomment the corresponding parts and comment out the bearer authorization.
Do not forget the swashbuckle settings!

When basic auth is active, then follwoing credentials can be used for test : 

**UserName:** Test <br/>  **Passwort:** Test

This must be changed inside your final code.

## API Versioning

The template support API versioning. It follow this [Article](https://dev.to/99darshan/restful-web-api-versioning-with-asp-net-core-1e8g)
Adjust Swashbuckle is following mainly this [Article](https://codingfreaks.de/dotnet-core-api-versioning/)

The template contain a sample for a versioned controller. Delete them as you do not need them. 
The Version 1.0 of this template is set to deprecated to show you this handling. 
Also each request provide as return a list of all supported versions and also if they are deprecated.

````json
 api-deprecated-versions: 1.0 
 api-supported-versions: 1.1 
 content-type: application/json 
 date: Wed, 27 May 2020 06:11:15 GMT 
 request-context: appId= 
 server: Microsoft-IIS/10.0 
 status: 200 
 x-powered-by: ASP.NET 
````




## Application Insights

The template support logging into application insights from scratch. The settings (e.g. Instrumentation key) can be done 
inside "appsettings.json"

````json
"ApplicationInsights": {
    "Region": "ECE",
    "InstrumentationKey": "place your key here",
    "TelemetryChannel": {
      "EndpointAddress": "https://dc.applicationinsights.azure.cn/v2/track"
    }
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
````

# Build and Test


