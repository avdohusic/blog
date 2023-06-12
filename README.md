# Blog
Simple blog with CRUD Operations

## About project
The purpose of this project is to develop a simple blog application that utilizes a database for storing blog posts.
The project follows the principles of the Command Query Responsibility Segregation (CQRS) pattern.

## Requirements
**Technical pre-request:**
- .NET 7
- Docker

**Optional technical pre-request to be installed:**
- Postman
- Docker Desktop

**Functional requirements:**
- Design and implement a C# RESTful API using ASP.NET Core.
- The API should support CRUD (Create, Read, Update, Delete) operations for blog posts.
- Each blog post should have the following properties: title, content, author, and publication date.
- The blog content should be entered by using WYSIWYG editor and correctly display it on the UI.
- Implement import/export of one or more blog post from Excel file.
- Implement proper input validation and error handling for the API.
- Implement proper authentication and authorization mechanisms for the API.
- The API should use a database (e.g., SQL Server) to persist the blog posts.
- Design the database schema and implement the necessary data access layer.
- Use appropriate design patterns and principles (e.g., SOLID principles) in the implementation.
- Write unit tests to ensure the correctness of the API.
- Provide API documentation (e.g., using Swagger or similar tools) for easy integration by other developers.

**Additional Guidelines:**
- You can use any libraries or frameworks commonly used with ASP.NET Core.
- Focus on writing clean, maintainable, and scalable code.
- Consider performance optimizations where applicable.
- Use Git for version control and provide a repository with your code.
- Write short instructions for installation and usage.
- Feel free to ask questions if any part of the task is unclear.

**Evaluation Criteria:**
- Code organization and structure.
- Correct implementation of CRUD operations.
- Proper input validation and error handling.
- Implementation of authentication and authorization.
- Design and implementation of the database schema and data access layer.
- Usage of design patterns and principles.
- Unit test coverage and correctness.
- API documentation.

## How to get the project up and running
1. Clone the project: `git clone https://github.com/avdohusic/blog.git`
- **BACKEND**
2.  Run command `docker-compose up -d` in project's root
3. If everything is ok, Swagger page inside of browser should open
4. In case that you have problem with docker, you can run projecti in VisualStudio, right click on `SimpleBlog.Api` -> Set as Startup Project (choose `http` profile from launch settings) and uncomment connection string for local mssql in `appsettings.json`
5. URL: `https://localhost:9021/api-docs/index.html` or `http://localhost:9020/api-docs/index.html`
- **FRONTEND**
6. Run command `npm install` inside `frontend` folder
7. Run command `ng serve`
8. Navigate to `http://localhost:4200` from your browser
9. You can use pre-seeded users to test out requests:

    - **username**: `admin` or `publisher` or `user`
    - **password**: `Test123!`

10. Permission level explanations:
    - **Every logged-in** can access to get all blogs and get by id endpoints
    - **Administrator** can access to: export and import endpoints
    - **Publisher** can access to: create, update and delete blog endpoints

## Additional information
- Database migrations have been configured to be migrated automatically on application startup. Please note that this is not a best practice by any means, but rather to enable the app to be started as easily as possible.
- Frontend: I had to perform the UI quickly as a result of the time limit and other obligations in my life. I rushed through the process and did not follow the best practices of coding. Also, I didn't have time to cover my code with comments and tests.

## Project structure
Project is built using principles of the clean architecture with .NET 7, C# 10 and MSSQL as a database provider.

- src
    - API: *contains Api project*
    - Application: *contains business logic, request/response contract models and domain entities*
    - Domain: *contains entities, enums and constants. Everything related to the core or domain of application*
    - Infrastructure: *contains persistence configuration*
- tests
    - Unit tests
    - Arhitecture tests

## Contributors
Code is written and maintained by Avdo Husic. You can reach me at: avdo.husic@gmail.com
