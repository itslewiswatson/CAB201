namespace MRRC
{
    /// <summary>
    /// Strategy for creating generic repository factories
    /// Lewis Watson 2020
    /// </summary>
    /// <typeparam name="T">Type of repository to create</typeparam>
    /// <typeparam name="E">Entity parser to use in creation and generic parameter to pass into the RepositoryStrategy</typeparam>
    public interface RepositoryFactoryStrategy<T, E> where T : RepositoryStrategy where E : EntityParser
    {
        /// <summary>
        /// Instantiate a repo of type T
        /// </summary>
        /// <returns>Repository of type T with all passed arguments</returns>
        T GetRepo();

        /// <summary>
        /// Get entity parser created in factory to pass down into the repository
        /// </summary>
        /// <returns>Single entity parser</returns>
        E GetEntityParser();
    }
}
