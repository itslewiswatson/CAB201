using System;
using System.Collections.Generic;
using MRRC;

namespace MRRCManagement
{
    /// <summary>
    /// Handler for adding a customer from user input
    /// Lewis Watson 2020
    /// </summary>
    public class AddCustomerHandler : CustomerHandler
    {
        public AddCustomerHandler(CRMRepository repository) : base(repository) { }

        protected override void Execute(List<string> args)
        {
            AddCustomer(args);
        }

        private void AddCustomer(List<string> args)
        {
            CRMRepository crmRepository = (CRMRepository)repository;
            args.Insert(0, crmRepository.GetNextID().ToString()); // Add new ID to arguments

            Customer newCustomer = (Customer)CreateEntity(args);
            CRM crm = repository.Get();

            crm.AddCustomer(newCustomer);

            Console.WriteLine("Added new customer {0} {1} (ID = {2}) to the MRRC CRM.", newCustomer.firstName, newCustomer.lastName, newCustomer.ID);
            Console.WriteLine();
        }

        private Entity CreateEntity(List<string> args)
        {
            return repository.entityParser.CreateEntity(args.ToArray());
        }
    }
}
