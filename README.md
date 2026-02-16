# WCF REST API With SQL Test Interview Project

## A functional test project written in C#
This project implement 2  REST API functions.
**GetEmplyeeByID** – should return an Employee in a tree structure, like on a picture in archive.
**EnableEmployee** – should change ‘Enable’ flag for the Employee.

Requirement: not to use EntityFramework, only ADO.NET



## Pre-requirements:
- Docker desktop
- Postman (optional, for testing API)

## Installation:

#### 1. Open PowerShell as administrator and run: `Install-Module SqlServer`

#### 2. **Database Initialization**

**Option #1. Using PowerShell script (Recommended)**

To initialize the SQL Server Docker container and database, run from project root:   
    ```
    powershell .\Scripts\DbInit.ps1
    ```
    
    The script will:
    1. Pull SQL Server 2022 image
    2. Create and start container mssql-employee
    3. Create database EmployeeServiceDB
    4. Create table Employees
    5. Insert initial data
    6. Create required stored procedures

**Option 2. Using SQL Server Management Studio**

* Connect to:
    ```
    Server: localhost
    Login: sa
    Password: pass@word1
    ```
+ Open Scripts folder
+ Run scripts in following order:
    - CreateDBInsertValuesScript.sql
    - dbo.EnableEmployee.sql
    - dbo.GetEmployeeWithHierarchyById.sql

#### 3. **Configure Connection String**

In web.config
``` xml
<connectionStrings>
  <add name="EmployeeDB"
       connectionString="Data Source=localhost;
                        Initial Catalog=EmployeesServiceDB;
                        User ID=sa;
                        Password=pass@word1;
                        Trust Server Certificate=True"
    />
</connectionStrings>
```
>`Trust Server Certificate=True` is required for local docker

#### 4. **Run the Project**

Example of running service address: 
`http://localhost:64014/EmployeeService.svc`

## Example API Call
#### **1. GetEmployeeById**

Retrieves an Employee in a tree structure including all deputies.

+ **Method**: `Get`
+ **Url**: `/EmployeeService.svc/GetEmployeeById?id={}`
+ **Parameter**: `id` - type `int` - employee identifier

>Example: http://localhost:64014/EmployeeService.svc/GetEmployeeById?id=3

**Success Response**
`✅ 200 OK`

**Error Responses:**
+ `404 Not Found`
Returned when employee with specified id does not exist.
+ `400 Bad Request`
Returned when id is not a valid integer.

#### **2. EnableEmployee**

Updates employee status according to the parameter enable. Can be set to true or false.
+ **Method**: `PUT`
+ **Url**: `/EmployeeService.svc/EnableEmployee`
+ **Headers**: 
    - Content-Type: `application/json`
+ **Parameters**: `Body (JSON)`
```json
{
  "id": 5,
  "enable": true
}
```
| Name     | Type      | Requered | Description                    |
| ---      | ---       | ---      | ---                            |
| id       | int       | yes      | employee identifier            |
| enable   | bool      | yes      | true = enable, false = disable |

**Success Response**
`✅ 200 OK`

**Error responses:**
+ `404 Not Found`
Returned when employee with specified id does not exist.