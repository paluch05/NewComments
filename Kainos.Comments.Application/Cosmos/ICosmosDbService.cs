using System.Collections.Generic;
using System.Threading.Tasks;
using Kainos.Comments.Application.Model.Database;
using Kainos.Comments.Application.Model.Domain;

namespace Kainos.Comments.Application.Cosmos
{
    public interface ICosmosDbService
    {
        Task<IEnumerable<Comment>> GetAllCommentsAsync();

        Task<Comment> AddCommentAsync(Comment comment);

        Task UpdateCommentByIdAsync(string id, Comment comment);

        Task DeleteCommentByIdAsync(string id);

        public Task<IEnumerable<BlackListItem>> GetAllBadWordsAsync();

        Task DeleteAllComments();

        Task<IEnumerable<SearchComment>> GetAllSearchCommentAsync();
    }
}