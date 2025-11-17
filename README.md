# Student API 🎓

This project is a simple **Web API** for managing student data, developed using **ASP.NET Core** and **SQL Server**.

## Features ✨

- View all students
- View only passed students
- Calculate grade average
- Search for a student using the ID
- Add a new student
- Update student data
- Delete a student

## Technical Details ⚙️

- **Stored Procedures** were used for all database operations.
- The layers are clearly separated:
  - `StudentAPI.Controllers`: The API layer.
  - `StudentAPIBusiness`: Business Logic.
  - `StudentAPIDataAccess`: Database access.
  - `StudentAPI.Shared`: Contains DTOs (Data Transfer Objects).

## API Routes 📡

| Method | Route                        | Description                |
| :----- | :--------------------------- | :------------------------- |
| GET    | `/api/Student/All`           | View all students          |
| GET    | `/api/Student/Passed`        | View only passed students  |
| GET    | `/api/Student/GradesAverage` | Calculate grade average    |
| GET    | `/api/Student/GetByID/{id}`  | Search for a student by ID |
| POST   | `/api/Student/Add`           | Add a new student          |
| PUT    | `/api/Student/Update/{id}`   | Update student data        |
| DELETE | `/api/Student/Delete/{id}`   | Delete a student           |

## Database 🛢️

The database used is **SQL Server**.

- All operations are performed via **Stored Procedures**.
