using EmployeeService.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel.Web;

namespace EmployeeService
{
    [DataContract]
    public class EmployeeDto
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember(Order = 2)]
        public string Name { get; set; }

        [DataMember(Order = 3)]
        public int? ManagerId { get; set; }

        [DataMember(Order = 4)]
        public bool Enabled { get; set; }

        [DataMember(Order = 5, EmitDefaultValue = false)]
        public List<EmployeeDto> Employees { get; set; }

        public static EmployeeDto MapToEmployeeDto(Employee employee)
        {
            return new EmployeeDto
            {
                Id = employee.Id,
                Name = employee.Name,
                ManagerId = employee.ManagerId,
                Enabled = employee.Enabled,
                Employees = employee.Employees?.Select(MapToEmployeeDto)?.ToList()
            };
        }
    }
}