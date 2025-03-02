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

// RoleId for Admin
76a0cbcd-3592-40c8-9439-1ff9d936ac39

// UserId for Admin
dd3dff8a-7a83-439a-8d1a-37d56f116a0b

## Docker

~~~bash
docker buildx build --progress=plain --force-rm --push -t akaver/webapp:latest . 

# multiplatform build on apple silicon
# https://docs.docker.com/build/building/multi-platform/
docker buildx create --name mybuilder --bootstrap --use
dock


~~~