using System.Collections.Generic;

namespace EnpalSharpTemplate.Model
{
    public class ErrorMessage
    {
        /// <summary>
        /// Provide a list of all errors happens
        /// </summary>
        public List<string> Messages { set; get; }

        public ErrorMessage()
        {
            Messages = new List<string>();
        }

        public ErrorMessage(string message)
        {
            Messages = new List<string>() { message };
        }
    }
}
