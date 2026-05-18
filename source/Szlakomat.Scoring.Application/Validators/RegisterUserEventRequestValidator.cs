using FluentValidation;
using Szlakomat.Scoring.Application.DTO;

namespace Szlakomat.Scoring.Application.Validators;

public class RegisterUserEventRequestValidator
    : AbstractValidator<RegisterUserEventRequest>
{
    public RegisterUserEventRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.Category)
            .NotEmpty();

        RuleFor(x => x.EventType)
            .NotEmpty();
    }
}