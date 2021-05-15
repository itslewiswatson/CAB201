namespace MRRC
{
    /// <summary>
    /// Strategy for instantiating repositories based on an aggregate and its associated parser
    /// Lewis Watson 2020
    /// </summary>
    /// <typeparam name="T">Aggregate to use</typeparam>
    /// <typeparam name="U">EntityParser to use</typeparam>
    public interface Repository<T, U> : RepositoryStrategy
    {
        /// <summary>
        /// Entity parser to use and tie to the repository
        /// </summary>
        U entityParser { get; }

        /// <summary>
        /// Create or return the aggregate instance to use
        /// Also loads it if null
        /// </summary>
        /// <returns>Aggregate tied to param T</returns>
        T Get();

        /// <summary>
        /// Write all aggregate data to risk through entity parsers
        /// </summary>
        void Flush();
    }
}