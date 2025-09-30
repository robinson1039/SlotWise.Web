# SlotWise.Web

Aplicación web desarrollada en **ASP.NET Core (.NET 8)** con soporte para Entity Framework Core y arquitectura MVC.

---

## 🚀 Requisitos previos

Antes de ejecutar el proyecto en tu máquina necesitas tener instalado:

- [Visual Studio 2022](https://visualstudio.microsoft.com/es/) con las cargas de trabajo:
  - **ASP.NET y desarrollo web**
  - **Desarrollo de escritorio .NET** (opcional)
- [SDK .NET 8](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/es-es/sql-server/sql-server-downloads) o [PostgreSQL](https://www.postgresql.org/) (dependiendo de tu base de datos configurada en `appsettings.json`).
- [Git](https://git-scm.com/) (para clonar el repositorio).

---

## ⚙️ Configuración

1. **Clonar el repositorio**
   ```bash
   git clone https://github.com/TU-USUARIO/SlotWise.Web.git
   cd SlotWise.Web
2. **Configurar la base de datos**
    - Abre el archivo appsettings.json.
    - Cambia la cadena de conexion segun tu entorno local:
    - "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=SlotWiseDB;User Id=sa;Password=tu_password;"
    }
3.  **Aplicar migraciones de Entity Framework Core**
    - dotnet ef database update
4. **Ejecutar el proyecto**
    - Desde Visual Studio: pulsa F5 o clic en ▶️ IIS Express / SlotWise.Web.
    - O desde la terminal:
    - dotnet run --project SlotWise.Web

## 📂 Estructura del proyecto

     Controllers/ → Controladores MVC.

    Models/ → Modelos de datos.

    Data/ → Contexto de base de datos (EF Core).

    Views/ → Vistas Razor.

    wwwroot/ → Archivos estáticos (css, js, imágenes).

    Migrations/ → Migraciones de base de datos generadas con EF Core.

## 🛠️ Tecnologías usadas

    .NET 8

    ASP.NET Core MVC

    Entity Framework Core

    SQL Server

    Bootstrap / Tailwind (dependiendo de tu frontend)

    GitHub Actions (CI/CD) (opcional si lo configuras)