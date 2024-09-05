using FluentValidation;
using KafkaDelivery.App.Commands;
using KafkaDelivery.Domain.Entities;
using KafkaDelivery.Infra.Repositories;

namespace KafkaDelivery.App.Validators;

public class PayOrderCommandValidator : AbstractValidator<PayOrderCommand>
{
    public PayOrderCommandValidator(IOrderRepository orderRepository)
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
        
        RuleFor(x => x.PaymentMethod)
            .NotEmpty().WithMessage("PaymentMethod cannot be null")
            .Must((paymentMethod => Enum.TryParse<Payments>(paymentMethod, out _))).WithMessage("Invalid PaymentMethod");
    }
}