using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using CrashCourse2021ExercisesDayThree.DB.Impl;
using CrashCourse2021ExercisesDayThree.Models;

namespace CrashCourse2021ExercisesDayThree.Services
{
    public class CustomerService
    {
        CustomerTable db; 
        public CustomerService()
        {
            this.db = new CustomerTable();
        }

        //Create and return a Customer Object with all incoming properties (no ID)
        internal Customer Create(string firstName, string lastName, DateTime birthDate)
        {
            if (firstName.Length < 2)
            {
                throw new ArgumentException(Constants.FirstNameException);
            }
            Customer customer = new Customer
            {
                FirstName = firstName,
                LastName = lastName,
                BirthDate = birthDate
            };
            return customer;
        }

        //db has an Add function to add a new customer!! :D
        //We can reuse the Create function above..
        internal Customer CreateAndAdd(string firstName, string lastName, DateTime birthDate)
        {
            Customer customer = new Customer();
            customer.FirstName = firstName;
            customer.LastName = lastName;
            customer.BirthDate = birthDate;
            db.AddCustomer(customer);
            return customer;
        }

        //Simple enough, Get the customers from the "Database" (db)
        internal List<Customer> GetCustomers()
        {
            List<Customer> allCustomer = db.GetCustomers();
            return allCustomer;
        }

        //Maybe Check out how to find in a LIST in c# Maybe there is a Find function?
        public Customer FindCustomer(int customerId)
        {
            if (customerId < 0)
            {
                throw new InvalidDataException(Constants.CustomerIdMustBeAboveZero);
            }
            var findCustomer = db.GetCustomers().Find(c => c.Id == customerId);
            return findCustomer;
        }
        
        /*So many things can go wrong here...
          You need lots of exceptions handling in case of failure and
          a switch statement that decides what property of customer to use
          depending on the searchField. (ex. case searchfield = "id" we should look in customer.Id 
          Maybe add searchField.ToLower() to avoid upper/lowercase letters)
          Another thing is you should use FindAll here to get all that matches searchfield/searchvalue
          You could also make another search Method that only return One Customer
           and uses Find to get that customer and maybe even test it.
        */
        public List<Customer> SearchCustomer(string searchField, string searchValue)
        {
            int searchedValueAsInt;

            if (searchField == null)
            {
                throw new InvalidDataException(Constants.CustomerSearchFieldCannotBeNull);
            }
            else if (searchValue == null)
            {
                throw new InvalidDataException(Constants.CustomerSearchValueCannotBeNull);
            }
            
            var foundCustomer = new List<Customer>();
            switch (searchField)
            {
                case "id":
                {
                    if (!int.TryParse(searchValue, out searchedValueAsInt))
                    {
                        throw new InvalidDataException(Constants.CustomerSearchValueWithFieldTypeIdMustBeANumber);
                    }
                    else if (int.Parse(searchValue) < 1)
                    {
                        throw new InvalidDataException(Constants.CustomerIdMustBeAboveZero);
                    }

                    foreach (var customer in GetCustomers())
                    {
                        if (customer.Id.ToString().Equals(searchValue))
                        {
                            foundCustomer.Add(customer);
                        }
                        else throw new InvalidDataException(Constants.CustomerSearchValueWithFieldTypeIdMustBeANumber);

                    }
                    break;
                }
                case "firstnames":
                    foreach (var customer in GetCustomers())
                    {
                        if (customer.FirstName.Equals(searchValue))
                        {
                            foundCustomer.Add(customer);
                        }
                    }

                    if (foundCustomer.Count == 0)
                    {
                        throw new InvalidDataException(Constants.CustomerSearchFieldNotFound);
                    }

                    break;
            }
            {
                return foundCustomer;
            }
        }
    }
}
