using System;
using System.Collections.Generic;

namespace MRRCManagement
{
    /// <summary>
    /// Menu of input options that are used to collect user input for use elsewhere
    /// Lewis Watson 2020
    /// </summary>
    public class InputMenu : Menu, Displayable<Input>
    {
        public string id { get; }
        public string header { get; }
        public List<Input> items { get; } = new List<Input>();
        public List<string> answers { get; } = new List<string>();

        public InputMenu(string id, string header, Menu parentMenu)
        {
            this.id = id;
            this.header = header;
            this.parentMenu = parentMenu;
        }

        /// <summary>
        /// Add an input to the menu
        /// </summary>
        /// <param name="displayText">Text to display before user input</param>
        /// <param name="inputValidator">Optional validator for what the user enters</param>
        /// <param name="allowBlanks">Boolean to allow blank inputs from the user (default: false)</param>
        /// <returns></returns>
        public Input AddInput(string displayText, InputValidator inputValidator = null, bool allowBlanks = false)
        {
            Input input = new Input(displayText, this, allowBlanks, null, inputValidator);
            items.Add(input);
            return input;
        }

        /// <summary>
        /// Clear all previous answers and display menu
        /// </summary>
        public override void Print()
        {
            answers.Clear();
            PrintHeader();
            PrintInputs();
            Console.WriteLine();
        }

        /// <summary>
        /// Print menu header
        /// </summary>
        private void PrintHeader()
        {
            Console.WriteLine("{0}:", header);
            Console.WriteLine();
        }

        /// <summary>
        /// Print desired inputs, wait for user input then add to answers for later parsing
        /// </summary>
        private void PrintInputs()
        {
            foreach (Input input in items)
            {
                Console.Write("{0}: ", input.displayText);
                string answer = Console.ReadLine().Trim();

                // Validate as inputted by user if field has an attached validator
                if (input.inputValidator != null)
                {
                    input.ValidateInput(answer);
                }

                answers.Add(answer);
            }
        }
    }
}
