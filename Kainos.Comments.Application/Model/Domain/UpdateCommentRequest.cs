using System;

namespace Kainos.Comments.Application.Model.Domain
{
    public class UpdateCommentRequest
    {
        public string Id { get; set; }
        public string Text { get; set; }
    }
}