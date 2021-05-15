namespace MRRC
{
    /// <summary>
    /// Strategy for a fully-fledged entity processor
    /// Lewis Watson 2020
    /// </summary>
    /// <typeparam name="T">Entity to convert to</typeparam>
    /// <typeparam name="P">EntityDTO to convert from</typeparam>
    public interface DTOPrcocessor<T, P> where T : Entity where P : EntityDTO
    {
        /// <summary>
        /// Converts an EntityDTO to an Entity
        /// </summary>
        /// <param name="objectDTO">A data-transfer-object with the same basic stringified properties as the Entity</param>
        /// <returns>Returns the fully-fledged entity that is not persisted</returns>
        T ConvertToEntity(P objectDTO);
    }
}
