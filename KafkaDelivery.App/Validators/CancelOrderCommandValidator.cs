using FluentValidation;
using KafkaDelivery.App.Commands;
using KafkaDelivery.Domain.Entities;
using KafkaDelivery.Infra.Repositories;

namespace KafkaDelivery.App.Validators;

public class CancelOrderCommandValidator : AbstractValidator<CancelOrderCommand>
{
    public CancelOrderCommandValidator(IRepository<Order> orderRepository)
    {
        RuleFor(x => x.OrderId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("OrderId cannot be null")
            .MustAsync(async (orderId, cancellation) =>
            {
                var orderExists = await orderRepository.GetAsync(o => o.Id == orderId);
                return orderExists != null;
            });
    }
}