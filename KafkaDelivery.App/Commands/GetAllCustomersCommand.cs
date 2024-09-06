using KafkaDelivery.Domain.Entities;
using MediatR;

namespace KafkaDelivery.App.Commands;

public class GetAllCustomersCommand : IRequest<IEnumerable<Customer>> 
{ }