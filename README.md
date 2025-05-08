# Documents

## Project Overview
The project consists of two main modules:

1. **Administration Module**: Allows users to upload document templates.
2. **Public Portal**: Enables users to complete documents using the uploaded templates, search for completed documents, and print them.

The Public Portal is built using ASP.NET MVC and emulates a Single Page Application (SPA) with raw JavaScript. The Administration Portal is developed using ASP.NET Web Forms.

![Application Screenshot](http://i.imgur.com/QXdXWhf.png?1)

---

## Features
- Upload and manage document templates in the Administration Module.
- Complete documents using templates in the Public Portal.
- Search functionality for completed documents.
- Print completed documents.

---

## Technologies Used
- **ASP.NET MVC**: For the Public Portal.
- **ASP.NET Web Forms**: For the Administration Module.
- **JavaScript**: For SPA emulation in the Public Portal.
- **Entity Framework**: For database interactions.
- **jQuery and Knockout.js**: For enhanced interactivity.

---

## Setup Instructions
1. Clone the repository to your local machine:
   ```bash
   git clone <repository-url>
   ```
2. Open the solution file `AppHarbor.sln` in Visual Studio.
3. Restore NuGet packages by right-clicking on the solution and selecting `Restore NuGet Packages`.
4. Build the solution to ensure all dependencies are resolved.
5. Configure the database connection string in `Web.config` for both modules.
6. Run the application using IIS Express or your preferred web server.

---

## Folder Structure
- **Acte/**: Contains the Public Portal (ASP.NET MVC).
- **Administrare/**: Contains the Administration Module (ASP.NET Web Forms).
- **packages/**: NuGet packages used in the project.
- **Tool PDF/**: Tools for PDF generation (if applicable).

---

## Usage
1. Access the Administration Module to upload document templates.
2. Navigate to the Public Portal to complete documents using the templates.
3. Use the search functionality to find completed documents.
4. Print the completed documents as needed.

---

## Contributing
Contributions are welcome! Please follow these steps:
1. Fork the repository.
2. Create a new branch for your feature or bug fix.
3. Commit your changes and push them to your fork.
4. Submit a pull request with a detailed description of your changes.

---

## License
This project is licensed under the MIT License. See the `LICENSE` file for more details.

