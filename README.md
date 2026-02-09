# Clean Architecture Framework

A modern .NET application demonstrating clean architecture principles with **Mediator**, **Factory**, **Unit of Work**, and **Repository** patterns. This project showcases how to build maintainable, scalable applications with clear architectural boundaries and minimal dependencies.

**📖 [View the Complete Architecture Guide](https://mail4hafij.github.io/dotnet-clean-architecture-demo/ARCHITECTURE_GUIDE.html)** - Interactive visual guide with diagrams and code comparisons

Developed by [Mohammad Hafijur Rahman](https://github.com/mail4hafij) | Free to use with attribution

---

## 📋 Table of Contents

- [Why This Framework?](#why-this-framework)
- [Tech Stack](#tech-stack)
- [Design Patterns](#design-patterns)
- [Getting Started](#getting-started)
- [Architecture Overview](#architecture-overview)
- [Factory Pattern vs Service Injection](#factory-pattern-vs-service-injection)
- [Project Structure](#project-structure)

---

## 🎯 Why This Framework?

Traditional .NET projects often suffer from **"service hell"** - where Service classes inject dozens of other services, making code:
- **Hard to understand** (what does this class actually do?)
- **Difficult to test** (mock 20+ dependencies?)
- **Impossible to maintain** (afraid to remove unused dependencies)
- **Architecturally unclear** (no enforced boundaries)

This framework solves these problems using the **Factory Pattern** with clear architectural layers:

```
Controller → Handler → Logic → Repository
```

**Each layer has 1-2 clean dependencies** instead of 10-20 scattered services.

---

## 🛠 Tech Stack

- **.NET 10** - Latest .NET framework with C# 13
- **Entity Framework Core** - ORM with Code First approach and migrations
- **Autofac** - IoC container for dependency injection
- **AutoMapper** - Object-to-object mapping for DTOs
- **ASP.NET Core Web API** - RESTful API with Swagger/OpenAPI
- **SQL Server** - Relational database

---

## 🏗 Design Patterns

### 1. **Mediator Pattern**
Request/Response handlers decouple controllers from business logic. Each request has a dedicated handler that processes it independently.

### 2. **Factory Pattern**
Factories create instances on-demand with proper dependencies. Keeps constructors clean and manages object creation complexity.

**Key Benefits:**
- **Architectural visibility** - Constructor signature reveals the layer role
- **Compile-time safety** - Circular dependencies fail at compile-time, not runtime
- **No dead dependencies** - Only create what you use, when you use it
- **Enforced boundaries** - Handlers can't access repositories directly (compiler prevents it)

### 3. **Unit of Work Pattern**
Manages database transactions ensuring all operations succeed or fail together. Provides a single commit point for data consistency.

### 4. **Repository Pattern**
Abstracts data access logic from business logic. Provides a collection-like interface for querying and persisting entities.

---

## 🚀 Getting Started

### Prerequisites

- .NET 10 SDK
- SQL Server (LocalDB or Express)
- Visual Studio 2022 or VS Code

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/mail4hafij/dotnet-clean-architecture-demo.git
   cd dotnet-clean-architecture-demo
   ```

2. **Update database connection string**

   Edit `Rest/appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=.;Database=efcore;Trusted_Connection=true;"
     }
   }
   ```

3. **Create and populate the database**

   Open Package Manager Console in Visual Studio:
   - Set **Core** project as default
   - Run migrations:
     ```
     Add-Migration InitialCreate
     Update-Database
     ```

   This will create tables and populate them with seed data (users, cars, orders).

4. **Run the application**

   - Set **Rest** as the startup project
   - Press F5 or run:
     ```bash
     dotnet run --project Rest
     ```

   - Swagger UI will open at `https://localhost:5001`

### Testing with Swagger

The project includes seed data for testing:

**Endpoints to try:**
- `GET /api/User` - List all users
- `GET /api/Car/user/1` - Get cars for user 1
- `GET /api/Order/user/1` - Get orders for user 1
- `GET /api/Order/1/summary` - Get order summary (demonstrates Logic-to-Logic dependency)
- `POST /api/Car` - Add a car with validation
- `POST /api/Order` - Create a new order

---

## 🏛 Architecture Overview

### Conceptual Flow

```
Controller (REST API)
    ↓
Handler (Orchestration)
    ↓
Logic (Business Rules)
    ↓
Repository (Data Access)
    ↓
Database
```

Any request from a controller is handled by a **Handler** via the **HandlerCaller**. The Handler uses **Logic** classes (via LogicFactory) to implement business rules. Logic classes use **Repositories** (via RepositoryFactory) for data access. All operations within a Handler share a **Unit of Work** for transactional consistency.


---

## ⚖️ Factory Pattern vs Service Injection

### The Problem with Traditional Service Injection

In older projects, everything is a "Service" that injects other Services, creating:

**❌ Traditional Service Class:**
```csharp
public class BusinessService
{
    // What does this class even do? You need to read the entire implementation to know.
    public BusinessService(
        IDataRepository dataRepo,
        IUserRepository userRepo,
        IProductRepository productRepo,
        ICustomerRepository customerRepo,
        IValidationService validationService,
        INotificationService notificationService,
        IEmailService emailService,
        IPaymentService paymentService,
        IInventoryService inventoryService,
        IShippingService shippingService,
        ILogger logger,
        IMapper mapper,
        IConfiguration config,
        ICache cache)  // 14 dependencies!
    {
        // Real projects have 20-30+ injections
        // What layer is this? Business logic? Data access? Nobody knows!
    }
}
```

**Problems:**
- **No layering** - Everything is a "Service" with no clear separation
- **Dependency explosion** - Services inject other Services infinitely
- **Mixed concerns** - Business logic + data access + infrastructure all mixed together
- **Testing nightmare** - Need to mock 15+ dependencies per test
- **Circular dependencies** - Won't know until runtime crash!

### The Factory Pattern Solution

**✅ Factory Pattern - Handler Shows Clear Dependencies:**

```csharp
// Handler: Clean constructor with only 2 factories
public class CreateOrderHandler : RequestHandler<CreateOrderReq, CreateOrderResp>
{
    private readonly ILogicFactory _logicFactory;
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public CreateOrderHandler(
        ILogicFactory logicFactory,
        IUnitOfWorkFactory uowFactory,
        IResponseFactory responseFactory)
        : base(uowFactory, responseFactory)
    {
        _logicFactory = logicFactory;
        _unitOfWorkFactory = uowFactory;
    }

    public override CreateOrderResp Process(CreateOrderReq req)
    {
        using (var unitOfWork = _unitOfWorkFactory.CreateUnitOfWork())
        {
            // Create CarLogic first
            var carLogic = _logicFactory.CreateCarLogic(unitOfWork);

            // Create OrderLogic and PASS CarLogic as constructor argument
            // Dependencies are VISIBLE right here!
            var orderLogic = _logicFactory.CreateOrderLogic(unitOfWork, carLogic);

            // Now OrderLogic can use CarLogic
            orderLogic.CreateOrder(req.UserId, req.OrderItems);

            unitOfWork.Commit();
        }

        return new CreateOrderResp { Success = true };
    }
}

// OrderLogic: Takes CarLogic as constructor parameter
public class OrderLogic : LogicBase
{
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly ICarLogic _carLogic;  // Passed as parameter by Handler!

    public OrderLogic(
        IRepositoryFactory repositoryFactory,
        ICarLogic carLogic,  // CarLogic passed as parameter - dependency is VISIBLE!
        IUnitOfWork unitOfWork)
        : base(unitOfWork)
    {
        _repositoryFactory = repositoryFactory;
        _carLogic = carLogic;  // Not created internally - passed in by Handler!
    }

    public void CreateOrder(long userId, List<OrderItem> items)
    {
        // Get repositories when needed
        var orderRepo = _repositoryFactory.CreateOrderRepository(_unitOfWork);
        var userRepo = _repositoryFactory.CreateUserRepository(_unitOfWork);

        // Use the injected CarLogic
        var user = userRepo.GetUser(userId);
        var userCarCount = _carLogic.GetUserCarCount(userId);

        // Business logic: Users with cars get 10% discount
        decimal discount = userCarCount > 0 ? 0.10m : 0.00m;

        // Create the order...
    }
}
```

**Why This Is Better:**

✅ **Dependencies visible in Handler** - You can SEE that OrderLogic depends on CarLogic (line where it's created)
✅ **Constructor shows dependencies** - OrderLogic constructor signature reveals it needs ICarLogic
✅ **No hidden dependencies** - Nothing happens "magically" inside Logic classes
✅ **No constructor bloat** - Handler still has 2 factories, creates dependencies on-demand
✅ **Easy to understand** - Looking at Handler, you immediately see the dependency graph

---

## 📊 Key Benefits Explained

### 1. Dependency Visibility & Control

**With traditional Service classes?** A "BusinessService" might inject 5 repositories, 8 other services, 3 mappers, 2 loggers, cache, config... Good luck figuring out what it does or what layer it represents!

**With Factory Pattern?** The constructor signature immediately reveals the class's architectural role. Handler has factories, Logic has repositories and other Logic, Repository has only UnitOfWork. Clean, predictable, and self-documenting.

### 2. Prevents Circular Dependencies (Compile-Time Safety)

```csharp
public interface ILogicFactory
{
    IOrderLogic CreateOrderLogic(IUnitOfWork uow);
    ICarLogic CreateCarLogic(IUnitOfWork uow);
}

// If OrderLogic needs CarLogic, and CarLogic needs OrderLogic,
// the LogicFactory implementation will fail to compile.
// You catch the circular dependency immediately!
```

**With service injection?** Circular dependencies only fail at runtime when the DI container starts. You might not catch it until production!

### 3. No More Dead Dependencies

```csharp
public void ProcessData(long id)
{
    var dataRepo = _repositoryFactory.CreateDataRepository(_unitOfWork);
    var validationLogic = _logicFactory.CreateValidationLogic(_unitOfWork);

    // That's it! Only 2 dependencies.
    // No hidden services lurking in the constructor.
}

// Want to refactor? Just delete the lines you don't use.
// No need to update constructors, tests, or IoC registrations.
```

**With service injection?** You have 15 services in the constructor. Are they all used? In which methods? Nobody knows. Everyone's afraid to remove them.

### 4. Architecture Enforcement (The Killer Feature)

```csharp
// GOOD: Handler uses Logic layer correctly
public class CreateOrderHandler
{
    public CreateOrderHandler(ILogicFactory logicFactory) { }
    // ✓ Can only access logic, not repositories directly
}

// BAD: This won't compile! Handler can't get repository factory!
public class BadHandler
{
    // ✗ Handler trying to bypass logic layer
    public BadHandler(IRepositoryFactory repoFactory) { }
    // Compile error: IRepositoryFactory not registered for handlers!
}
```

**Factory pattern enforces your architecture at compile time.** You can't accidentally violate layering because the factories physically prevent it.

---

## 📁 Project Structure

```
├── Common/                     # Shared contracts and DTOs
│   ├── Contract/
│   │   ├── Model/             # Data transfer objects (UserContract, CarContract, etc.)
│   │   └── Messaging/         # Request/Response messages
│   └── ICoreService.cs        # Core service interface
│
├── Core/                       # Business logic and data access
│   ├── DB/
│   │   ├── Builder/           # Entity builders for seeding data
│   │   ├── Logic/             # Business logic classes (UserLogic, CarLogic, OrderLogic)
│   │   ├── Mapper/            # Custom mappers for entities
│   │   ├── Model/             # Entity models (User, Car, Order)
│   │   ├── Repo/              # Repositories (UserRepository, CarRepository, etc.)
│   │   ├── DataContext.cs     # EF Core DbContext
│   │   ├── ILogicFactory.cs   # Factory for creating Logic instances
│   │   ├── IRepositoryFactory.cs  # Factory for creating Repository instances
│   │   └── UnitOfWork.cs      # Transaction management
│   │
│   ├── Handler/               # Request handlers (Mediator pattern)
│   │   ├── User/
│   │   ├── Car/
│   │   └── Order/
│   │
│   ├── IoC/                   # Dependency injection configuration
│   │   ├── CoreContainer.cs   # Autofac container setup
│   │   └── MappingProfile.cs  # AutoMapper configuration
│   │
│   └── LIB/                   # Framework libraries
│       ├── HandlerCaller.cs   # Mediator implementation
│       └── RequestHandler.cs  # Base handler class
│
└── Rest/                      # REST API project
    ├── Controllers/           # API controllers (UserController, CarController, OrderController)
    └── Program.cs            # API startup and configuration
```

---

## 📝 License

This framework is **free to use** - just keep the credit in file headers. See individual files for attribution.

---

## 🤝 Contributing

Contributions are welcome! Please feel free to submit issues or pull requests.

---

## 📧 Contact

Developed by **Mohammad Hafijur Rahman**
- GitHub: [@mail4hafij](https://github.com/mail4hafij)
- Repository: [dotnet-clean-architecture-demo](https://github.com/mail4hafij/dotnet-clean-architecture-demo)

---

**⭐ If you find this framework helpful, please star the repository!**


