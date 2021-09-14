using System.Threading.Tasks;

namespace Kainos.Comments.Application.Services
{
    public interface IExecutable<in TRequest, TResponse>
    {
        Task<TResponse> ExecuteAsync(TRequest request);
    }
}