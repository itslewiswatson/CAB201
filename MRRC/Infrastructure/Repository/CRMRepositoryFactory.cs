namespace MRRC
{
    /// <summary>
    /// Concrete implementation of a CRM repository factory
    /// Lewis Watson 2020
    /// </summary>
    public class CRMRepositoryFactory : RepositoryFactoryStrategy<CRMRepository, CustomerEntityParser>
    {
        private string customerDirectory { get; }

        public CRMRepositoryFactory(string customerDirectory)
        {
            this.customerDirectory = customerDirectory;
        }

        /// <summary>
        /// Parser appropriate for creation of the repo as defined in the generic parameters
        /// </summary>
        /// <returns>Entity parser for use in repository creation</returns>
        public CustomerEntityParser GetEntityParser()
        {
            return new CustomerEntityParser(customerDirectory);
        }

        /// <summary>
        /// Spins up a new instance of the repository
        /// </summary>
        /// <returns>An instance of the defined repository according to the strategy generic parameters</returns>
        public CRMRepository GetRepo()
        {
            return new CRMRepository(GetEntityParser());
        }
    }
}
