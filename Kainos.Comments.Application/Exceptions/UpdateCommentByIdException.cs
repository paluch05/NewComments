using System;

namespace Kainos.Comments.Application.Exceptions
{
    class UpdateCommentByIdException : Exception
    {
        public UpdateCommentByIdException(string message) : base(message)
        {
        }
    }
}
