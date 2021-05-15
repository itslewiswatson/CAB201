using System.Collections.Generic;
using System.Linq;

namespace MRRC
{
    /// <summary>
    /// Customer impelmented entity parser
    /// Lewis Watson 2020
    /// </summary>
    public class CustomerEntityParser : EntityParser, TypedEntityParser<Customer>
    {
        public CustomerEntityParser(string customerFile) : base(customerFile) { }

        protected override string GetEntityClassName()
        {
            return "Customer";
        }

        protected override string GetCsvFileName()
        {
            return "customers.csv";
        }

        public List<Customer> LoadAll()
        {
            return Downcast(_LoadAll());
        }

        public Customer Downcast(Entity entity)
        {
            return (Customer)entity;
        }

        public List<Customer> Downcast(List<Entity> entities)
        {
            List<Customer> customers = new List<Customer>();

            foreach (Entity entity in entities)
            {
                customers.Add(Downcast(entity));
            }

            return customers;
        }

        public List<Entity> Upcast(List<Customer> entities)
        {
            return entities.Cast<Entity>().ToList();
        }

        public void SaveAll(List<Customer> entities)
        {
            _SaveAll(Upcast(entities));
        }
    }
}
