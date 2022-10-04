using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Models.Entities;
using WebApi.Models.Queries;
using WebApi.Repositories;

namespace WebApi.Core
{
    public class Update
    {
        public class EmployeeUpdateHandler : IRequestHandler<EmployeeUpdateCommand, GenericResponse<bool>>
        {
            private readonly IGenericRepository<Employee> _employee;
            private readonly IMapper _mapper;

            public EmployeeUpdateHandler(IGenericRepository<Employee> employee, IMapper mapper)
            {
                _employee = employee;
                _mapper = mapper;
            }
            public async Task<GenericResponse<bool>> Handle(EmployeeUpdateCommand request, CancellationToken cancellationToken)
            {
                var resultValidate = ValidateEmployee(request.EmployeeId, request.Employee.Username);
                if (resultValidate.result)
                {
                    var employee = _mapper.Map<EmployeeCommand, Employee>(request.Employee);
                    employee.UpdatedOn = DateTime.Now;
                    employee.CreatedOn = resultValidate.oldEmployee.CreatedOn;
                    await _employee.UpdateByIdAsync(employee, request.EmployeeId.ToString());
                }

                return new GenericResponse<bool>()
                {
                    Message = resultValidate.result ? "Empleado actualizado con éxito!!!!" : resultValidate.message,
                    Result = resultValidate.result
                };
            }

            private (bool result, string message, Employee oldEmployee) ValidateEmployee(int employeeId, string username)
            {
                var employeeByEmployeeId = _employee.GetAllByFilterAsync(GetFilterByEmployeeId(employeeId)).Result.FirstOrDefault();
                if (employeeByEmployeeId == null)
                {
                    return (false, $"El empleado con Id {employeeId} no existe!", new Employee());
                }
                var employeeByUserName = _employee.GetAllByFilterAsync(GetFilterByUserName(username)).Result.FirstOrDefault();
                if (employeeByUserName != null && employeeByUserName.EmployeeId != employeeId)
                {
                    return (false, $"Ya existe un empleado con el Username {username}", new Employee());
                }

                return (true, "", employeeByEmployeeId);
            }

            private List<Filter> GetFilterByUserName(string username) => new List<Filter> { new Filter() { Column = "Username", Operator = "=", Value = username } };
            private List<Filter> GetFilterByEmployeeId(int employeeId) => new List<Filter> { new Filter() { Column = "EmployeeId", Operator = "=", Value = employeeId.ToString() } };
        }
    }
}
