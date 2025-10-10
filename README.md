# 🛍️ E-Commerce Web Application

[![.NET](https://img.shields.io/badge/.NET-8.0-purple?logo=dotnet)](https://dotnet.microsoft.com/)
[![Build Status](https://img.shields.io/badge/build-passing-brightgreen)](#)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![Database](https://img.shields.io/badge/Database-SQL%20Server-red?logo=microsoft-sql-server)](#)

A full-stack **E-Commerce Web Application** built using **ASP.NET Core 8**, **Entity Framework Core**, and **SQL Server**.  
It provides a robust backend for user authentication, product management, shopping cart operations, and payment preparation — following clean architecture and best backend design practices.

---

## 🚀 Features

- 🔐 **User Authentication & Authorization** — Secure login and registration using **ASP.NET Identity** and **JWT** tokens.  
- 🛒 **Shopping Cart Management** — Automatic cart creation on user registration, with add, update, and delete operations.  
- 💰 **Stock Control** — Product quantity automatically adjusted upon adding/removing cart items.  
- 🔎 **Search & Filtering** — Case-insensitive search and price-based product filtering.  
- 🔁 **Transactional Safety** — Database operations are wrapped in transactions to ensure data integrity.  
- 🧾 **Audit Tracking** — Every entity tracks creation and modification history (`CreatedAt`, `CreatedBy`, etc.).  
- 💳 **Payment-Ready Design** — Structured for easy integration with gateways like **Paymob**, **Stripe**, or **Fawry**.  
- 🧱 **Clean Architecture** — Modular design with **Repository**, **Unit of Work**, and **Service** layers.  

---

## 💡 Benefits

- Highly extensible and maintainable backend.  
- Clean separation of concerns for scalable development.  
- Production-grade transaction handling.  
- Ideal foundation for integration with React, Angular, or Blazor frontends.  
- Demonstrates real-world backend development skills for portfolios or interviews.

---
### 🧩 Prerequisites

Ensure you have the following installed:

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)  
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)  
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)  
---
