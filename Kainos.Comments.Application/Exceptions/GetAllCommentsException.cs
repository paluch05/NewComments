using System;

namespace Kainos.Comments.Application.Exceptions
{
    public class GetAllCommentsException : Exception
    {
        public GetAllCommentsException(string message) : base(message)
        {
        }
    }
}
