using FluentValidation;
using KafkaDelivery.App.Commands;
using KafkaDelivery.App.Dtos;
using KafkaDelivery.Domain.Entities;
using KafkaDelivery.Domain.ValueObjects;
using KafkaDelivery.Infra.Repositories;

namespace KafkaDelivery.App.Validators;

public class PayOrderRequestDtoValidator : AbstractValidator<PayOrderRequestDto>
{
    public PayOrderRequestDtoValidator(IRepository<Order> orderRepository)
    {
        RuleFor(x => x.OrderId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("OrderId cannot be null")
            .Must((orderId) =>
            {
                var order = orderRepository.GetAsync(o => o.Id == orderId).Result;
                return order != null;
            }).WithMessage("Order not found");

        RuleFor(x => x.PaymentMethod)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("PaymentMethod cannot be null")
            .Must(((paymentMethod) => Enum.TryParse<Payments>(paymentMethod, out _))).WithMessage("Invalid PaymentMethod");
    }
}