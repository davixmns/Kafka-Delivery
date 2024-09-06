using FluentValidation;
using KafkaDelivery.App.Commands;

namespace KafkaDelivery.App.Validators;

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.Email)
            .NotEmpty();

        RuleFor(x => x.PhoneNumber)
            .NotEmpty();
    }
}