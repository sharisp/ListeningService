# 🎧 Listening Episodes API

Welcome! This is a **Listening Episodes Management API** and **User Consumption API**, built with **.NET 8** and designed following **Domain-Driven Design (DDD)** principles.

---

## 🌐 Features

### 🔧 Manage API
- Admin-level access for creating, editing, deleting episodes.
- Control publishing status (published/unpublished).
- Follows clean architecture principles.

### 👂 User API
- Public or authenticated access to browse and listen to episodes.
- Optimized using **caching** for faster access.
- Only published content is exposed.

---

## ⚙️ Architecture Highlights

- ✅ **Caching**: For fast and efficient user-side access.
- ✅ **Unit of Work Pattern**:  
  - `POST`, `PUT`, `DELETE`: Auto-commit to DB  
  - `GET`: Manual control (read-only)
- ✅ **MediatR Integration**:  
  - Decoupled command/query & domain event handling
- ✅ **Snowflake ID Generation**:  
  - Ensures unique ID across users, roles, and permissions
Event Bus Integration (Planned)--Rabbitmq

---

## 📦 Special Dependencies

This service uses my **three NuGet packages** built for reusability and structure,you can find the source code from my github

- [`Andrew.Domain.SharedKernel`](https://www.nuget.org/packages/Andrew.Domain.SharedKernel)
- [`Andrew.Infrastructure.SharedKernel`](https://www.nuget.org/packages/Andrew.Infrastructure.SharedKernel)
- [`Andrew.CommonUse.JWT`](https://www.nuget.org/packages/Andrew.CommonUse.JWT)

---

## 🔒 Permission Control

Before uploading episodes or performing sensitive actions, permission validation is required.  
This integrates with a **centralized permission management system**:

👉 [Centralized Authority API](https://github.com/sharisp/Centralized.Authority)

---

## 🚀 Tech Stack

- ASP.NET Core 8
- EF Core
- MediatR
- JWT Auth
- restful Api
- SQL Server

---

---

## 📄 License

MIT © Andrew Wang
```
