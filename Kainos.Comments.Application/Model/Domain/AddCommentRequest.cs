using System;
using System.Collections.Generic;
using System.Text;

namespace Kainos.Comments.Application.Model
{
    public class AddCommentRequest
    {
        public string Author { get; set; }
        public string Text { get; set; }
    }
}