using System;

namespace Kainos.Comments.Application.Exceptions
{
    class CensorCommentServiceException : Exception
    {
        public CensorCommentServiceException(string message) : base(message)
        {
        }
    }
}
