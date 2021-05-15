using MRRC;
using System;
using System.Collections.Generic;

namespace MRRCManagement
{
    /// <summary>
    /// High-level handler for generic typing
    /// Lewis Watson 2020
    /// </summary>
    /// <typeparam name="T">Repository generic parameter</typeparam>
    /// <typeparam name="U">Repository generic parameter</typeparam>
    abstract public class Handler<T, U>
    {
        protected Repository<T, U> repository { get; }

        public Handler(Repository<T, U> repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Simple instructions to execute in the domain for the specified arguments
        /// </summary>
        /// <param name="args">A list of validated arguments ready for action in the domain</param>
        abstract protected void Execute(List<string> args);

        /// <summary>
        /// Wrapper to handle validation and exceptions thrown within Execute().
        /// Count of both parameters should match.
        /// </summary>
        /// <param name="inputs">List of child inputs from the selected menu</param>
        /// <param name="args">List of user-provided answers to inputs</param>
        public void Handle(List<Input> inputs, List<string> args)
        {
            try
            {
                ValidateAnswers(inputs, args);
                Execute(args);
            }
            catch (Exception e)
            {
                PrintErrorMessage(e.Message);
            }
        }

        /// <summary>
        /// Validate given answers against the corresponding validator passed to the input at runtime
        /// </summary>
        /// <param name="inputs">List of child inputs from the selected menu</param>
        /// <param name="args">List of user-provided answers to inputs</param>
        private void ValidateAnswers(List<Input> inputs, List<string> answers)
        {
            for (int index = 0; index < inputs.Count; index++)
            {
                Input input = inputs[index];
                var answer = answers[index];

                // Skip null answers
                if (answer != null)
                {
                    input.ValidateInput(answer);
                }
            }
        }

        /// <summary>
        /// Print an error message with wrapping for legibility
        /// </summary>
        /// <param name="message">Message extracted from the thrown exception</param>
        private void PrintErrorMessage(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine();
        }
    }
}
