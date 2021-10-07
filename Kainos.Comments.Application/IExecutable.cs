using System.Collections.Generic;
using System.Threading.Tasks;
using Kainos.Comments.Application.Model.Database;

namespace Kainos.Comments.Application.Services
{
    public interface IExecutable<in TRequest, TResponse>
    {
        Task<TResponse> ExecuteAsync(TRequest request);
    }
}