using System.Collections;
using KafkaDelivery.Domain.Entities;

namespace KafkaDelivery.Infra.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private static IList<Customer> _customers = [];
    
    public IList<Customer> GetAll()
    {
        return _customers;
    }

    public Customer? GetById(string customerId)
    {
        return _customers.FirstOrDefault(x => x.CustomerId == customerId);
    }

    public void Save(Customer customer)
    {
        _customers.Add(customer);
    }

    public void Update(Customer customer)
    {
        throw new NotImplementedException();
    }

    public void Delete(string customerId)
    {
        throw new NotImplementedException();
    }
}