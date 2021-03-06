using System.Threading.Tasks;
using Kainos.Comments.Application.Model.Database;

namespace Kainos.Comments.Application
{
    public interface ICosmosExecutable
    {
        Task ExecuteAsync(Comment comment);
    }
}
