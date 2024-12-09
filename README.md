# File Storage App 📁

FileStorageApp is a robust application designed to manage, store, and share files with ease. It offers secure file storage and seamless sharing functionalities tailored to user needs.

---

## Features 🚀

- **File Upload & Management:** Effortlessly upload, organize, and manage your files.
- **Secure Sharing:** Share files securely with other users or generate public links.
- **Version Control:** Keep track of file updates with built-in version management.
- **Search & Filters:** Quickly locate files using advanced search and filtering options.
- **Authorization & Privacy:** Protected by role-based access control (RBAC) for user privacy.

---

## Technologies Used 🛠️

### Backend:
- **.NET 8.0**
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **Azure SQL Server**

### Additional Tools:
- **Azure Blob Storage** (File hosting)
- **JWT** (Authentication)

---

## Getting Started 🏗️

### Prerequisites:
1. Install [.NET SDK](https://dotnet.microsoft.com/download/dotnet)
2. Set up a local or remote SQL Server instance.
3. Set up an Azure Blog Storage Container.
4. Optionally, configure [Docker](https://www.docker.com/) for containerized environments.

### Package Dependencies

**API Documentation**

- Swashbuckle.AspNetCore 
- Microsoft.AspNetCore.OpenApi
  
**Authentication and Security**
  
- Microsoft.AspNetCore.Authentication.JwtBearer 
- System.IdentityModel.Tokens.Jwt
  
**Entity Framework Core**

- Microsoft.EntityFrameworkCore 
- Microsoft.EntityFrameworkCore.Design
- Microsoft.EntityFrameworkCore.Tools
- Microsoft.EntityFrameworkCore.SqlServer

**Unit Test**

- Microsoft.NET.Test.Sdk
- Moq
- xunit

**Additional**

- AutoMapper 
- Azure.Storage.Blobs
- Microsoft.AspNetCore.Http.Features
- Microsoft.AspNetCore.Http.Abstractions

---

# API Documentation

## Table of Contents
- [Authentication](#authentication)
- [Auth Endpoints](#auth-endpoints)
- [Files Endpoints](#files-endpoints)
- [Folders Endpoints](#folders-endpoints)
- [Sharing Endpoints](#sharing-endpoints)
- [Data Models](#data-models)

## Authentication

The API uses JWT (JSON Web Token) authentication. Clients must include the Bearer token in the Authorization header for most endpoints.

### Authentication Scheme
- Type: Bearer Token
- Scheme: JWT

## Auth Endpoints

### 1. User Login
- **Endpoint**: `/Auth/login`
- **Method**: POST
- **Request Body**: `LoginDto`
  - `username` (required)
  - `password` (required)

### 2. Create User
- **Endpoint**: `/Auth/createUser`
- **Method**: POST
- **Request Body**: `CreateUserDto`
  - `username` (required)
  - `password` (required)
  - `email` (required)

### 3. Get Users
- **Endpoint**: `/Auth/getUsers`
- **Method**: GET
- **Response**: Array of `UserDto`

## Files Endpoints

### 1. Upload File (Multipart)
- **Endpoint**: `/api/Files/upload`
- **Method**: POST
- **Request Body**: 
  - `File` (required, binary)
  - `FolderPath` (optional)
- **Response**: `FileDto`

### 2. Upload File (JSON)
- **Endpoint**: `/api/Files/upload-2`
- **Method**: POST
- **Request Body**: `CreateFileWithNameDto`
  - `fileName` (required)
  - `folderPath` (optional)
- **Response**: `FileDto`

### 3. Get File Details
- **Endpoint**: `/api/Files/{fileId}`
- **Method**: GET
- **Response**: `FileDto`

### 4. Delete File
- **Endpoint**: `/api/Files/{fileId}`
- **Method**: DELETE

### 5. Download File
- **Endpoint**: `/api/Files/download/{fileId}`
- **Method**: GET

## Folders Endpoints

### 1. Get Folder Details
- **Endpoint**: `/api/Folders/{folderId}`
- **Method**: GET
- **Response**: `FolderDetailsDto`

### 2. Delete Folder
- **Endpoint**: `/api/Folders/{folderId}`
- **Method**: DELETE

## Sharing Endpoints

### 1. Share Resource
- **Endpoint**: `/api/Sharing/share`
- **Method**: POST
- **Request Body**: `CreateShareRequestDto`
  - `resourceId`
  - `resourceType`
  - `sharedWithId`
  - `permission`
  - `expiresAt` (optional)
- **Response**: `ShareDto`

### 2. Get Resource Sharing
- **Endpoint**: `/api/Sharing/{resourceId}/sharing`
- **Method**: GET
- **Response**: Array of `ShareDto`

## Data Models

### ResourceType Enum
- `0`: File
- `1`: Folder

### SharePermission Enum
- `0`: None
- `1`: View
- `2`: Edit
- `3`: Owner

### Key DTOs

#### LoginDto
- `username`: string
- `password`: string
  
#### UserDto
- `id`: UUID
- `username`: string
- `email`: string
- `createdAt`: datetime
- `lastLoginAt`: datetime

#### FileDto
- `id`: UUID
- `fileName`: string
- `fileExtension`: string
- `fileSize`: long
- `createdAt`: datetime
- `lastModifiedAt`: datetime
- `owner`: string
- `storagePath`: string

#### FolderDto
- `id`: UUID
- `name`: string
- `parentFolderId`: UUID (optional)
- `createdAt`: datetime
- `owner`: string
- `fileCount`: int
- `subfolderCount`: int

## Notes
- All endpoints require JWT authentication
- Timestamps are in ISO 8601 format
- Resource IDs are UUIDs

## Error Handling
- Standard HTTP status codes will be used
- Detailed error messages will be provided in the response body

## Versioning
Current API Version: v1

---

# Deployment Guide

## Prerequisites
- .NET 8.0 SDK
- Azure Subscription
- GitHub Account
- SQL Server or Azure SQL Database

## Configuration Steps

### 1. GitHub Secrets Setup
Configure the following secrets in your GitHub repository:
- `AZURE_WEBAPP_PUBLISH_PROFILE`: Azure Web App publish profile
- `DATABASE_CONNECTION_STRING`: Database connection string
- `AZURE_TENANT_ID`: Azure Active Directory Tenant ID
- `AZURE_APPLICATION_ID`: Application registration ID

### 2. Local Development
1. Clone the repository
2. Install .NET 8.0 SDK
3. Restore dependencies:
   ```bash
   dotnet restore
   ```

### 3. Database Migration
Run database migrations:
```bash
dotnet ef database update
```

### 4. Azure Deployment
The GitHub Actions workflow will automatically:
- Build the application
- Run unit tests
- Migrate the database
- Deploy to Azure App Service

## Environment Configuration
Modify `appsettings.json` or use Azure App Service configuration to set:
- Connection strings
- Azure-specific settings
- Logging levels

## Troubleshooting
- Ensure all GitHub secrets are correctly set
- Verify database connection strings
- Check Azure App Service configuration


