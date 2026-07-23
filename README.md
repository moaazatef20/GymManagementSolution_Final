# 🏋️‍♂️ Power Fitness (GymCore) - Gym Management System

A comprehensive, real-world Gym Management System built with **ASP.NET Core MVC (.NET 10)** utilizing a clean **N-Tier Architecture**. This application serves as a single source of truth for gym owners to manage their entire business seamlessly—replacing scattered Excel sheets and paper logs with one powerful dashboard.

## 🌟 Live Demo & Test Credentials

You can test the live application here: **[https://powerfitness.ddns.net/]**

To explore the system's full capabilities, please use the following seeded admin credentials on the **Login Page**:
- **Email:** `moaazatef2020@gmail.com`
- **Password:** `P@ssw0rd`

*(Note: The database is seeded with sample data for testing purposes. Public account creation is intentionally disabled as this is an internal management system.)*

## 🏗️ Architecture & Technologies

- **Backend:** ASP.NET Core MVC (.NET 10), C#
- **Architecture:** N-Tier Architecture (Presentation, BLL, DAL)
- **Database:** SQL Server (mssql-server on Linux) with Entity Framework Core
- **Frontend:** HTML5, CSS3, JavaScript, Bootstrap, Razor Views, Tag Helpers
- **Deployment:** Nginx (Reverse Proxy), Contabo Linux VPS (Ubuntu)

## 🧩 System Modules & Core Features

1. **🔐 Authentication & Security:** Secure login page for gym administration with role-based access control.
2. **🏠 Home / Dashboard:** Landing page with real-time gym stats (total members, active sessions, trainers count).
3. **👥 Members:** Register and manage gym members including photos, contact info, and health records.
4. **🏋️ Trainers:** Manage trainer profiles and their specialties (e.g., Yoga, Boxing, CrossFit, General Fitness).
5. **📅 Sessions:** Schedule classes specifying the trainer, time, and capacity (1–25 people).
6. **💳 Plans:** Manage membership tiers (Basic, Standard, Premium, Annual) with set prices and durations.
7. **🎫 MemberShips:** Assign plans to members with auto-calculation of start and end dates based on plan duration.
8. **📋 Sessions Schedule:** Allow members to book seats in sessions, track attendance, and handle cancellations.

## 🚀 Local Setup Instructions

1. Clone the repository:
   ```bash
   git clone [https://github.com/moaazatef20/GymManagementSolution_Final.git](https://github.com/moaazatef20/GymManagementSolution_Final.git)
