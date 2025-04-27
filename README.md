# In Memory Cache Example

InMemoryCacheExample is a Web API project developed on the .NET 9 platform.
This project demonstrates how to optimize database access by integrating Dapper for fast queries and IMemoryCache for caching frequently accessed data.
It applies a clean, layered architecture and the cache-aside pattern to improve performance when handling a high number of database records.

## Technologies Used

- .NET 9 (ASP.NET Core Web API): Main framework of the project.
- Dapper ORM: Lightweight data access for SQL Server.
- Microsoft SQL Server: Storing user data.
- IMemoryCache: In-memory caching for rapid data retrieval.

## Setup

### Running Locally

- Install [Visual Studio Code or Visual Studio 2022](https://visualstudio.microsoft.com/vs/) or any suitable IDE.

- Install [.NET 9 SDK.](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)

- Set up a [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) database and use
  * ```CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY,
    Username NVARCHAR(100),
    Fullname NVARCHAR(150));

    SET NOCOUNT ON;
    DECLARE @i INT = 1;
    WHILE @i <= 1000000
    BEGIN
    INSERT INTO Users (Username, Fullname)
    VALUES (CONCAT('user', @i),CONCAT('Fullname ', @i);
    SET @i = @i + 1;
    END

- Clone the Repository:
  * `git clone https://github.com/RidvanOzturk/InMemoryCacheExample.git`

- Run project through IDE.

---

<br>
