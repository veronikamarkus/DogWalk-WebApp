# DogWalk WebApp

This project is a full-stack web application designed for booking dog walks. It enables dog owners to find and book dog walkers, while allowing dog walkers to accept or decline walk requests. Both owners and walkers can track current and past walks.

The application features a .NET backend and a Next.js with React frontend. Users can manage their dog walking activities, view profiles, and track walk offers. It supports three user roles: dog owner, dog walker, and admin. Security is handled using JWT tokens.

## Project Structure

- **/DogWalk-backend**: Contains the .NET API for the backend.
- **/DogWalk-frontend**: Contains the Next.js React frontend for the web app.

## Prerequisites

Before running the project, ensure you have the following installed:

- **[JetBrains Rider](https://www.jetbrains.com/rider/)** for running the **.NET backend**.
- **[Visual Studio Code](https://code.visualstudio.com/)** for running the **Next.js frontend**.
- **Docker** (optional, for containerization of the backend and frontend).

---

## Running the Application

### 1. Backend Setup (Rider)

For the **.NET backend**, follow these steps in **JetBrains Rider**:

1. Navigate to the `/DogWalk-backend` directory.
2. Follow the instructions in the [Backend README](./DogWalk-backend/README.md) to set up and run the backend using Rider.

### 2. Frontend Setup (VSCode)

For the **Next.js frontend**, follow these steps in **Visual Studio Code (VSCode)**:

1. Navigate to the `/DogWalk-frontend` directory.
2. Follow the instructions in the [Frontend README](./DogWalk-frontend/README.md) to set up and run the frontend using VSCode.

---

## Useful Commands for Backend (.NET CLI)

### Install Tooling
To update .NET tools, run the following commands in the terminal:

```bash
dotnet tool update -g dotnet-ef
dotnet tool update -g dotnet-aspnet-codegenerator
```

### EF Core Migrations 

```bash
dotnet ef migrations --project App.DAL.EF --startup-project WebApp add FOOBAR
dotnet ef database --project App.DAL.EF --startup-project WebApp update
dotnet ef database --project App.DAL.EF --startup-project WebApp drop
```

### MVC Controllers

To generate controllers, use the following commands (run from the WebApp folder):

```bash
cd WebApp

dotnet aspnet-codegenerator controller -name DogsController -actions -m App.Domain.Dog -dc AppDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
# For areas (e.g., Admin):
dotnet aspnet-codegenerator controller -name ProfilesController -actions -m App.Domain.Profile -dc AppDbContext -outDir Areas\Admin\Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f

cd ..

```

### API Controllers

To generate API controllers:

```bash
dotnet aspnet-codegenerator controller -name WalkOffersController -m App.Domain.WalkOffer -dc AppDbContext -outDir ApiControllers -api --useAsyncActions -f

```
