using FluentValidation;
using KafkaDelivery.App.Commands;
using KafkaDelivery.App.Dtos;
using KafkaDelivery.Domain.Entities;
using KafkaDelivery.Infra.Repositories;

namespace KafkaDelivery.App.Validators;

public class CreateOrderRequestDtoValidator : AbstractValidator<CreateOrderRequestDto>
{
    public CreateOrderRequestDtoValidator(IRepository<Customer> customerRepository)
    {
        RuleFor(x => x.CustomerId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("CustomerId cannot be null")
            .Must((customerId) =>
            {
                var customerExists = customerRepository.GetAsync(c => c.Id == customerId).Result;
                return customerExists != null;
            }).WithMessage("Customer does not exist");
        
        RuleFor(x => x.Products)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Items cannot be null");
    }
}