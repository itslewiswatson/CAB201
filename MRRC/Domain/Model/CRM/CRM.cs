using System.Collections.Generic;

namespace MRRC
{
    /// <summary>
    /// Aggregate that houses all customers, with a few helper methods to assist in the aggregation of customers
    /// Lewis Watson 2020
    /// </summary>
    public class CRM : Aggregate
    {
        public List<Customer> customers { get; }

        public CRM(List<Customer> customers)
        {
            this.customers = customers;
        }

        /// <summary>
        /// Add a customer to the CRM with logic to check if a customer with the same ID already exists within the CRM
        /// </summary>
        /// <param name="customer">Customer to add</param>
        public void AddCustomer(Customer customer)
        {
            if (IsCustomerInCRM(customer.ID))
            {
                throw new CustomerAlreadyExistsException(string.Format("Customer with ID {0} already exists. Please choose another ID.", 
                                                            customer.ID));
            }
            customers.Add(customer);
        }

        /// <summary>
        /// Hard check to resolve a customer from an ID
        /// </summary>
        /// <param name="customerID">Integer representing the customer ID to resolve</param>
        /// <returns>A customer object if successful, or throw an exception if the customer cannot be resolved</returns>
        public Customer GetCustomer(int customerID)
        {
            Customer customer = FindCustomer(customerID);
            if (customer != null)
            {
                return customer;
            }
            throw new CustomerDoesNotExistException(string.Format("Customer with ID = '{0}' does not exist.", customerID));
        }

        /// <summary>
        /// Soft check to resolve a customer from an ID
        /// </summary>
        /// <param name="customerID">Integer representing the customer ID to resolve</param>
        /// <returns>A customer object if successful, or null if the ID does not correspond to a customer</returns>
        public Customer FindCustomer(int customerID)
        {
            return customers.Find(existingCustomer => existingCustomer.ID == customerID);
        }

        /// <summary>
        /// Simple boolean gate for finding a customer
        /// </summary>
        /// <param name="customerID">Integer representing the customer ID to resolve</param>
        /// <returns></returns>
        public bool IsCustomerInCRM(int customerID)
        {
            return FindCustomer(customerID) != null;
        }

        /// <summary>
        /// Remove a fully-fledged customer from the CRM
        /// </summary>
        /// <param name="customer">Customer object to remove from the CRM</param>
        public void RemoveCustomer(Customer customer)
        {
            customers.Remove(customer);
        }
    }
}
