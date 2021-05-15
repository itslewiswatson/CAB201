using System.Linq;

namespace MRRC
{
    /// <summary>
    /// Provide helper methods when dealing with Enum objects in the domain or elsewhere
    /// Lewis Watson 2020
    /// </summary>
    public class EnumHelper
    {
        /// <summary>
        /// Converts a potentially unfriendly string to one that can be parsed into a domain enum
        /// </summary>
        /// <param name="input">An unfriendly string (loose capitalisation, etc)</param>
        /// <returns>A string with one uppercase letter, followed by all lowercase letters</returns>
        public static string MakeFriendlyString(string input)
        {
            return char.ToUpper(input.First()) + input.Substring(1).ToLower();
        }
    }
}
