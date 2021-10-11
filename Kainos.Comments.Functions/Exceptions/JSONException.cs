using System;

namespace Kainos.Comments.Functions.Exceptions
{
    class JsonException : Exception
    {
        public JsonException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
