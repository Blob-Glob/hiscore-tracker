# OSRS HiScore Tracker

## 📌 Overview
OSRS HiScore Tracker is a web application that allows users to lookup and track player statistics from Old School RuneScape (OSRS). The project consists of a **Blazor Web App** for the frontend and an **Azure Function App** as the backend, which fetches and updates player HiScores.

## 🚀 Features
- **Player HiScore Lookup:** Retrieve skill levels, experience, and ranks for any OSRS player.
- **Daily Tracking:** An automated Azure Function updates and stores user HiScores daily.
- **Blazor Web UI:** (Planned) A clean and interactive interface for viewing and comparing player stats.
- **Cloud-Powered Backend:** Azure Function App handles API requests efficiently.
- **.NET Aspire Integration:** (Planned) Simplified service orchestration and monitoring.
- **Microsoft SQL Server Database:** Stores historical player HiScores for tracking progress over time.
- **Database Versioning with Flyway:** Ensures smooth database migrations and schema management.

## 🛠️ Tech Stack
- **Frontend:** Blazor WebAssembly (.NET 9)
- **Backend:** Azure Function App (.NET 8)
- **Database:** PostgreSQL (managed with Flyway)
- **Database Migration:** Flyway
- **Hosting:** Azure
- **Authentication:** (Planned) Google Login Integration

## 🔧 Setup & Installation

### 1️⃣ Clone the Repository
```sh
git clone https://github.com/Blob-Glob/hiscore-tracker.git
cd hiscore-tracker
