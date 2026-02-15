using EmployeeService.DTO;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeService.DAL.EmployeeRepository
{
    public interface IEmployeeRepository
    {
        /// <summary>
        /// Retrives employee by Id with all their subordinates.
        /// </summary>
        /// <param name="id">Employee id</param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns>Employee or null in case of emlpoyee cannot be found.</returns>
        /// <exception cref="Microsoft.Data.SqlClient.SqlException"></exception>
        Task<Employee> GetEmployeeByIdAsync(int id, CancellationToken token);

        /// <summary>
        /// Change enablement of a given employee.
        /// </summary>
        /// <param name="id">Employee id</param>
        /// <param name="enable">New state</param>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns>Return <b>true</b> in case of success, and <b>false</b> in case of user not found</returns>
        /// <exception cref="Microsoft.Data.SqlClient.SqlException"></exception>
        Task<bool> EnableEmployee(int id, bool enable, CancellationToken token);
    }
}