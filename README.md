# Task Management Application

This is a test exercise by [Software Engineer Daniel Silva Jiménez](https://www.linkedin.com/in/daniel-silva-jim%C3%A9nez-12871653) for the Lateral Group evaluation.

Objective:
Build a small task management API and frontend that supports creating, viewing, and toggling
the status of tasks.

## Technologies

**Backend:**
- .NET 8
- EF Core In-Memory Database
- Clean Architecture (4 layers)
- AutoMapper
- xUnit for testing

**Frontend:**
- Vue.js 3
- Vite
- Pinia (State Management)
- Axios

## Architecture

### Backend Layers
- **Domain**: Entities and business logic
- **Infrastructure**: Data access and repositories
- **Application**: Business services and DTOs
- **API**: REST endpoints

### Key Design Decisions
- Used In-Memory database for simplicity (easy to swap for SQL)
- Repository pattern for data abstraction
- DTOs to separate API contracts from domain models
- Factory methods for domain validation

## Setup & Run

### Backend
```bash
cd backend
dotnet restore
dotnet build
cd TaskManagement.API
dotnet run
```
API runs on: https://localhost:7xxx

### Frontend
```bash
cd frontend
npm install
npm run dev
```
App runs on: http://localhost:5173

## API Endpoints
- GET /api/tasks - Get all tasks
- GET /api/tasks/{id} - Get task by ID
- GET /api/tasks/status/{status} - Get tasks by status (0=Todo, 1=InProgress, 2=Done)
- POST /api/tasks - Create task
- PUT /api/tasks/{id} - Update task
- DELETE /api/tasks/{id} - Delete task

## Features
- ✅ Create, read, update, delete tasks
- ✅ Change task status (Todo, In Progress, Done)
- ✅ Filter tasks by status
- ✅ Real-time statistics
- ✅ Form validation
- ✅ Responsive design

## Testing
```bash
cd backend
dotnet test
```