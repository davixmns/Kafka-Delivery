using FluentValidation;
using KafkaDelivery.App.Commands;
using KafkaDelivery.App.Dtos;
using KafkaDelivery.Domain.Entities;
using KafkaDelivery.Infra.Repositories;

namespace KafkaDelivery.App.Validators;

public class CancelOrderRequestDtoValidator : AbstractValidator<CancelOrderRequestDto>
{
    public CancelOrderRequestDtoValidator(IRepository<Order> orderRepository)
    {
        RuleFor(x => x.OrderId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("OrderId cannot be null")
            .Must((orderId) =>
            {
                var orderExists = orderRepository.GetAsync(o => o.Id == orderId).Result;
                return orderExists != null;
            });
    }
}