using EmployeeService.DTO;
using System.Data;
using Microsoft.Data.SqlClient;
using EmployeeService.DAL.Configs;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeService.DAL.EmployeeRepository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeRepositorySettings employeeRepositorySettings;

        public EmployeeRepository(EmployeeRepositorySettings employeeRepositorySettings)
        {
            this.employeeRepositorySettings = employeeRepositorySettings;
        }

        #region implementation of IEmployeeRepository
        /// <inheritdoc/>
        public async Task<Employee> GetEmployeeByIdAsync(int id, CancellationToken token)
        {
            var parameters = new[] { new SqlParameter("@ManagerId", id) };
            var data = await GetQueryResultAsync("dbo.GetEmployeeWithHierarchyById", parameters, token);
            var employees = new List<Employee>();

            foreach (DataRow item in data.Rows)
            {
                employees.Add(ReadEmployee(item));
            }

            var employee = employees.FirstOrDefault();
            if (employee != null)
            {
                employee.Employees = GetSubordinates(employee, employees);
            }

            return employee;
        }

        /// <inheritdoc/>
        public async Task<bool> EnableEmployee(int id, bool enable, CancellationToken token)
        {

            var parameters = new[] { new SqlParameter("@EmployeeId", id), new SqlParameter("@Enabled", enable) };

            var rowsAffected = await ExecuteNonQueryAsync("dbo.EnableEmployee", parameters, token);

            return rowsAffected > 0;
        }

        #endregion


        #region PrivateMethods

        private List<Employee> GetSubordinates(Employee employee, List<Employee> allEmployees, List<int> path = null)
        {
            if (path == null)
            {
                path = new List<int>();
            }
            path.Add(employee.Id);

            var employees = allEmployees.Where(e => !path.Contains(e.Id) && e.ManagerId == employee.Id).ToList();
            if (employees.Count == 0)
            {
                return null;
            }

            foreach (var e in employees)
            {
                {
                    e.Employees = GetSubordinates(e, allEmployees, path);
                }

            }

            return employees;
        }

        private Employee ReadEmployee(DataRow dataRow)
        {
            return new Employee
            {
                Id = dataRow.Field<int>("Id"),
                Name = dataRow.Field<string>("Name"),
                Enabled = dataRow.Field<bool>("Enabled"),
                ManagerId = dataRow.Field<int?>("ManagerId")
            };
        }

        private async Task<DataTable> GetQueryResultAsync(string query, SqlParameter[] sqlParameters, CancellationToken token)
        {
            var dt = new DataTable();

            using (var connection = new SqlConnection(employeeRepositorySettings.ConnectionString))
            {
                await connection.OpenAsync(token);

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.CommandType = CommandType.StoredProcedure;

                    foreach (var parameter in sqlParameters)
                    {
                        command.Parameters.Add(parameter);
                    }

                    using (var reader = await command.ExecuteReaderAsync(token))
                    {
                        dt.Load(reader);
                    }
                }
            }

            return dt;
        }

        private async Task<int> ExecuteNonQueryAsync(string query, SqlParameter[] sqlParameters, CancellationToken token)
        {
            using (var connection = new SqlConnection(employeeRepositorySettings.ConnectionString))
            {
                await connection.OpenAsync(token);

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.CommandType = CommandType.StoredProcedure;

                    foreach (var parameter in sqlParameters)
                    {
                        command.Parameters.Add(parameter);
                    }

                    return await command.ExecuteNonQueryAsync(token);
                }
            }
        }
#endregion
    }
}