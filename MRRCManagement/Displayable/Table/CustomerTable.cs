using MRRC;
using System;
using System.Collections.Generic;

namespace MRRCManagement
{
    /// <summary>
    /// Implement functionality required to generate a customer table
    /// Lewis Watson 2020
    /// </summary>
    public class CustomerTable : DynamicTable<CRM, CustomerEntityParser>
    {
        public CustomerTable(Menu parentMenu, CRMRepository crmRepository) : base(parentMenu, crmRepository) { }

        /// <summary>
        /// Set up columns for this specific type of table
        /// </summary>
        /// <returns>Dictionary of column name and expected type</returns>
        protected override Dictionary<string, Type> GetColumnTypes()
        {
            Dictionary<string, Type> customerTypes = new Dictionary<string, Type>();

            customerTypes.Add("ID", typeof(string));
            customerTypes.Add("Title", typeof(string));
            customerTypes.Add("First Name", typeof(string));
            customerTypes.Add("Last Name", typeof(string));
            customerTypes.Add("Gender", typeof(string));
            customerTypes.Add("DOB", typeof(string));
            
            return customerTypes;
        }

        /// <summary>
        /// Get the name of the table
        /// </summary>
        /// <returns>String to be used in parent classes</returns>
        protected override string GetTableName()
        {
            return "Customers";
        }

        /// <summary>
        /// Predefined width of table
        /// </summary>
        protected override int GetTableWidth()
        {
            return 91;
        }

        /// <summary>
        /// Implementation of updating a table with data
        /// </summary>
        protected override void Update()
        {
            CRM crm = repository.Get();
            foreach (Customer customer in crm.customers)
            {
                AddRow(customer.ToStringArray());
            }
        }
    }
}
