using DotnetApiTemplate.WebApi.Endpoints.Event.Request;
using DotnetApiTemplate.WebApi.Endpoints.UserManagement.Requests;
using DotnetApiTemplate.WebApi.Validators;
using FluentValidation;

namespace DotnetApiTemplate.WebApi.Endpoints.Event.Validator
{
  public class CreateEventValidator : AbstractValidator<CreateEventRequest>
  {
    public CreateEventValidator()
    {
      RuleFor(e => e.Name).NotNull().NotEmpty();
      RuleFor(e => e.CountTicket).NotNull();
    }
  }
}
