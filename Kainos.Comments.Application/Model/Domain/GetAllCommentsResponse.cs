using System;
using System.Collections.Generic;
using System.Text;
using Kainos.Comments.Application.Model.Database;

namespace Kainos.Comments.Application.Model.Domain
{
    public class GetAllCommentsResponse
    {
        public List<Comment> AllComments { get; set; } 
    }
}
