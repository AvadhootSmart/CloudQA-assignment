## Prerequisites

- **.NET Framework 4.7.2** or **.NET Core 3.1+**
- **Google Chrome Browser** (latest version)
- **Visual Studio** or **Visual Studio Code**

## Setup Instructions

### 1. Clone the Repository
```bash
git clone <your-repository-url>
cd <repository-folder>
```

### 2. Install NuGet Packages
Run these commands in the Package Manager Console or use NuGet Package Manager:

```bash
Install-Package Selenium.WebDriver
Install-Package Selenium.WebDriver.ChromeDriver
Install-Package Selenium.Support
Install-Package DotNetSeleniumExtras.WaitHelpers
```

Or using .NET CLI:
```bash
dotnet add package Selenium.WebDriver
dotnet add package Selenium.WebDriver.ChromeDriver
dotnet add package Selenium.Support
dotnet add package DotNetSeleniumExtras.WaitHelpers
```

### 3. Build the Project
```bash
dotnet build
```

## Running the Tests

### Option 1: Command Line
```bash
dotnet run
```

### Option 2: Run Executable
```bash
# After building, navigate to bin folder
cd bin/Debug/net{version}/
./CloudQAFormAutomation.exe
```
