using EmployeeService.DAL.EmployeeRepository;
using System.Net;
using System.ServiceModel.Web;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeService
{
    public class EmployeeServiceImpl : IEmployeeService
    {
        public async Task<EmployeeDto> GetEmployeeById(int id)
        {
            var repo = DI.Instance.Get<IEmployeeRepository>();

            var employee = await repo.GetEmployeeByIdAsync(id, CancellationToken.None);

            if (employee == null)
            {
                //this means that there's no employee with such id in db
                //and the repository returned nothing
                throw new WebFaultException<string>(
                    "Employee not found",
                    HttpStatusCode.NotFound);
            }

            return EmployeeDto.MapToEmployeeDto(employee);
        }

        public async Task EnableEmployee(int id, bool enable)
        {
            var repo = DI.Instance.Get<IEmployeeRepository>();

            if (!await repo.EnableEmployee(id, enable, CancellationToken.None))
            {
                //if the method returns false, means that no rows were affected in db 
                //in other cases the other exceptions will be thrown
                throw new WebFaultException<string>(
                    "User not found",
                    HttpStatusCode.NotFound);
            }
        }
    }
}