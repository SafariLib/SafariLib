<!-- markdownlint-disable-next-line -->
<p align="center">
    <img width="200" src="https://raw.githubusercontent.com/SafariLib/.github/main/assets/logo.png" alt="SafariUI logo">
</p>
<h1 align="center">SafariLib</h1>
<div align="center">
    <a href="https://github.com/SafariLib"><img src="https://img.shields.io/badge/SafariLib-yellow.svg"></a>
    <a href="./LICENSE"><img src="https://img.shields.io/badge/license-MIT-blue.svg"></a>
    <a href="https://dotnet.microsoft.com/en-us/"><img src="https://img.shields.io/badge/.net_8.0-blue.svg"></a>
</div>

<p align="center">
    <em>The SafariLib project is a collection of helpers for build Web Applications using .Net</em>
</p>


---
![license](https://img.shields.io/badge/Core-1.0.0-yellow.svg)

Core library containing base classes of the SafariLib. Part of the SafariLib project.
```bash
dotnet add package SafariLib.Core
```
---
![license](https://img.shields.io/badge/Repositories-1.0.0-yellow.svg)

Entity Framework Repository overload that handles validation. Part of the SafariLib project.
#### Installation
```bash
dotnet add package Microsoft.EntityFrameworkCore --version 8.0.4
dotnet add package SafariLib.Repositories
```
#### Usage
Inject the repository and repository service into the service collection.
```csharp
// Inject the repository and repository service into the service collection.
services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
services.AddScoped(typeof(IRepositoryService<SomeModel>), typeof(RepositoryService<SomeContext, SomeModel>));

// Call the repository service in the controller.
public class SomeController : ControllerBase
{
    private readonly IRepositoryService<SomeModel> _repositoryService;

    public SomeController(IRepositoryService<SomeModel> repositoryService)
    {
        _repositoryService = repositoryService;
    }
}
```

---
![license](https://img.shields.io/badge/Jwt-1.0.0-yellow.svg)

AspNetCore JwtBearer overload that handles Token caching and revoke. Part of the SafariLib project.
#### Installation
```bash
dotnet add package SafariLib.Jwt
```
#### Usage
Inject the JwtService into the service collection.
```csharp
// Inject the JwtService into the service collection.
services
    .AddMemoryCache()
    .AddJwtService()
    .AddJwtCacheService()
    .AddJwtSwagger();
```
Use _appsettings_ to configure the JwtService:

|Name|Type|
|---|---|
| Jwt:RefreshExpiration | **long**  |
| Jwt:BearerExpiration | **long**  |
| Jwt:Secret | **string?** |
| Jwt:Issuer | **string?** |
| Jwt:Audience | **string?** |
| Jwt:CookieName | **string?** |
| Jwt:MaxTokenAllowed | **int?** |
