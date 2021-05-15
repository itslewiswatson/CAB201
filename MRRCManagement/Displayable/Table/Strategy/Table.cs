using System;
using System.Collections.Generic;
using System.Data;

namespace MRRCManagement
{
    /// <summary>
    /// High level table with common data and display functionality
    /// Lewis Watson 2020 with assistance from SO 
    /// (https://stackoverflow.com/questions/856845/how-to-best-way-to-draw-table-in-console-app-c)
    /// </summary>
    abstract public class Table : Menu, BasicDisplayable
    {
        protected DataTable table { get; }

        public Table(Menu parentMenu)
        {
            // Intialise DataTable with the name prop
            DataTable table = new DataTable
            {
                TableName = GetTableName()
            };

            // Add columns to table on instiatiation
            foreach (KeyValuePair<string, Type> entry in GetColumnTypes())
            {
                table.Columns.Add(entry.Key, entry.Value);
            }

            this.parentMenu = parentMenu;
            this.table = table;
        }

        abstract protected Dictionary<string, Type> GetColumnTypes();
        abstract protected string GetTableName();
        abstract protected int GetTableWidth();
        abstract public void Refresh();
        
        /// <summary>
        /// Clear the current rows in the table
        /// </summary>
        protected void Clear()
        {
            table.Clear();
        }

        /// <summary>
        /// Add row to the table
        /// </summary>
        /// <param name="values">Parameterised array of object values to add as a row</param>
        protected void AddRow(params object[] values)
        {
            table.Rows.Add(values);
        }

        /// <summary>
        /// Print a singular line of the table
        /// </summary>
        private void PrintLine()
        {
            Console.WriteLine(new string('-', GetTableWidth()));
        }

        /// <summary>
        /// Print a table's data row
        /// </summary>
        /// <param name="columns"></param>
        private void PrintRow(params string[] columns)
        {
            int width = (GetTableWidth() - columns.Length) / columns.Length;
            string row = "|";

            foreach (string column in columns)
            {
                row += AlignCentre(column, width) + "|";
            }

            Console.WriteLine(row);
        }

        /// <summary>
        /// Centre-align text within a table cell
        /// </summary>
        /// <param name="text">Text to center</param>
        /// <param name="width">Width parameter to work with</param>
        /// <returns>Stirng with necessary padding</returns>
        private string AlignCentre(string text, int width)
        {
            text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', width);
            }
            else
            {
                return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
            }
        }

        /// <summary>
        /// Master function for displaying a table
        /// </summary>
        public override void Print()
        {
            // Display a tailored message instead of a blank table
            if (table.Rows.Count == 0)
            {
                Console.WriteLine("There are no items to display. Perhaps you could make something to go here!?");
                Console.WriteLine();
                return;
            }

            // Print table header including column names
            PrintLine();
            List<string> columns = new List<string>();
            foreach (DataColumn column in table.Columns)
            {
                columns.Add(column.ColumnName);
            }
            PrintRow(columns.ToArray());
            PrintLine();

            // Print all data rows in the table
            foreach (DataRow row in table.Rows)
            {
                List<string> rowColumns = new List<string>();
                foreach (DataColumn column in table.Columns)
                {
                    rowColumns.Add(row[column].ToString());
                }
                PrintRow(rowColumns.ToArray());
            }
            PrintLine();
            Console.WriteLine();
        }
    }
}
