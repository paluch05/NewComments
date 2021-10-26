using System;
using System.Collections.Generic;
using System.Text;

namespace Kainos.Comments.Application.Exceptions
{
    class CosmosDbException : Exception
    {
        public CosmosDbException(string message) : base(message)
        {

        }
    }
}
