using FluentValidation;
using KafkaDelivery.App.Commands;
using KafkaDelivery.Domain.Entities;
using KafkaDelivery.Infra.Repositories;

namespace KafkaDelivery.App.Validators;

public class PayOrderCommandValidator : AbstractValidator<PayOrderCommand>
{
    public PayOrderCommandValidator(IRepository<Order> orderRepository)
    {
        RuleFor(x => x.OrderId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("OrderId cannot be null")
            .MustAsync(async (orderId, cancellation) =>
            {
                var order = await orderRepository.GetAsync(o => o.Id == orderId);
                return order != null;
            }).WithMessage("Order not found");

        RuleFor(x => x.PaymentMethod)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("PaymentMethod cannot be null")
            .Must(((paymentMethod) => Enum.TryParse<Payments>(paymentMethod, out _))).WithMessage("Invalid PaymentMethod");
    }
}