using System.IO;
using System.Linq;
using Dapper;
using SQLiteDemo.Model;
using System.Data.SQLite;
using System.Collections.Generic;

namespace SQLiteDemo.Data
{
    public class SqLiteCustomerRepository : SqLiteBaseRepository, ICustomerRepository
    {
        public Customer GetCustomer(long id)
        {
            if (!File.Exists(DbFile)) return null;

            using (var cnn = SimpleDbConnection())
            {
                cnn.Open();
                Customer result = cnn.Query<Customer>(
                    @"SELECT Id, FirstName, LastName, DateOfBirth
                    FROM Customer
                    WHERE Id = @id", new { id }).FirstOrDefault();
                return result;
            }
        }

        public Customer GetCustomerByName(string firstName, string lastName)
        {
            if (!File.Exists(DbFile)) return null;

            using (var cnn = SimpleDbConnection())
            {
                cnn.Open();
                Customer result = cnn.Query<Customer>(
                    @"SELECT Id, FirstName, LastName, DateOfBirth
                    FROM Customer
                    WHERE FirstName = @firstName
                    AND LastName = @lastName", new { firstName, lastName }).FirstOrDefault();
                return result;
            }
        }

        public List<Customer> GetAllCustomers()
        {
            if (!File.Exists(DbFile)) return null;

            using (var cnn = SimpleDbConnection())
            {
                cnn.Open();
                List<Customer> result = cnn.Query<Customer>(
                    @"SELECT *
                    FROM Customer").ToList();
                return result;
            }
        }

        public void SaveCustomer(Customer customer)
        {
            if (!File.Exists(DbFile))
            {
                CreateDatabase();
            }

            using (var cnn = SimpleDbConnection())
            {
                cnn.Open();
                customer.Id = cnn.Query<long>(
                    @"INSERT INTO Customer 
                    ( FirstName, LastName, DateOfBirth ) VALUES 
                    ( @FirstName, @LastName, @DateOfBirth );
                    select last_insert_rowid()", customer).First();
            }
        }

        public int UpdateCustomer(Customer oldCustomer, Customer newCustomer)
        {
            // Check customer exists
            var theOldCustomer = GetCustomerByName(oldCustomer.FirstName, oldCustomer.LastName);
            int updatedCustomer = 0;

            if (theOldCustomer != null)
            {
                var theOldCustomerID = theOldCustomer.Id;
                var theNewCustomerFirstName = newCustomer.FirstName;
                var theNewCustomerLastName = newCustomer.LastName;

                // if they do using execute DELETE query to remove customer
                using (var cnn = SimpleDbConnection())
                {
                    cnn.Open();
                    updatedCustomer = cnn.Execute(
                        @"UPDATE Customer
                        SET FirstName = @theNewCustomerFirstName , LastName = @theNewCustomerLastName
                        WHERE Id = @theOldCustomerID",
                        new { theNewCustomerFirstName, theNewCustomerLastName, theOldCustomerID });
                }

            }

            return updatedCustomer;
        }

        public int DeleteCustomer(Customer customer)
        {
            // Check customer exists
            var theCustomer = GetCustomerByName(customer.FirstName, customer.LastName);
            int result = 0;

            if (theCustomer != null)
            {

                var theCustomerID = theCustomer.Id;

                // if they do using execute DELETE query to remove customer
                using (var cnn = SimpleDbConnection())
                {
                    cnn.Open();
                    result = cnn.Execute(
                        @"DELETE FROM Customer
                        WHERE Id = @theCustomerID;", new { theCustomerID });

                }

            }

            return result;
        }

        private static void CreateDatabase()
        {
            using (var cnn = SimpleDbConnection())
            {
                cnn.Open();
                cnn.Execute(
                    @"create table Customer
                      (
                         ID                                  integer primary key AUTOINCREMENT,
                         FirstName                           varchar(100) not null,
                         LastName                            varchar(100) not null,
                         DateOfBirth                         datetime not null
                      )");
            }
        }
    }
}
