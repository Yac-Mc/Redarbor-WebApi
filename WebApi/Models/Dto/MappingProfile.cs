using AutoMapper;
using WebApi.Core;
using WebApi.Models.Entities;
using WebApi.Models.Queries;

namespace WebApi.Models.Dto
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Employee, EmployeeDto>();
            CreateMap<EmployeeCommand, Employee>();
            CreateMap<Pagination<Employee>, Pagination<EmployeeDto>>();
        }
    }
}
