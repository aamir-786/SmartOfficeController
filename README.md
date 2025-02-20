# Smart Office Controller Project

## Project Overview

This project implements a Smart Office Controller class using Test-Driven Development (TDD) in C#. The controller manages various smart systems in an office, including doors, lights, and fire alarms. It also logs state changes to an online web service and sends maintenance emails when necessary.

## Project Structure

- `Source/`: Contains the implementation files for the `OfficeController` class and its dependencies.
  - `OfficeController.cs`: The main class for managing the smart office systems.
  - `IDoorManager.cs`: Interface for managing doors.
  - `ILightManager.cs`: Interface for managing lights.
  - `IFireAlarmManager.cs`: Interface for managing fire alarms.
  - `IWebService.cs`: Interface for logging to an online web service.
  - `IEmailService.cs`: Interface for sending maintenance emails.
- `Tests/`: Contains the unit tests for the `OfficeController` class.
  - `OfficeControllerTests.cs`: Unit tests for the `OfficeController` class.
- `Docs/`: Contains documentation files.
  - `README.md`: This file.
  - `Requirements Checklist.docx`: A checklist of all implemented requirements.
- `Demo/`: Contains the demo video.
  - `DemoVideo.mp4`: A short demo video explaining the implementation and testing.

## Building and Running the Project

### Prerequisites

- .NET SDK installed.
- Visual Studio or Visual Studio Code installed.

### Building the Project

1. Open a terminal and navigate to the project directory.
2. Run the following command to build the solution:

   ```sh
   dotnet build
Running the Tests
Run the following command to execute the unit tests:
shCopy
dotnet test
Contributing
This project was developed as part of the Software Development module (CO2401) at Preston City Council. The project follows Test-Driven Development (TDD) principles and uses the NUnit framework for unit testing.
Contact Information
For any questions or issues, please contact:
Module Leader: [Module Leader's Email]
Tutor: [Tutor's Email]
License
This project is licensed under the MIT License. See the LICENSE file for details.
Copy

### Instructions to Create the `README.md` File

1. **Navigate to the `Docs/` Directory**:
   - Open your project directory in a file explorer.
   - Navigate to the `Docs/` directory.

2. **Create the `README.md` File**:
   - Inside the `Docs/` directory, create a new file named `README.md`.
   - Open the file in a text editor (e.g., Notepad, VS Code).

3. **Copy and Paste the Content**:
   - Copy the provided content above and paste it into the `README.md` file.

4. **Save the File**:
   - Save the `README.md` file.

### Example Project Structure

Ensure your project structure looks like this:
SmartOfficeController/
│
├── OfficeController/
│   ├── OfficeController.cs
│   ├── IDoorManager.cs
│   ├── ILightManager.cs
│   ├── IFireAlarmManager.cs
│   ├── IWebService.cs
│   └── IEmailService.cs
│
├── OfficeControllerTests/
│   └── OfficeControllerTests.cs
│
├── Docs/
│   ├── README.md
│   └── Requirements Checklist.docx
│
└── SmartOfficeController.sln
