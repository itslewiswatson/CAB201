using MRRC;

namespace MRRCManagement
{
    /// <summary>
    /// Implement the handler for a customer for easier type-hinting
    /// Lewis Watson 2020
    /// </summary>
    abstract public class CustomerHandler : Handler<CRM, CustomerEntityParser>
    {
        /// <summary>
        /// Pass the repository to the parent with improved type-hinting
        /// </summary>
        /// <param name="repository">Repository that corresponds with the expected repository for the entity</param>
        public CustomerHandler(CRMRepository repository) : base(repository) { }
    }
}