using System;

namespace Kainos.Comments.Functions.Exceptions
{
    class CosmosInsertException : Exception
    {
        public CosmosInsertException(string message) : base(message)
        {
        }
    }
}
