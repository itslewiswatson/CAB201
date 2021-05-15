using System.Collections.Generic;

namespace MRRC
{
    /// <summary>
    /// Concrete implementation of the CRM repository
    /// Lewis Watson 2020
    /// </summary>
    public class CRMRepository : Repository<CRM, CustomerEntityParser>
    {
        public CustomerEntityParser entityParser { get; }
        private CRM crm { get; set; }

        public CRMRepository(CustomerEntityParser entityParser)
        {
            this.entityParser = entityParser;
        }

        /// <summary>
        /// Return or create an isntance of a CRM, including loading assocated entities
        /// </summary>
        public CRM Get()
        {
            if (crm == null)
            {
                crm = new CRM(SelectAll());
            }
            return crm;
        }

        /// <summary>
        /// Load all customers from disk
        /// </summary>
        /// <returns>List of customers loaded from disk</returns>
        public List<Customer> SelectAll()
        {
            return entityParser.LoadAll();
        }

        /// <summary>
        /// Read CRM for highest customer ID and return one higher for the next ID
        /// </summary>
        /// <returns>ID of the next new customer to enter the CRM</returns>
        public int GetNextID()
        {
            int highestID = 0;
            foreach (Customer customer in crm.customers)
            {
                if (customer.ID > highestID)
                {
                    highestID = customer.ID;
                }
            }
            return highestID + 1;
        }

        /// <summary>
        /// Write all CRM data to disk
        /// </summary>
        public void Flush()
        {
            entityParser.SaveAll(crm.customers);
        }
    }
}
