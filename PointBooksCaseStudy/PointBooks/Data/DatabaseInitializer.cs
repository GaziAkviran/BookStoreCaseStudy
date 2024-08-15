using Dapper;
using System.Data;
using System.Runtime.CompilerServices;

namespace PointBooks.Data
{
    public static class DatabaseInitializer
    {
        public static void InitializeDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<DapperContext>();
                Initialize(context);
            }
        }

        public static void Initialize(DapperContext context)
        {
            using (var connection = context.CreateConnection())
            {
                var createTablesQuery = @"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' AND xtype='U')
                    BEGIN
                        CREATE TABLE Users (
                            UserID INT PRIMARY KEY IDENTITY(1,1),
                            Name NVARCHAR(50) NOT NULL,
                            Surname NVARCHAR(50) NOT NULL,
                            Email NVARCHAR(100) NOT NULL,
                            Password NVARCHAR(255) NOT NULL,
                            RoleID INT NOT NULL
                        );
                    END

                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Roles' AND xtype='U')
                    BEGIN
                        CREATE TABLE Roles (
                            RoleID INT PRIMARY KEY IDENTITY(1,1),
                            RoleName NVARCHAR(50) NOT NULL
                        );
                    END

                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Books' AND xtype='U')
                    BEGIN
                        CREATE TABLE Books (
                            BookID INT PRIMARY KEY IDENTITY(1,1),
                            Title NVARCHAR(250) NOT NULL,
                            CategoryID INT NOT NULL,
                            PublisherID INT NOT NULL,
                            Price DECIMAL(18, 2) NOT NULL,
                            Language NVARCHAR(50) NOT NULL,
                            PublicationYear INT NOT NULL,
                            Stock INT NOT NULL
                        );
                    END

                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Orders' AND xtype='U')
                    BEGIN
                        CREATE TABLE Orders (
                            OrderID INT PRIMARY KEY IDENTITY(1,1),
                            UserID INT NOT NULL,
                            CartID INT NOT NULL,
                            OrderDate DATETIME NOT NULL,
                            TotalAmount DECIMAL(18, 2) NOT NULL
                        );
                    END

                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='OrderItems' AND xtype='U')
                    BEGIN
                        CREATE TABLE OrderItems (
                            OrderItemID INT PRIMARY KEY IDENTITY(1,1),
                            OrderID INT NOT NULL,
                            BookID INT NOT NULL,
                            Quantity INT NOT NULL,
                            Price DECIMAL(18, 2) NOT NULL
                        );
                    END

                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Cart' AND xtype='U')
                    BEGIN
                        CREATE TABLE Cart (
                            CartID INT PRIMARY KEY IDENTITY(1,1),
                            UserID INT NOT NULL
                        );
                    END

                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='CartItems' AND xtype='U')
                    BEGIN
                        CREATE TABLE CartItems (
                            CartItemID INT PRIMARY KEY IDENTITY(1,1),
                            CartID INT NOT NULL,
                            BookID INT NOT NULL,
                            Quantity INT NOT NULL
                        );
                    END

                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Authors' AND xtype='U')
                    BEGIN
                        CREATE TABLE Authors (
                            AuthorID INT PRIMARY KEY IDENTITY(1,1),
                            Name NVARCHAR(100) NOT NULL,
                            Surname NVARCHAR(100) NOT NULL
                        );
                    END

                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='BookAuthors' AND xtype='U')
                    BEGIN
                        CREATE TABLE BookAuthors (
                            BookID INT NOT NULL,
                            AuthorID INT NOT NULL,
                            PRIMARY KEY (BookID, AuthorID)
                        );
                    END

                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Categories' AND xtype='U')
                    BEGIN
                        CREATE TABLE Categories (
                            CategoryID INT PRIMARY KEY IDENTITY(1,1),
                            CategoryName NVARCHAR(50) NOT NULL
                        );
                    END

                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Publishers' AND xtype='U')
                    BEGIN
                        CREATE TABLE Publishers (
                            PublisherID INT PRIMARY KEY IDENTITY(1,1),
                            PublisherName NVARCHAR(100) NOT NULL
                        );
                    END
                ";

                connection.Execute(createTablesQuery);
            }
        }
    }
}
