using MediatR;
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
    public class Delete
    {
        public class EmployeeDeleteHandler : IRequestHandler<EmployeeDeleteCommand, GenericResponse<bool>>
        {
            private readonly IGenericRepository<Employee> _employee;

            public EmployeeDeleteHandler(IGenericRepository<Employee> employee)
            {
                _employee = employee;
            }
            public async Task<GenericResponse<bool>> Handle(EmployeeDeleteCommand request, CancellationToken cancellationToken)
            {
                var exists = ExistsEmployee(request.EmployeeId);
                if (exists.result)
                    await _employee.DeleteByIdAsync(exists.filter);

                return new GenericResponse<bool>()
                {
                    Message = exists.result ? "Empleado eliminado con éxito!!!!" : $"El empleado con id {request} no existe.",
                    Result = exists.result
                };
            }

            private (bool result, List<Filter> filter) ExistsEmployee(int employeeId) {
                var filter = GetFilterByEmployeeId(employeeId);

                return (_employee.GetAllByFilterAsync(filter).Result.Any(), filter);
            }

            private List<Filter> GetFilterByEmployeeId(int employeeId) => new List<Filter> { new Filter() { Column = "EmployeeId", Operator = "=", Value = employeeId.ToString() } };

        }
    }
}
