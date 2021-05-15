using System;
using System.Collections.Generic;

namespace MRRC
{
    /// <summary>
    ///  Container class for all domain entities.
    /// Used to clearly identifying and type-hinting the use of entities in the domain.
    /// Lewis Watson 2020
    /// </summary>
    abstract public class Entity
    {
        /// <summary>
        /// Format any entity into an array for use elsewhere
        /// </summary>
        public string[] ToStringArray()
        {
            List<string> properties = GetStringFormattedProperities();
            return properties.ToArray();
        }

        /// <summary>
        /// Format any entity into a comma-delimited CSV-friendly string
        /// </summary>
        public override string ToString()
        {
            List<string> properties = GetStringFormattedProperities();

            string value = string.Join(",", properties.ToArray());
            return value;
        }

        /// <summary>
        /// Helper function for implementing a ToString method on an entity
        /// </summary>
        /// <param name="dateTime">The specified date time property to stringify</param>
        /// <returns>Returns a string of the date in a dd/mm/yyyy format</returns>
        private string FormatDateOfBirth(DateTime dateTime)
        {
            return dateTime.ToString("dd/MM/yyyy");
        }

        /// <summary>
        /// Returns the value of all properties in an entity using reflection
        /// </summary>
        private List<string> GetStringFormattedProperities()
        {
            List<string> properties = new List<string>();

            foreach (var property in GetType().GetProperties())
            {
                object propertyValue = property.GetValue(this, null);
                string propertyString = propertyValue.ToString();

                // Custom parsing for DateTime objects
                if (propertyValue is DateTime)
                {
                    DateTime dateTime = (DateTime)propertyValue; // Cast to DateTime
                    propertyString = FormatDateOfBirth(dateTime);
                }

                properties.Add(propertyString);
            }

            return properties;
        }
    }
}
