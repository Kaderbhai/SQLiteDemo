using SQLiteDemo.Model;
using System.Collections.Generic;

namespace SQLiteDemo.Data
{
    public interface ICustomerRepository
    {
        Customer GetCustomer(long id);
        List<Customer> GetAllCustomers();
        Customer GetCustomerByName(string firstName, string lastName);
        void SaveCustomer(Customer customer);
        int DeleteCustomer(Customer customer);
        int UpdateCustomer(Customer oldCustomer, Customer newCustomer);
    }
}