using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Kainos.Comments.Application.Model.Database;
using Kainos.Comments.Application.Model.Domain;

namespace Kainos.Comments.Application.Search
{
    public interface ISearchService
    {
        Task CreateIndexAsync();
        Task AddCommentsAsync();
    }
}
