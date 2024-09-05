using FluentValidation;
using KafkaDelivery.App.Commands;
using KafkaDelivery.Infra.Repositories;

namespace KafkaDelivery.App.Validators;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator(ICustomerRepository customerRepository)
    {
        RuleFor(x => x.CustomerId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("CustomerId cannot be null")
            .Must(((command, customerId) =>
            {
                var customer = customerRepository.GetById(customerId);
                if (customer is null)
                    return false;
                command.Customer = customer;
                return true;
            })).WithMessage("Customer not found");
        
        RuleFor(x => x.Items)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Items cannot be null");
    }
}