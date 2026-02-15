USE EmployeesServiceDB;
GO
DROP PROCEDURE IF EXISTS dbo.EnableEmployee;
GO

CREATE PROCEDURE dbo.EnableEmployee
    @EmployeeId INT,
    @Enabled BIT
AS
BEGIN

    UPDATE Employees
    SET Enabled = @Enabled
    WHERE Id = @EmployeeId;

END
GO