using System;

namespace Kainos.Comments.Functions.Exceptions
{
    class GetAllCommentsFunctionException : Exception
    {
        public GetAllCommentsFunctionException(string message) : base(message)
        {
        }
    }
}
