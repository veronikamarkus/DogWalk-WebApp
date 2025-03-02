## Useful commands in .net console CLI

Install tooling

~~~bash
dotnet tool update -g dotnet-ef
dotnet tool update -g dotnet-aspnet-codegenerator 
~~~

## EF Core migrations

Run from solution folder

~~~bash
dotnet ef migrations --project App.DAL.EF --startup-project WebApp add FOOBAR
dotnet ef database   --project App.DAL.EF --startup-project WebApp update
dotnet ef database   --project App.DAL.EF --startup-project WebApp drop
~~~


## MVC controllers

Install from nuget:
- Microsoft.VisualStudio.Web.CodeGeneration.Design
- Microsoft.EntityFrameworkCore.SqlServer


Run from WebApp folder!

~~~bash
cd WebApp

dotnet aspnet-codegenerator controller -name DogsController        -actions -m  App.Domain.Dog        -dc AppDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
# use area
dotnet aspnet-codegenerator controller -name ProfilesController        -actions -m  App.Domain.Profile        -dc AppDbContext -outDir Areas\Admin\Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f

cd ..
~~~

Api controllers
~~~bash
dotnet aspnet-codegenerator controller -name WalkOffersController  -m  App.Domain.WalkOffer       -dc AppDbContext -outDir ApiControllers -api --useAsyncActions -f
~~~
