using FluentValidation;
using KafkaDelivery.App.Commands;
using KafkaDelivery.Infra.Repositories;

namespace KafkaDelivery.App.Validators;

public class CancelOrderCommandValidator : AbstractValidator<CancelOrderCommand>
{
    public CancelOrderCommandValidator(IOrderRepository orderRepository)
    {
        RuleFor(x => x.OrderId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("OrderId cannot be null")
            .Must(((command, orderId) =>
            {
                var order = orderRepository.GetById(orderId);
                if (order is null)
                    return false;
                command.Order = order;
                return true;
            })).WithMessage("Order not found");
    }
}