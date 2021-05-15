using System.Collections.Generic;

namespace MRRC
{
    /// <summary>
    /// Common interface for handling entity to type casts between the domain and the entity parsers
    /// Lewis Watson 2020
    /// </summary>
    /// <typeparam name="T">Entity to implement the parser for</typeparam>
    public interface TypedEntityParser<T> where T : Entity
    {
        /// <summary>
        /// Load all entities of type T
        /// </summary>
        /// <returns>List of casted entities to type T</returns>
        List<T> LoadAll();

        /// <summary>
        /// Saves a list of entities of type T
        /// </summary>
        /// <param name="entities">List of casted entities to type T</param>
        void SaveAll(List<T> entities);

        /// <summary>
        /// Wrapper for casting generic entity to type T
        /// </summary>
        /// <param name="entity">Entity to clone and cast</param>
        /// <returns>Casted entity of type T</returns>
        T Downcast(Entity entity);

        /// <summary>
        /// Cast a list of entities to a list of type T
        /// </summary>
        /// <param name="entities">List of generic entities</param>
        /// <returns>List of casted entities</returns>
        List<T> Downcast(List<Entity> entities);

        /// <summary>
        /// Cast a list of concrete entities to their generic counterpart
        /// </summary>
        /// <param name="entities">List of concrete entities</param>
        /// <returns>List of generic entities</returns>
        List<Entity> Upcast(List<T> entities);
    }
}
