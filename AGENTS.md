# Project context for AI assistants

## About me
- Junior full-stack developer (JS/TS background: React, Vue, Node/Express, PostgreSQL/Supabase). **New to C#** - explain simply, step by step, and compare with JS when helpful.
- Please reply in **Thai**.
- This is a resume/portfolio project: keep it short, clean, and easy to understand. Don't add features or packages beyond the current phase unless I ask.

## Stack
- Monorepo: `client/` (React + Vite + TypeScript, not Next.js) and `server/` (ASP.NET Core **.NET 10** Web API with Controllers, namespace `Server`).
- EF Core 10 + SQL Server (Code-first, migrations). Local dev: SQL Server Express `localhost\SQLEXPRESS`, Windows Auth, database `ChatDb`. Production: Azure SQL Database (free offer, auto-pause -> cold start).
- Auth: JWT (BCrypt.Net-Next for password hashing, Microsoft.AspNetCore.Authentication.JwtBearer).
- Real-time: SignalR.
- Deploy: client on Vercel, API on Azure App Service (WebSockets on). CORS must allow the Vercel domain and `http://localhost:5173` with AllowCredentials.

## Data model (server/Models, server/Data/AppDbContext.cs)
- User: Id, Username (unique), DisplayName, Email (unique), PasswordHash, PhotoUrl (nullable, used in phase 2), CreatedAt
- Room: Id, Name (unique), CreatedById (FK -> User.Id), CreatedBy, CreatedAt, Messages, Members
- RoomMember: composite key (RoomId, UserId), JoinedAt. Join table User <-> Room (many-to-many). The sidebar shows only rooms the user joined.
- Message: Id, RoomId, UserId, Content, SentAt
- Delete behavior: Room -> Members cascade; RoomMember.User, Room.CreatedBy, Message.User are Restrict (avoid SQL Server multiple cascade paths).

## Conventions
- Use `.Select(...)` / DTOs in API responses. Never return `PasswordHash` or full entity graphs.
- Use `async` EF calls (`ToListAsync`, `FirstOrDefaultAsync`).
- Connection string key: `ConnectionStrings:Default`. Don't commit secrets (JWT key, Azure connection string).
- Git: `main` stays working; one short feature branch per task (`feature/db-setup`, `feature/auth`, `feature/rooms`, `feature/chat`, `feature/deploy`), merge back to main.

## Phases
- **Phase 1 (current):** EF Core + SQL Server, JWT register/login (with DisplayName), profile with initials avatar, create/join public rooms (RoomMember), SignalR chat + React UI, deploy (Vercel / App Service / Azure SQL).
- **Phase 2:** file/image attachments + profile photo upload (Azure Blob Storage), private rooms with invite (Room.IsPrivate, `GET /api/users/search?q=`, `POST /api/rooms/{id}/members`), polls in MongoDB, Redis (online users + SignalR backplane).
- **Phase 3:** rate limiting, Dockerfile + Docker Compose (API, SQL, Redis).

## Current status
- Part 1 (Models, AppDbContext) in progress on branch `feature/db-setup`.
- Still to do in Part 1: delete WeatherForecast.cs + WeatherForecastController.cs, add connection string to appsettings.Development.json, register AddDbContext in Program.cs, `dotnet build`, `dotnet ef migrations add Init`, `dotnet ef database update`.
- Next: Part 2 = Register/Login with JWT.
