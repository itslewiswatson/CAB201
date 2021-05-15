using System;
using System.Reflection;
using System.Collections.Generic;
using MRRC;

namespace MRRCManagement
{
    /// <summary>
    /// Handles the editing of customers based on user input fields
    /// Lewis Watson 2020
    /// </summary>
    public class EditCustomerHandler : CustomerHandler
    {
        private const int Index_To_Begin = 1;
        private const int Index_To_Remove = Index_To_Begin - 1;

        private FleetRepository fleetRepository { get; }

        public EditCustomerHandler(CRMRepository repository, FleetRepository fleetRepository) : base(repository)
        {
            this.fleetRepository = fleetRepository;
        }

        /// <summary>
        /// Edits a customer based on user input
        /// </summary>
        /// <param name="args">List of user-input arguments</param>
        protected override void Execute(List<string> args)
        {
            PropertyInfo[] customerProperties = typeof(Customer).GetProperties();

            Customer existingCustomer = ResolveCustomer(args);
            List<string> trimmedArgs = TrimArguments(args);
            DefaultArguments(ref trimmedArgs, customerProperties, existingCustomer);

            // Ensure customer isn't renting
            Fleet fleet = fleetRepository.Get();
            if (fleet.IsCustomerRenting(existingCustomer.ID))
            {
                throw new CustomerCurrentlyRentingException(string.Format("This customer is currently renting a vehicle and " +
                                                            "cannot be edited", existingCustomer));
            }

            Customer validatedCustomer = CreateNewCustomer(trimmedArgs);

            ValidateConstraints(existingCustomer, validatedCustomer);
            EqualiseCustomers(existingCustomer, validatedCustomer);

            Console.WriteLine("Successfully edited customer {0} {1} (ID = {2}).", validatedCustomer.firstName, validatedCustomer.lastName, 
                                validatedCustomer.ID);
            Console.WriteLine();
        }

        /// <summary>
        /// Creates a new unpersisted customer from user input
        /// </summary>
        /// <param name="args">List of user-input arguments</param>
        /// <returns>New customer entity</returns>
        private Customer CreateNewCustomer(List<string> args)
        {
            return (Customer)repository.entityParser.CreateEntity(args.ToArray());
        }

        /// <summary>
        /// Validate business logic surrounding customers
        /// </summary>
        /// <param name="existingCustomer">Customer to update parameters on</param>
        /// <param name="validatedCustomer">Customer to update parameters from</param>
        private void ValidateConstraints(Customer existingCustomer, Customer validatedCustomer)
        {
            CRM crm = repository.Get();

            if (validatedCustomer.ID != existingCustomer.ID)
            {
                Customer newIdCustomer = crm.FindCustomer(validatedCustomer.ID);
                if (newIdCustomer != null)
                {
                    throw new CustomerAlreadyExistsException(string.Format("Customer with ID {0} already exists. Please choose a different ID.", 
                                                                existingCustomer.ID));
                }
            }
        }

        /// <summary>
        /// Remove arguments from user-input that are not relevant to the current entity
        /// </summary>
        /// <param name="args">Parsed user-input arguments</param>
        /// <returns>List of relevant arguments</returns>
        private List<string> TrimArguments(List<string> args)
        {
            return args.GetRange(Index_To_Begin, args.Count - Index_To_Begin); // Remove first element (current ID)
        }

        /// <summary>
        /// Default any blank arguments specified by user to the current customer's value
        /// </summary>
        /// <param name="args">List of user-inputted arguments</param>
        /// <param name="customerProperties">List of reflection-based customer properties</param>
        /// <param name="existingCustomer">Customer to pull values from</param>
        private void DefaultArguments(ref List<string> args, PropertyInfo[] customerProperties, Customer existingCustomer)
        {
            for (int index = 0; index < args.Count; index++)
            {
                string value = args[index].ToString().Trim();
                if (value == "")
                {
                    var currentValue = customerProperties[index].GetValue(existingCustomer).ToString();
                    args[index] = currentValue;
                }
            }
        }

        /// <summary>
        /// Equalise the properties of two customer entities
        /// </summary>
        /// <param name="existingCustomer">Customer to update parameters on</param>
        /// <param name="validatedCustomer">Customer to update parameters from</param>
        private void EqualiseCustomers(Customer existingCustomer, Customer validatedCustomer)
        {
            existingCustomer.ID = validatedCustomer.ID;
            existingCustomer.title = validatedCustomer.title;
            existingCustomer.firstName = validatedCustomer.firstName;
            existingCustomer.lastName = validatedCustomer.lastName;
            existingCustomer.gender = validatedCustomer.gender;
            existingCustomer.dateTime = validatedCustomer.dateTime;
        }

        /// <summary>
        /// Resolve customer given arguments
        /// </summary>
        /// <param name="args">Parsed user-input arguments</param>
        /// <returns>Resolved customer or throws an exception</returns>
        private Customer ResolveCustomer(List<string> args)
        {
            CRM crm = repository.Get();

            int customerID = int.Parse(args[Index_To_Remove]);
            return crm.GetCustomer(customerID);
        }
    }
}
