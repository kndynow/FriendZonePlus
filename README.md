<!-- Improved compatibility of back to top link: See: https://github.com/othneildrew/Best-README-Template/pull/73 -->
<a id="readme-top"></a>

<!-- PROJECT SHIELDS -->
[![Contributors][contributors-shield]][contributors-url]
[![Stargazers][stars-shield]][stars-url]
[![Method Coverage][coverage-method-shield]][coverage-url]


<!-- PROJECT LOGO -->
<br />
<div align="center">
  <h3 align="center">FriendZone+</h3>

  <p align="center">
    Finally a zone you want to be in!
    <br />
    A modern social media application built with .NET and React
    <br />
    <br />
    <a href="https://github.com/kndynow/FriendZonePlus/issues/new?labels=bug&template=bug-report---.md">Report Bug</a>
    &middot;
    <a href="https://github.com/kndynow/FriendZonePlus/issues/new?labels=enhancement&template=feature-request---.md">Request Feature</a>
  </p>
</div>

<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#features">Features</a></li>
    <li><a href="#project-structure">Project Structure</a></li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#code-coverage">Code Coverage</a></li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#acknowledgments">Acknowledgments</a></li>
  </ol>
</details>

<!-- ABOUT THE PROJECT -->
## About The Project

FriendZone+ is what happens when the teacher says "let's build a social media app in two weeks" and actually tries to do it. It's a not so modern social media application that allows users to connect, share posts, follow each other, and send real-time messages. Built with a clean architecture approach (because we had to pretend we knew what we were doing), it features a robust .NET backend with a React TypeScript frontend. Yes, it works. Mostly. Sometimes. We're still figuring it out.

### Key Features

- **User Authentication**: Secure JWT-based authentication system
- **Wall Posts**: Share posts on your own wall or on friends' walls
- **Follow System**: Follow and unfollow other users
- **Real-time Messaging**: Send and receive messages instantly using SignalR
- **User Profiles**: Customizable user profiles with profile pictures
- **User Discovery**: Find and connect with other users

<p align="right">(<a href="#readme-top">back to top</a>)</p>

### Built With

#### Backend
* [![.NET][.NET]][.NET-url]
* [![ASP.NET Core][ASP.NET]][ASP.NET-url]
* [![Entity Framework Core][EF-Core]][EF-Core-url]
* [![SQLite][SQLite]][SQLite-url]
* [![SignalR][SignalR]][SignalR-url]
* [![FluentValidation][FluentValidation]][FluentValidation-url]
* [![Mapster][Mapster]][Mapster-url]

#### Frontend
* [![React][React.js]][React-url]
* [![TypeScript][TypeScript]][TypeScript-url]
* [![Vite][Vite]][Vite-url]
* [![Bootstrap][Bootstrap.com]][Bootstrap-url]
* [![React Router][React-Router]][React-Router-url]
* [![Axios][Axios]][Axios-url]

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- GETTING STARTED -->
## Getting Started

This section will guide you through setting up FriendZone+ locally on your machine.

### Prerequisites

Before you begin, ensure you have the following installed:

* .NET 10.0 SDK or later
  ```sh
  # Check if .NET is installed
  dotnet --version
  ```
* Node.js (v18 or later) and npm
  ```sh
  # Check if Node.js is installed
  node --version
  npm --version
  ```

### Installation

1. Clone the repository
   ```sh
   git clone https://github.com/kndynow/FriendZonePlus.git
   cd FriendZonePlus
   ```

2. **Backend Setup**

   Navigate to the backend directory:
   ```sh
   cd backend
   ```

   Restore NuGet packages:
   ```sh
   dotnet restore
   ```

   The database will be automatically created and migrated when you run the application. The default SQLite database file (`friendzoneplus.db`) will be created in the `FriendZonePlus.API` directory.

3. **Frontend Setup**

   Navigate to the frontend directory:
   ```sh
   cd ../frontend
   ```

   Install npm packages:
   ```sh
   npm install
   ```

4. **Configuration**

   Backend configuration is in `backend/src/FriendZonePlus.API/appsettings.json`. You may need to configure:
   - JWT settings (SecretKey, Issuer, Audience)
   - Connection string for the database

   Frontend API configuration is in `frontend/src/api/client.ts`. Ensure the API URL matches your backend configuration.

5. **Running the Application**

   **Backend:**
   ```sh
   cd backend/src/FriendZonePlus.API
   dotnet run
   ```
   The API will run on `http://localhost:5000` (or the port specified in `launchSettings.json`)

   **Frontend:**
   ```sh
   cd frontend
   npm run dev
   ```
   The frontend will run on `http://localhost:5173`

6. **API Documentation**

   When running in Development mode, you can access:
   - OpenAPI documentation at `/openapi/v1.json`
   - Scalar API Reference at `/scalar`

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- FEATURES -->
## Features

- ✅ **User Registration & Authentication**
  - Secure password hashing with BCrypt
  - JWT token-based authentication
  - Cookie-based session management

- ✅ **Wall Posts**
  - Create posts on your own wall
  - Post on friends' walls
  - View posts from users you follow

- ✅ **Follow System**
  - Follow and unfollow users
  - View followers and following lists
  - Discover new users

- ✅ **Real-time Messaging**
  - Send and receive messages instantly
  - SignalR-powered real-time communication
  - Message read status tracking

- ✅ **User Profiles**
  - Customizable user profiles
  - Profile picture support
  - View user information and activity

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- PROJECT STRUCTURE -->
## Project Structure

```
FriendZonePlus/
├── backend/
│   ├── src/
│   │   ├── FriendZonePlus.API/          # Web API layer
│   │   ├── FriendZonePlus.Application/  # Application logic & services
│   │   ├── FriendZonePlus.Core/        # Domain entities & interfaces
│   │   └── FriendZonePlus.Infrastructure/ # Data access & external services
│   └── tests/
│       └── FriendZonePlus.UnitTests/    # Unit tests
├── frontend/
│   ├── src/
│   │   ├── api/                         # API client & services
│   │   ├── components/                  # Reusable UI components
│   │   ├── feature/                     # Feature-specific components
│   │   ├── hooks/                       # Custom React hooks
│   │   ├── pages/                       # Page components
│   │   └── types/                       # TypeScript type definitions
│   └── sass/                            # Stylesheets
└── learning-resources/                  # Documentation & learning materials
```

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- USAGE -->
## Usage

### Running Tests

**Backend Unit Tests:**
```sh
cd backend
dotnet test
```

**Code Coverage:**
```sh
# On Windows
.\run-coverage.ps1

# On Unix/Linux/Mac
./run-coverage.sh
```

### Development

The application uses:
- **Clean Architecture** for backend separation of concerns
- **Repository Pattern** for data access
- **Dependency Injection** throughout the application
- **FluentValidation** for request validation
- **Mapster** for object mapping

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- CODE COVERAGE -->
## Code Coverage

The project uses [Coverlet](https://github.com/coverlet-coverage/coverlet) for code coverage analysis and [ReportGenerator](https://github.com/danielpalme/ReportGenerator) for generating HTML reports.

### Current Coverage Statistics

- **Line Coverage**: 34.1% (187 of 548 coverable lines)
- **Method Coverage**: 58%
- **Branch Coverage**: 0% (0 of 12 branches)

### Generating Coverage Reports

**On Windows:**
```sh
cd backend
.\run-coverage.ps1
```

**On Unix/Linux/Mac:**
```sh
cd backend
./run-coverage.sh
```

The coverage report will be generated in `backend/coverage/report/index.html`. Open this file in a browser to view the detailed coverage report.

### Coverage by Assembly

- **FriendZonePlus.Core**: 100% line coverage
- **FriendZonePlus.Application**: 63.4% line coverage
- **FriendZonePlus.Infrastructure**: 63.6% line coverage
- **FriendZonePlus.API**: 0% line coverage (endpoints not yet covered by unit tests)

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- ROADMAP -->
## Roadmap

- [ ] Enhanced user profile customization
- [ ] Post likes and comments
- [ ] Image upload for posts
- [ ] Notifications system
- [ ] Search functionality
- [ ] Privacy settings

See the [open issues](https://github.com/kndynow/FriendZonePlus/issues) for a full list of proposed features (and known issues).

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- CONTRIBUTING -->
## Contributing

Contributions are what make the open source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

If you have a suggestion that would make this better, please fork the repo and create a pull request. You can also simply open an issue with the tag "enhancement".
Don't forget to give the project a star! Thanks again!

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

<p align="right">(<a href="#readme-top">back to top</a>)</p>

### Top contributors:

<a href="https://github.com/kndynow/FriendZonePlus/graphs/contributors">
  <img src="https://contrib.rocks/image?repo=kndynow/FriendZonePlus" alt="contrib.rocks image" />
</a>

<!-- LICENSE -->
## License

Distributed under the MIT License. See `LICENSE.txt` for more information.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- ACKNOWLEDGMENTS -->
## Acknowledgments

* [Best README Template](https://github.com/othneildrew/Best-README-Template)
* [.NET Documentation](https://learn.microsoft.com/en-us/dotnet/)
* [React Documentation](https://react.dev/)

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[contributors-shield]: https://img.shields.io/github/contributors/kndynow/FriendZonePlus.svg?style=for-the-badge
[contributors-url]: https://github.com/kndynow/FriendZonePlus/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/kndynow/FriendZonePlus.svg?style=for-the-badge
[forks-url]: https://github.com/kndynow/FriendZonePlus/network/members
[stars-shield]: https://img.shields.io/github/stars/kndynow/FriendZonePlus.svg?style=for-the-badge
[stars-url]: https://github.com/kndynow/FriendZonePlus/stargazers
[issues-shield]: https://img.shields.io/github/issues/kndynow/FriendZonePlus.svg?style=for-the-badge
[issues-url]: https://github.com/kndynow/FriendZonePlus/issues
[license-shield]: https://img.shields.io/github/license/kndynow/FriendZonePlus.svg?style=for-the-badge
[license-url]: https://github.com/kndynow/FriendZonePlus/blob/master/LICENSE.txt
[coverage-line-shield]: https://img.shields.io/badge/Line%20Coverage-34.1%25-red?style=for-the-badge
[coverage-method-shield]: https://img.shields.io/badge/Method%20Coverage-58%25-yellow?style=for-the-badge
[coverage-branch-shield]: https://img.shields.io/badge/Branch%20Coverage-0%25-red?style=for-the-badge
[coverage-url]: backend/coverage/report/index.html

<!-- Technology Badges -->
[.NET]: https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white
[.NET-url]: https://dotnet.microsoft.com/
[ASP.NET]: https://img.shields.io/badge/ASP.NET%20Core-10.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white
[ASP.NET-url]: https://learn.microsoft.com/en-us/aspnet/core/
[EF-Core]: https://img.shields.io/badge/Entity%20Framework%20Core-10.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white
[EF-Core-url]: https://learn.microsoft.com/en-us/ef/core/
[SQLite]: https://img.shields.io/badge/SQLite-003B57?style=for-the-badge&logo=sqlite&logoColor=white
[SQLite-url]: https://www.sqlite.org/
[SignalR]: https://img.shields.io/badge/SignalR-512BD4?style=for-the-badge&logo=dotnet&logoColor=white
[SignalR-url]: https://learn.microsoft.com/en-us/aspnet/core/signalr/introduction
[FluentValidation]: https://img.shields.io/badge/FluentValidation-12.1-FF5722?style=for-the-badge
[FluentValidation-url]: https://docs.fluentvalidation.net/
[Mapster]: https://img.shields.io/badge/Mapster-7.4-FF5722?style=for-the-badge
[Mapster-url]: https://github.com/MapsterMapper/Mapster

[React.js]: https://img.shields.io/badge/React-20232A?style=for-the-badge&logo=react&logoColor=61DAFB
[React-url]: https://reactjs.org/
[TypeScript]: https://img.shields.io/badge/TypeScript-007ACC?style=for-the-badge&logo=typescript&logoColor=white
[TypeScript-url]: https://www.typescriptlang.org/
[Vite]: https://img.shields.io/badge/Vite-646CFF?style=for-the-badge&logo=vite&logoColor=white
[Vite-url]: https://vitejs.dev/
[Bootstrap.com]: https://img.shields.io/badge/Bootstrap-563D7C?style=for-the-badge&logo=bootstrap&logoColor=white
[Bootstrap-url]: https://getbootstrap.com
[React-Router]: https://img.shields.io/badge/React%20Router-CA4245?style=for-the-badge&logo=react-router&logoColor=white
[React-Router-url]: https://reactrouter.com/
[Axios]: https://img.shields.io/badge/Axios-5A29E4?style=for-the-badge&logo=axios&logoColor=white
[Axios-url]: https://axios-http.com/
