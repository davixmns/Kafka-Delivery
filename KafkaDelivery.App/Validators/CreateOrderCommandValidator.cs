using FluentValidation;
using KafkaDelivery.App.Commands;
using KafkaDelivery.Domain.Entities;
using KafkaDelivery.Infra.Repositories;

namespace KafkaDelivery.App.Validators;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator(IRepository<Customer> customerRepository)
    {
        RuleFor(x => x.CustomerId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("CustomerId cannot be null")
            .MustAsync(async (command, customerId, cancellation) =>
            {
                var customerExists = await customerRepository.GetAsync(c => c.Id == customerId);
                
                if (customerExists is null)
                    return false;
                
                command.Customer = customerExists;

                return true;
            });
        
        RuleFor(x => x.Products)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Items cannot be null");
    }
}