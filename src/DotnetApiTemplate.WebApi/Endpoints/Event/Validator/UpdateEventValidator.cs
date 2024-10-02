using DotnetApiTemplate.WebApi.Endpoints.Event.Request;
using FluentValidation;

namespace DotnetApiTemplate.WebApi.Endpoints.Event.Validator
{
  public class UpdateEventValidator : AbstractValidator<UpdateEventRequest>
  {
    public UpdateEventValidator()
    {
      RuleFor(e => e.EventId).NotNull().NotEmpty();
      RuleFor(e => e.Name).NotNull().NotEmpty();
      RuleFor(e => e.Lokasi).NotNull().NotEmpty();
      RuleFor(e => e.JumlahTiket).NotNull();
    }
  }
}
