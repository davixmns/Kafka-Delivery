using FluentValidation;
using KafkaDelivery.App.Commands;
using KafkaDelivery.App.Dtos;
using KafkaDelivery.Domain.Entities;
using KafkaDelivery.Infra.Repositories;

namespace KafkaDelivery.App.Validators;

public class CreateCustomerRequestDtoValidator : AbstractValidator<CreateCustomerRequestDto>
{
    public CreateCustomerRequestDtoValidator(IRepository<Customer> customerRepository)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name cannot be null");

        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Email cannot be null")
            .Must((email) =>
            {
                var customerExists = customerRepository.GetAsync(c => c.Email == email).Result;
                return customerExists == null;
            }).WithMessage("Customer already exists");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("PhoneNumber cannot be null");
    }
}