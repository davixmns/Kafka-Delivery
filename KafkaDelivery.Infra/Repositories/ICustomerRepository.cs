using KafkaDelivery.Domain.Entities;

namespace KafkaDelivery.Infra.Repositories;

public interface ICustomerRepository
{
    IList<Customer> GetAll();
    Customer? GetById(string customerId);
    void Save(Customer customer);
    void Update(Customer customer);
    void Delete(string customerId);
}