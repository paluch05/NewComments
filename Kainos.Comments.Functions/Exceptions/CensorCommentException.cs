using System;

namespace Kainos.Comments.Functions.Exceptions
{
    public class CensorCommentException : Exception
    {
        public CensorCommentException(string message) : base(message)
        {
        }
    }
}
