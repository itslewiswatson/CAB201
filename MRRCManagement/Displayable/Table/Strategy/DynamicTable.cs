using MRRC;

namespace MRRCManagement
{
    /// <summary>
    /// Untyped table with common functionality that allows it to utilise the parent repository
    /// Lewis Watson 2020
    /// </summary>
    /// <typeparam name="T">Repository generic type parameter</typeparam>
    /// <typeparam name="U">Repository generic type parameter</typeparam>
    abstract public class DynamicTable<T, U> : Table
    {
        protected Repository<T, U> repository { get; }

        /// <summary>
        /// Set up table and immediately fill it with data
        /// </summary>
        /// <param name="parentMenu">Menu that contains table</param>
        /// <param name="repository">Repository to draw table data from</param>
        public DynamicTable(Menu parentMenu, Repository<T, U> repository) : base(parentMenu)
        {
            this.repository = repository;
            Refresh();
        }

        /// <summary>
        /// Specific strategy for updating the data within a table
        /// </summary>
        abstract protected void Update();

        /// <summary>
        /// Helper method to be called anywhere that can refresh the contents of the table
        /// </summary>
        public override void Refresh()
        {
            Clear();
            Update();
        }
    }
}
