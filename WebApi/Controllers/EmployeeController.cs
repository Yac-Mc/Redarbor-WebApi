using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Core;
using WebApi.Models;
using WebApi.Models.Dto;
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
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public EmployeeController(IGenericRepository<Employee> employee, IMediator mediator, IMapper mapper)
        {
            _employee = employee;
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<GenericResponse<IEnumerable<EmployeeDto>>> Get()
        {
            return new GenericResponse<IEnumerable<EmployeeDto>>()
            {
                Result = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeDto>>(await _employee.GetAllAsync())
            };
        }

        [HttpGet("{id}")]
        public async Task<GenericResponse<IEnumerable<EmployeeDto>>> Get(int id)
        {
            return new GenericResponse<IEnumerable<EmployeeDto>>()
            {
                Result = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeDto>>(await _employee.GetAllByFilterAsync(GetFilterByEmployeeId(id)))
            };
        }

        [HttpGet("GetAllWithPagination")]
        public async Task<GenericResponse<Pagination<EmployeeDto>>> GetAllWithPagination()
        {
            var pagination = new Pagination<Employee>();
            return new GenericResponse<Pagination<EmployeeDto>>()
            {
                Result = _mapper.Map<Pagination<Employee>, Pagination<EmployeeDto>>(await _employee.GetAllPaginationByFilterAsync(pagination))
            };
        }

        [HttpGet("GetAllWithPaginationById/{id}")]
        public async Task<GenericResponse<Pagination<EmployeeDto>>> GetAllWithPaginationById(int id)
        {
            var pagination = new Pagination<Employee>() { Filters = GetFilterByEmployeeId(id) };
            return new GenericResponse<Pagination<EmployeeDto>>()
            {
                Result = _mapper.Map<Pagination<Employee>, Pagination<EmployeeDto>>(await _employee.GetAllPaginationByFilterAsync(pagination))
            };
        }

        [HttpPost]
        public async Task<GenericResponse<EmployeeDto>> Post(EmployeeCommand employee)
        {
            return await _mediator.Send(employee);
        }

        [HttpPut("{id}")]
        public async Task<GenericResponse<bool>> Put(int id, EmployeeCommand employee)
        {
            EmployeeUpdateCommand emplyeeUpdate = new EmployeeUpdateCommand()
            {
                EmployeeId = id,
                Employee = employee
            };
            return await _mediator.Send(emplyeeUpdate);
        }

        [HttpDelete("{id}")]
        public async Task<GenericResponse<bool>> Delete(int id)
        {
            EmployeeDeleteCommand emplyeeDelete = new EmployeeDeleteCommand() { EmployeeId = id };
            return await _mediator.Send(emplyeeDelete);
        }

        private List<Filter> GetFilterByEmployeeId(int employeeId) => new List<Filter> { new Filter() { Column = "EmployeeId", Operator = "=", Value = employeeId.ToString() } };
    }
}
