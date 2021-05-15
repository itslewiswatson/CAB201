using System;

namespace MRRCManagement
{
    /// <summary>
    /// Custom exception catering to specifically invalid inputs parsed
    /// Lewis Watson 2020
    /// </summary>
    class InputInvalidException : Exception
    {
        public InputInvalidException(string message) : base(message) { }
    }
}
