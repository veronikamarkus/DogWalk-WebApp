# DogWalk WebApp

This project is a full-stack web application for dog walks bookings, it allows dog owners to find dog walkers and dog walkers to accept or decline dog walks, both can also track current walks and walks history. It includes a **.NET backend** and a **Next.js with React frontend**. The application allows users to manage dog walking activities, view profiles, and track walk offers.

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
