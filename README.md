# MiniDashboard - JaysonDelosSantos

A full‑stack exam project combining a Web API, WPF desktop application, and automated testing.  
The solution applies Clean Architecture principles, ensuring separation of concerns, contract‑first design, dependency injection, and maintainability.  
It also integrates Swagger/OpenAPI for interactive API documentation and testing.

---

##  Entities
- Defines the structure of the tables used in the application.  
- In this exam, the only entity is Product.  
- Entities represent the domain objects and are shared between repositories and services.  

---

##  ServiceContracts
- Contains signatures for all service operations.  
- Defines the business logic contract without implementation details.  
- Ensures that the Services project can be swapped or extended without breaking consumers.  

---

##  Services
- Implements the ServiceContracts.  
- Contains the business logic layer (validation, orchestration, workflows).  
- Calls into repositories for persistence.  
- Acts as the bridge between Web API controllers and the Repository layer.  

---

##  RepositoryContracts
- Contains signatures for data access operations.  
- Defines how repositories should behave without tying to a specific database.  
- Ensures the Repositories project can be swapped (e.g., SQL, NoSQL, In‑Memory) without breaking services.  

---

##  Repositories
- Implements the RepositoryContracts.  
- Contains the data access layer.  
- In a production environment, this would typically use Entity Framework Core  
- For the purpose of this exam, the implementation uses an in‑memory list to simulate persistence.  

---

##  MiniDashboard.Api
- The Web API project that exposes endpoints to external clients.  
- Contains controllers which handle HTTP requests and responses.  
- Controllers delegate work to the Services layer, ensuring that business logic is not mixed with API concerns.  
- Uses Dependency Injection (DI) to register and resolve services and repositories at runtime.  
- Integrated with Swagger/OpenAPI for interactive API documentation:  
  - Automatically generates endpoint descriptions.  
  - Provides a UI to test API calls directly from the browser.  
  - Makes the API easier to understand and consume for developers and testers.  

---

##  MiniDashboard.App (WPF Desktop Application)
- The desktop client built with WPF and MVVM.  
- **Assets**: Stores images used in the UI.  
- **Commands**: Includes relay commands (parameterized and non‑parameterized) for binding actions.  
- **Components**: Navigation UI elements for switching views.  
- **Converters**: Value converters for UI binding scenarios.  
- **Enums**: Defines custom dialog types (info, error, warning, success, custom).  
- **Model**: In‑app models such as `MenuItem` for navigation lists.  
- **Services**: `DialogService` and `IDialogService` (interface + implementation) for showing windows and custom dialogs.  
- **Styles**: Centralized styling to make the UI more aesthetic.  
- **ViewModels**: Where API calls happen, bridging the desktop app to the Web API.  
- **Views**: The actual XAML designs for each screen.  
- Implements **INotifyPropertyChanged** and **ObservableCollection** to update the UI in real time when data changes.  

### App.xaml.cs
- Configured with IHost (Microsoft.Extensions.Hosting) to enable Dependency Injection in the desktop app.  
- Registered all Views, ViewModels, and HttpClient services.  
- HttpClient base URL set to `http://localhost:5125` (matching Web API launch settings).  

---

##  Testing Projects

###  MiniDashboard.Tests
- Focuses on service business logic.  
- Contains unit tests for the Services layer.  
- Validates that business rules and workflows behave correctly in isolation.  
- Uses mocked repositories to avoid external dependencies.  

###  MiniDashboard.IntegrationTests
- Focuses on API endpoint testing.  
- Calls the MiniDashboard.Api controllers directly.  
- Ensures that services and repositories are wired correctly through Dependency Injection.  
- Verifies that the API returns the expected responses and status codes.  

###  MiniDashboard.UITests
- Focuses on UI automation testing using FlaUI.  
- Validates navigation and dialog workflows from the user’s perspective.  
- For this exam, it contains only one test case:  
  - The user navigates to the Product view.  
  - Clicks the Add button.  
  - The ProductAddEditView dialog should display.  
  - The `IsEdit` property is verified to be false, confirming that the dialog opened in Add mode.  

---

