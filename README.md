# MTG Dotnet Project

A Magic: The Gathering companion web application built with Blazor Server and .NET. The project allows users to browse and search Magic: The Gathering cards, manage filters, and interact with a PostgreSQL backed database through Entity Framework Core.

## Overview

This is a group project built using the Blazor Server hosting model with interactive server side components. The application is structured around a service layer that handles card data, authentication, and persisted user filter preferences.

## Features

- Interactive server side Blazor components for a responsive user interface
- Card browsing and search functionality through a dedicated card service
- User authentication service
- Filter preferences persisted using browser local storage
- PostgreSQL database integration through Entity Framework Core
- Configurable environment settings for development and production

## Tech Stack

- **Framework:** ASP.NET Core (.NET), Blazor Server with Interactive Server Components
- **Database:** PostgreSQL, accessed through Entity Framework Core using the Npgsql provider
- **Frontend:** Razor Components, HTML, CSS
- **Local Storage:** Blazored.LocalStorage
- **Language Breakdown:** HTML, C#, CSS

## Prerequisites

Before running this project, make sure you have the following installed:

- .NET SDK (version compatible with the project target framework)
- PostgreSQL server
- A code editor such as Visual Studio, Visual Studio Code, or JetBrains Rider

## Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/CalinNicolae/mtg-dotnet-project.git
cd mtg-dotnet-project
```

### 2. Configure the database connection

Update the `DefaultConnection` connection string in `appsettings.json` or `appsettings.Development.json` with your PostgreSQL credentials.

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=mtgproject;Username=your_username;Password=your_password"
  }
}
```

### 3. Apply database migrations

If Entity Framework Core migrations are present in the project, apply them with:

```bash
dotnet ef database update
```

### 4. Restore dependencies

```bash
dotnet restore
```

### 5. Run the application

```bash
dotnet run
```

The application will start and can be accessed at the address shown in the console output, typically `https://localhost:5001` or a similar local URL.

## Configuration

The application uses the standard ASP.NET Core configuration system. Key settings are stored in:

- `appsettings.json` for shared configuration values
- `appsettings.Development.json` for development environment overrides

Sensitive values, such as database credentials, should not be committed to source control. Consider using environment variables or the .NET user secrets tool for local development.

```bash
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "your_connection_string"
```

## Services

| Service | Responsibility |
|---|---|
| CardService | Retrieves and manages Magic: The Gathering card data |
| AuthService | Handles user authentication |
| FilterLocalStorageService | Persists user filter preferences in browser local storage |

## License

This was a group project done for the Howest course called .NET Technology Fundamentals.

## Acknowledgments

This project was developed as a group assignment focused on building a full stack application using Blazor and .NET technologies within the Magic: The Gathering domain.

The contributors to this project are:

Eti Chisom Peter

Suh Conrad
