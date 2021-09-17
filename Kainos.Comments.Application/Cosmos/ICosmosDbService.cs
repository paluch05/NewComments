using System.Collections.Generic;
using System.Threading.Tasks;
using Kainos.Comments.Application.Model;
using Kainos.Comments.Application.Model.Database;

namespace Kainos.Comments.Application.Cosmos
{
    public interface ICosmosDbService
    {
        Task<IEnumerable<Comment>> GetAllComments();

        Task<Comment> GetCommentByIdAsync(string id);

        Task<Comment> AddCommentAsync(Comment comment);

        Task UpdateCommentByIdAsync(string id, Comment comment);

        Task DeleteCommentByIdAsync(string id);

        public Task<IEnumerable<BlackListItem>> GetAllBadWords();
    }
}