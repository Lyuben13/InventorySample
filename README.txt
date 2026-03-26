Inventory Management System
==========================

A comprehensive console-based inventory management application built with C# and .NET 8.
This system demonstrates best practices in software development including clean architecture,
comprehensive error handling, and maintainable code structure.

Features:
--------
- Full CRUD operations for product management
- Advanced search capabilities with case-insensitive matching
- Flexible filtering by stock quantity levels
- Comprehensive inventory reporting with financial summaries
- Low stock alerts for proactive inventory management
- Input validation and error handling throughout the application

Technical Architecture:
---------------------
- **Models/Product.cs**: Core entity representing inventory items with computed properties
- **Services/IInventoryService.cs**: Interface defining business operations contract
- **Services/InventoryService.cs**: Concrete implementation with in-memory storage
- **Reports/InventoryReportService.cs**: Dedicated reporting service with formatted output
- **Extensions/ConsoleMenu.cs**: Utility methods for user input validation
- **Program.cs**: Main application entry point with dependency injection

Key Design Principles:
--------------------
- **Separation of Concerns**: Business logic separated from UI and data access
- **Dependency Injection**: Services injected for testability and flexibility
- **Comprehensive Documentation**: XML documentation for all public members
- **Error Handling**: Graceful error handling with user-friendly messages
- **Input Validation**: Robust validation to ensure data integrity
- **Financial Precision**: Uses decimal type for all monetary calculations

Getting Started:
---------------
1. Navigate to the InventoryApp directory
2. Restore dependencies:
   ```
   dotnet restore
   ```
3. Run the application:
   ```
   dotnet run
   ```

Code Quality Features:
---------------------
- **Decimal for Financial Calculations**: Ensures precise monetary values without floating-point errors
- **Service Layer Architecture**: Business logic encapsulated in service classes for maintainability
- **Comprehensive Validation**: Validates names, prices, quantities, and ensures unique IDs
- **Database Ready**: Architecture supports easy migration to Entity Framework Core and SQL Server
- **Error Resilience**: Try-catch blocks in all user-facing operations with meaningful error messages

Future Enhancements:
-------------------
- Database persistence with Entity Framework Core
- Web API interface for remote access
- Advanced reporting with charts and graphs
- User authentication and authorization
- Bulk import/export functionality
- Automated low stock notifications

This application serves as an excellent foundation for learning C# development patterns
and can be easily extended to meet enterprise inventory management needs.
