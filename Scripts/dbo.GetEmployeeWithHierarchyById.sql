DROP PROCEDURE IF EXISTS dbo.GetEmployeeWithHierarchyById;
GO

CREATE PROCEDURE dbo.GetEmployeeWithHierarchyById
	@ManagerId INT
AS
BEGIN	
	WITH EmployeeHierarhy AS (
		SELECT e.*,
		0 as Level,
		CAST(',' + CAST (e.Id as NVARCHAR)  + ',' as NVARCHAR)  AS Visited
		FROM Employees e
		WHERE e.Id = @ManagerId

		UNION ALL
	
		SELECT
			e.*,
			eh.Level + 1 as Level,
			CAST(eh.Visited + CAST (e.Id as NVARCHAR) + ',' as NVARCHAR) AS Visited
		FROM Employees e
		INNER JOIN EmployeeHierarhy eh ON eh.Id = e.ManagerId
		WHERE eh.Visited NOT LIKE '%,' + CAST (e.Id as NVARCHAR)  + ',%'
	)
	SELECT
	 eh.Id,
	 eh.Name,
	 eh.ManagerId,
	 eh.Enabled
	FROM EmployeeHierarhy eh
	ORDER BY eh.Level;

END
GO