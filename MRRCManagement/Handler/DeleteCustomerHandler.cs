using MRRC;
using System;
using System.Collections.Generic;

namespace MRRCManagement
{
    /// <summary>
    /// Handler for deleting a customer from user input
    /// Lewis Watson 2020
    /// </summary>
    class DeleteCustomerHandler : CustomerHandler
    {
        private const int Index_To_Use = 0;

        private FleetRepository fleetRepository { get; set; }

        public DeleteCustomerHandler(CRMRepository repository, FleetRepository fleetRepository) : base(repository)
        {
            this.fleetRepository = fleetRepository;
        }

        /// <summary>
        /// Deletes customer and outputs result to user
        /// </summary>
        /// <param name="args">List of user-input arguments</param>
        protected override void Execute(List<string> args)
        {
            CRM crm = repository.Get();

            int customerID = int.Parse(args[Index_To_Use]);
            Customer customer = ResolveCustomer(customerID);

            // Ensure customer isn't renting
            Fleet fleet = fleetRepository.Get();
            if (fleet.IsCustomerRenting(customer.ID))
            {
                throw new CustomerCurrentlyRentingException(string.Format("Customer with ID {0} is currently renting a vehicle and " +
                                                            "cannot be deleted", customerID));
            }

            crm.RemoveCustomer(customer);

            Console.WriteLine("Successfully removed customer {0} {1} (ID = {2}) from the MRRC CRM.", customer.firstName, customer.lastName, customer.ID);
            Console.WriteLine();
        }

        /// <summary>
        /// Hard resolve customer
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns>Customer or throws an exception if the customer does not exist</returns>
        private Customer ResolveCustomer(int customerID)
        {
            CRM crm = repository.Get();
            return crm.GetCustomer(customerID);
        }
    }
}
