using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Models.Dto;
using WebApi.Models.Entities;
using WebApi.Models.Queries;
using WebApi.Repositories;

namespace WebApi.Core
{
    public class Register
    {
        public class EmployeeRegisterHandler : IRequestHandler<EmployeeCommand, GenericResponse<EmployeeDto>>
        {
            private readonly IGenericRepository<Employee> _employee;
            private readonly IMapper _mapper;

            public EmployeeRegisterHandler(IGenericRepository<Employee> employee, IMapper mapper)
            {
                _employee = employee;
                _mapper = mapper;
            }

            public async Task<GenericResponse<EmployeeDto>> Handle(EmployeeCommand request, CancellationToken cancellationToken)
            {
                EmployeeDto employeeDto = new EmployeeDto();
                bool exists = ExistsEmployee(request);

                if (!exists)
                {
                    var employee = _mapper.Map<EmployeeCommand, Employee>(request);
                    await _employee.InsertAsync(employee);
                    employeeDto = _mapper.Map<Employee, EmployeeDto>(_employee.GetAllByFilterAsync(GetFilterByUserName(employee.Username)).Result.FirstOrDefault());
                }
                return new GenericResponse<EmployeeDto>()
                {
                    Message = exists ? "El nombre de usuario ya existe!" : "Empleado creado con éxito!!!",
                    Result = employeeDto
                };
            }

            private bool ExistsEmployee(EmployeeCommand employee) => _employee.GetAllByFilterAsync(GetFilterByUserName(employee.Username)).Result.Any();

            private List<Filter> GetFilterByUserName(string username) => new List<Filter>{ new Filter() { Column = "Username", Operator = "=", Value = username } };
        }
    }
}
