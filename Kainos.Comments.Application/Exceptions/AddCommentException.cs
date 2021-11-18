using System;

namespace Kainos.Comments.Application.Exceptions
{
    class AddCommentException : Exception
    {
        public AddCommentException(string message) : base(message)
        {
        }
    }
}
