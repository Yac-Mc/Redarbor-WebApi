using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Models.Entities;
using WebApi.Models.Queries;
using WebApi.Repositories;

namespace WebApi.Controllers
{
    [Route("api/redarbor")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IGenericRepository<Employee> _employee;

        public EmployeeController(IGenericRepository<Employee> employee)
        {
            _employee = employee;
        }

        [HttpGet]
        public async Task<GenericResponse<IEnumerable<Employee>>> Get()
        {
            GenericResponse<IEnumerable<Employee>> result = new GenericResponse<IEnumerable<Employee>>()
            {
                Result = await _employee.GetAllAsync()
            };

            return result;
        }

        [HttpGet("{id}")]
        public async Task<GenericResponse<IEnumerable<Employee>>> Get(int id)
        {
            List<Filter> listFilters = new List<Filter>
            {
                new Filter() { Order = 1, Column = "EmployeeId", Operator = "=", Value = id.ToString() }
            };
            GenericResponse<IEnumerable<Employee>> result = new GenericResponse<IEnumerable<Employee>>()
            {
                Result = await _employee.GetAllByFilterAsync(listFilters)
            };

            return result;
        }

        [HttpGet("GetAllWithPagination")]
        public async Task<GenericResponse<Pagination<Employee>>> GetAllWithPagination()
        {
            var pagination = new Pagination<Employee>();
            GenericResponse<Pagination<Employee>> result = new GenericResponse<Pagination<Employee>>()
            {
                Result = await _employee.GetAllPaginationByFilterAsync(pagination)
            };

            return result;
        }

        [HttpGet("GetAllWithPaginationById/{id}")]
        public async Task<GenericResponse<Pagination<Employee>>> GetAllWithPaginationById(int id)
        {
            List<Filter> listFilters = new List<Filter>
            {
                new Filter() { Order = 1, Column = "EmployeeId", Operator = "=", Value = id.ToString() }
            };
            var pagination = new Pagination<Employee>()
            {
                Filters = listFilters
            };
            GenericResponse<Pagination<Employee>> result = new GenericResponse<Pagination<Employee>>()
            {
                Result = await _employee.GetAllPaginationByFilterAsync(pagination)
            };

            return result;
        }

        [HttpPost]
        public async Task<GenericResponse<bool>> Post(Employee employee)
        {
            bool exists = ValidateEmployee(employee);

            if (!exists)
                await _employee.InsertAsync(employee);

            GenericResponse<bool> result = new GenericResponse<bool>()
            {
                Message = exists ? "El nombre de usuario ya existe!" : "Usuario creado con éxito!!!",
                Result = !exists
            };

            return result;
        }

        [HttpPut("{id}")]
        public async Task<GenericResponse<bool>> Put(int id, Employee employee)
        {
            await _employee.UpdateByIdAsync(employee, id.ToString());
            GenericResponse<bool> result = new GenericResponse<bool>()
            {
                Message = "El registro ha sido actualizado con éxito",
                Result = true
            };
            return result;
        }

        [HttpDelete("{id}")]
        public async Task<GenericResponse<bool>> Delete(int id)
        {
            List<Filter> listFilters = new List<Filter>
            {
                new Filter() { Order = 1, Column = "EmployeeId", Operator = "=", Value = id.ToString() }
            };
            await _employee.DeleteByIdAsync(listFilters);
            GenericResponse<bool> result = new GenericResponse<bool>()
            {
                Message = "El registro ha sido eliminado con éxito",
                Result = true
            };
            return result;
        }

        private bool ValidateEmployee(Employee employee)
        {
            List<Filter> listFilters = new List<Filter>
            {
                new Filter() { Column = "Username", Operator = "=", Value = employee.Username}
            };

            return _employee.GetAllByFilterAsync(listFilters).Result.Any();
        }
    }
}
