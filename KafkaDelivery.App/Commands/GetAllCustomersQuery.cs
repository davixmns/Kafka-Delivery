using KafkaDelivery.Domain.Entities;
using MediatR;

namespace KafkaDelivery.App.Commands;

public class GetAllCustomersQuery : IRequest<IEnumerable<Customer>> 
{ }