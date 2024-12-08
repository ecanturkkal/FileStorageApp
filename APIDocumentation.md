# File Storage App API Documentation

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
