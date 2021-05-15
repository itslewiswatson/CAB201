using System.Collections.Generic;

namespace MRRCManagement
{
    /// <summary>
    /// Interface for all displayable types of menus with abstract child items
    /// Lewis Watson 2020
    /// </summary>
    /// <typeparam name="T">Type of abstract child options for this implementation</typeparam>
    public interface Displayable<T> : BasicDisplayable
    {
        List<T> items { get; }
    }
}
