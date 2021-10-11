using System;

namespace Kainos.Comments.Application.Exceptions
{
    class GetAllCommentsException : Exception
    {
        public GetAllCommentsException(string message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
