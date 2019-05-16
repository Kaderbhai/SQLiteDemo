using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLiteDemo.Data;
using SQLiteDemo.Model;

namespace SQLiteDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            string CRUD;
            Console.WriteLine($"Please choose functionality from the list below {Environment.NewLine}");
            Console.WriteLine($"Create,");
            Console.WriteLine($"Get (Customer List)");
            Console.WriteLine($"Update,");
            Console.WriteLine($"Delete{Environment.NewLine}");

            CRUD = Console.ReadLine();

            switch (CRUD.ToUpper())
            {
                case "CREATE":
                    Console.WriteLine("Create and save a customer to database");
                    SaveCustomer();
                    break;
                case "GET":
                    Console.WriteLine("Get list of customers from database");
                    GetCustomerList();
                    break;
                case "UPDATE":
                    Console.WriteLine("Update customer from database");
                    UpdateCustomer();
                    break;
                case "DELETE":
                    Console.WriteLine("Delete a customer from the database");
                    DeleteCustomer();
                    break;
                default:
                    Console.WriteLine("Well that went tits up, oh well its all shits and gigs!");
                    break;
            }

            Console.WriteLine("Press return to exit");
            Console.ReadKey();
        }

        public static void SaveCustomer()
        {
            string firstName;
            string lastName;
            string yesOrNo;

            Console.WriteLine($"Choose first name:");
            firstName = Console.ReadLine();

            Console.WriteLine($"Choose second name:");
            lastName = Console.ReadLine();

            Console.WriteLine($"Do you want to add this customer to the most excellent sqlite DB?{Environment.NewLine}");
            Console.WriteLine($"Y/N{Environment.NewLine}");
            yesOrNo = Console.ReadLine();


            if (yesOrNo.ToUpper() == "Y")
            {
                ICustomerRepository rep = new SqLiteCustomerRepository();
                var customer = new Customer
                {
                    FirstName = firstName,
                    LastName = lastName,
                    DateOfBirth = DateTime.Now
                };
                rep.SaveCustomer(customer);

                Customer retrievedCustomer = rep.GetCustomer(customer.Id);

                Console.WriteLine($"Customer {firstName} {lastName} has been added to the sqliteDB!!!");
            }
        }

        public static void DeleteCustomer()
        {
            string firstName;
            string lastName;
            string yesOrNo;

            Console.WriteLine($"Choose first name:");
            firstName = Console.ReadLine();

            Console.WriteLine($"Choose second name:");
            lastName = Console.ReadLine();

            Console.WriteLine($"Do you want to delete this customer to the most excellent sqlite DB?{Environment.NewLine}");
            Console.WriteLine($"Y/N{Environment.NewLine}");
            yesOrNo = Console.ReadLine();

            if (yesOrNo.ToUpper() == "Y")
            {
                ICustomerRepository rep = new SqLiteCustomerRepository();
                var customer = new Customer
                {
                    FirstName = firstName,
                    LastName = lastName,
                    DateOfBirth = DateTime.Now
                };

                var isDeleted = rep.DeleteCustomer(customer);

                if (isDeleted == 1)
                {
                    Console.WriteLine($"Customer {firstName} {lastName} has been removed to the sqliteDB!!!");
                }
                else
                {
                    Console.WriteLine($"Customer {firstName} {lastName} was not deleted!!!");
                }
            }
        }

        public static void UpdateCustomer()
        {
            string oldFirstName;
            string oldLastName;
            string newFirstName;
            string newLastName;
            string yesOrNo;

            Console.WriteLine($"Customers first name:");
            oldFirstName = Console.ReadLine();

            Console.WriteLine($"Customers new first name:");
            newFirstName = Console.ReadLine();

            Console.WriteLine($"Customers second name:");
            oldLastName = Console.ReadLine();

            Console.WriteLine($"Customers new second name:");
            newLastName = Console.ReadLine();

            Console.WriteLine($"Do you want to update this customer to the most excellent sqlite DB?{Environment.NewLine}");
            Console.WriteLine($"Y/N{Environment.NewLine}");
            yesOrNo = Console.ReadLine();

            if (yesOrNo.ToUpper() == "Y")
            {
                ICustomerRepository rep = new SqLiteCustomerRepository();
                var oldCustomer = new Customer
                {
                    FirstName = oldFirstName,
                    LastName = oldLastName
                };

                var newCustomer = new Customer
                {
                    FirstName = newFirstName,
                    LastName = newLastName
                };

                int isUpdated = rep.UpdateCustomer(oldCustomer, newCustomer);

                if (isUpdated != 0)
                {
                    Console.WriteLine($"Customer {oldFirstName} {oldLastName} name has been changed to {newFirstName} {newLastName} has been updated to the sqliteDB!!!");
                }
                else
                {
                    Console.WriteLine($"Customer {oldFirstName} {oldLastName} was not updated!!!");
                }
            }
        }

        public static void GetCustomerList()
        {
            string yesOrNo;

            Console.WriteLine($"Please confirm you wish to see our excusive customer list?{Environment.NewLine}");
            Console.WriteLine($"Y/N{Environment.NewLine}");
            yesOrNo = Console.ReadLine();

            if (yesOrNo.ToUpper() == "Y")
            {
                ICustomerRepository rep = new SqLiteCustomerRepository();

                var customerList = rep.GetAllCustomers();

                if (customerList == null || customerList.Count() == 0)
                {
                    Console.WriteLine($"Customer List is empty");
                }
                else
                {
                    foreach(var customer in customerList)
                    {
                        Console.WriteLine($"{customer.FirstName} {customer.LastName}");
                    }
                }
            }
        }
    }
}
