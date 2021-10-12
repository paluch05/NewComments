using System;

namespace Kainos.Comments.Application.Exceptions
{
    public class UpdateCommentByIdException : Exception
    {
        public UpdateCommentByIdException(string message) : base(message)
        {
        }
    }
}
