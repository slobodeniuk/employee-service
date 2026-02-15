$containerName =  "mssql-employee";
$dataBase = "EmployeesServiceDB"
$connectionString = "Data Source=localhost;Initial Catalog=EmployeesServiceDB;User ID=sa;Password=pass@word1;Trust Server Certificate=True";
$masterConnectionString = "Data Source=localhost;Initial Catalog=master;User ID=sa;Password=pass@word1;Trust Server Certificate=True";


if(!(docker inspect $containerName -f '{{.State.Status}}').StartsWith("error"))
{
    docker stop $containerName;
    docker rm $containerName;
}

docker run --name=$containerName -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=pass@word1" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest

$continuePoll = $true;
while($continuePoll -eq $true)
{
    try
    {
        (Invoke-Sqlcmd -Query "select 1 from master" -ConnectionString $masterConnectionString) 2> $null
        $continuePoll = $false
    }
    catch
    {
        $continuePoll = $true
        echo "DB is not ready, waiting..."
        Start-Sleep -Seconds 5
    }
}


echo "Creating EmployeesDB and seeding with initial data..."
Invoke-Sqlcmd -InputFile ./CreateDBInsertValuesScript.sql -ConnectionString $masterConnectionString

echo "Creating GetEmployeeWithHierarchyById stored procedure..."
Invoke-Sqlcmd -InputFile ./dbo.GetEmployeeWithHierarchyById.sql -ConnectionString $connectionString
echo "Creating EnableEmployee stored procedure..."
Invoke-Sqlcmd -InputFile ./dbo.EnableEmployee.sql -ConnectionString $connectionString

echo "DB Initialization has been finished successfully"
