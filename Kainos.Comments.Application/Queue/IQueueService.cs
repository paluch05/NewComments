using System.Threading.Tasks;
using Kainos.Comments.Application.Model.Database;

namespace Kainos.Comments.Application.Queue
{
    public interface IQueueService
    {
        Task ExecuteAsync(Comment comment);
    }
}
