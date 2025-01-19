# Goalify Application

Goalify is a modern web application built with **.NET** and **React** to help users set, track, and achieve their goals effectively.

---

## 🛠️ Technologies Used

- **Backend**: .NET Core, Entity Framework Core
- **Frontend**: React.js
- **Database**: PostgreSQL

---

## 🚀 Getting Started

Follow these steps to set up and run the application on your local machine.

### Prerequisites

Make sure you have the following installed:
- [.NET SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/)
- PostgreSQL (ensure your database server is running)

---

### 🔧 Backend Setup

1. Navigate to the backend project directory:
   ```bash
   cd Backend_Goalify.Infrastructure
   ```
2. Apply database migrations:
   ```
   dotnet ef database update --startup-project ../Backend_Goalify.API
   ```
3. Add a new migration (if needed):
   ```
   dotnet ef migrations add MigrationName --startup-project ../Backend_Goalify.API
   ```
4. Run the backend server:
   ```
   dotnet watch run
   ```
