using System;
using System.Collections;

namespace MRRCManagement
{
    /// <summary>
    /// Backend search algorith using shunting yard algorithm
    /// Lewis Watson & Shlomo 2020
    /// </summary>
    public class SearchAlgorithm
    {
        ArrayList operators;
        ArrayList operands;
        public ArrayList Infix { get; } = new ArrayList();
        public ArrayList Postfix { get; } = new ArrayList();

        // Data entered as '-seater' so this is 7
        private const int Seat_Suffix_Length = 7;

        /// <summary>
        /// Translates infix expressions into postfix expressions
        /// </summary>
        /// <param name="query">Infix query that has been split into its tokens</param>
        /// <param name="attributes"></param>
        public SearchAlgorithm(ArrayList query, ArrayList attributes)
        {
            // define valid tokens
            operators = new ArrayList();
            operands = attributes;
            // here we hardcode opernads and operators; this can be done in a more elegant manner.
            string[] t1 = { "AND", "OR", "(", ")" }; // operators and parentheses
            operators.AddRange(t1);
            // Create and instantiate a new empty Stack.
            Stack rpnStack = new Stack();

            // apply dijkstra algorithm using a stack to convert infix to postfix notation (=rpn)
            Infix.AddRange(query);
            foreach (string infixToken in Infix)
            {
                // Replace underscores with space for things like "Fiero 2M4"
                string token = infixToken;
                if (token.Contains("_"))
                {
                    token = token.Replace("_", " ");
                }

                if (operands.Contains(token))
                {   // move operands across to output
                    Postfix.Add(token);
                }
                else if (token.Equals("("))
                {   // push open parenthesis onto stack
                    rpnStack.Push(token);
                }
                else if (token.Equals(")"))
                {   // pop all operators off the stack until the mathcing open parenthesis is found
                    while ((rpnStack.Count > 0) && !((string)rpnStack.Peek()).Equals("("))
                    {
                        Postfix.Add(rpnStack.Pop());  // transfer operator to output
                        if (rpnStack.Count == 0)
                        {
                            throw new Exception("Unbalanced parenthesis. Make sure you close any parenthesis and don't have any extras!");
                        }
                    }
                    if (rpnStack.Count == 0)
                    {
                        throw new Exception("Unbalanced parenthesis. Make sure you close any parenthesis and don't have any extras!");
                    }
                    rpnStack.Pop(); // discard open parenthesis
                }
                else if (operators.Contains(token))
                {   // push operand to the rpn stack after moving to output all higher or equal priority operators
                    while (rpnStack.Count > 0 && ((string)rpnStack.Peek()).Equals("AND"))
                    {
                        Postfix.Add(rpnStack.Pop());  // pop and add to output
                    }
                    rpnStack.Push(token); // now pus the operator onto the stack
                }
                else
                {
                    if (token.Length >= 8 && token.Substring(token.Length - Seat_Suffix_Length) == "-SEATER") 
                    {
                        throw new Exception(string.Format("No matches found for a {0} vehicle! Try a different number of seats.", token));
                    }
                    throw new Exception(string.Format("Unrecognised token '{0}'. Ensure you only use 'AND', 'OR', '(' or ')', in addition to " +
                        "vehicle attributes only!", token));
                }
            }
            // now copy what's left on the rpnStack
            while (rpnStack.Count > 0)
            {   // move to the output all remaining operators
                if (((string)rpnStack.Peek()).Equals("("))
                {
                    throw new Exception("Unbalanced parenthesis. Make sure you close any parenthesis and don't have any extras!");
                }
                Postfix.Add(rpnStack.Pop());
            }
        }
    }
}
