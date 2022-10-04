using FluentValidation;
using MediatR;
using WebApi.Models;
using WebApi.Models.Dto;

namespace WebApi.Core
{
    public class EmployeeCommand : IRequest<GenericResponse<EmployeeDto>>
    {
        public int CompanyId { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int PortalId { get; set; }
        public int RoleId { get; set; }
        public int StatusId { get; set; }
        public string Telephone { get; set; }
        public string Username { get; set; }
    }

    public class EmployeeUpdateCommand : IRequest<GenericResponse<bool>>
    {
        public int EmployeeId { get; set; }
        public EmployeeCommand Employee { get; set; }
    }

    public class EmployeeDeleteCommand : IRequest<GenericResponse<bool>>
    {
        public int EmployeeId { get; set; }
    }

    public class EmployeeCommandValidator : AbstractValidator<EmployeeCommand>
    {
        public EmployeeCommandValidator()
        {
            RuleFor(x => x.CompanyId).NotEmpty();
            RuleFor(x => x.PortalId).NotEmpty();
            RuleFor(x => x.RoleId).NotEmpty();
            RuleFor(x => x.StatusId).NotEmpty();
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Fax).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.Telephone).NotEmpty();
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
        }
    }
}
