-- Create Database
CREATE DATABASE EmployeesServiceDB;
GO

USE EmployeesServiceDB;
GO

-- Create Employees table
CREATE TABLE Employees (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    ManagerId INT NULL,
    Enabled BIT NOT NULL,
    CONSTRAINT FK_Employees_Manager
        FOREIGN KEY (ManagerId) REFERENCES Employees(Id)
);
GO

-- Insert Employees
-- Top level manager (no manager)
INSERT INTO Employees (Name, ManagerId, Enabled) VALUES ('John Smith', NULL, 1);

-- Level 2 managers
INSERT INTO Employees (Name, ManagerId, Enabled) VALUES
('Sarah Johnson', 1, 1),
('Michael Brown', 1, 1),
('Emily Davis', 1, 1);

-- Level 3 employees
INSERT INTO Employees (Name, ManagerId, Enabled) VALUES
('David Wilson', 2, 1),
('Jessica Miller', 2, 1),
('Daniel Moore', 3, 1),
('Laura Taylor', 3, 1),
('James Anderson', 4, 1),
('Olivia Thomas', 4, 1);

-- More employees
INSERT INTO Employees (Name, ManagerId, Enabled) VALUES
('William Jackson', 2, 1),
('Sophia White', 2, 0),
('Benjamin Harris', 3, 1),
('Isabella Martin', 3, 1),
('Lucas Thompson', 4, 1),
('Mia Garcia', 4, 0),
('Henry Martinez', 5, 1),
('Charlotte Robinson', 5, 1),
('Alexander Clark', 6, 1),
('Amelia Rodriguez', 6, 1),
('Ethan Lewis', 7, 1),
('Harper Lee', 7, 0),
('Sebastian Walker', 8, 1),
('Evelyn Hall', 8, 1),
('Jack Allen', 9, 1),
('Avery Young', 9, 1),
('Owen Hernandez', 10, 1),
('Ella King', 10, 1),
('Matthew Wright', 11, 1),
('Scarlett Lopez', 11, 0);

-- Broken data
INSERT INTO Employees (Name, ManagerId, Enabled) VALUES
('Broken Manager', null, 1),
('Broken Employee', null, 1);

GO
UPDATE Employees
	SET ManagerId = (SELECT TOP 1 e.Id FROM Employees e WHERE e.Name = 'Broken Employee')
	WHERE Name = 'Broken Manager';

	UPDATE Employees
	SET ManagerId = (SELECT TOP 1 e.Id FROM Employees e WHERE e.Name = 'Broken Manager')
	WHERE Name = 'Broken Employee';
GO
-- Broken data2
INSERT INTO Employees (Name, ManagerId, Enabled) VALUES
('Broken Self Manager', null, 1),
('Broken Employee of Self Manager', null, 1);

GO
UPDATE Employees
	SET ManagerId = (SELECT TOP 1 e.Id FROM Employees e WHERE e.Name = 'Broken Self Manager')
	WHERE Name = 'Broken Self Manager';

	UPDATE Employees
	SET ManagerId = (SELECT TOP 1 e.Id FROM Employees e WHERE e.Name = 'Broken Self Manager')
	WHERE Name = 'Broken Employee of Self Manager';
GO