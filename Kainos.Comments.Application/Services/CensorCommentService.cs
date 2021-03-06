using System;
using System.Threading.Tasks;
using Kainos.Comments.Application.Cosmos;
using Kainos.Comments.Application.Exceptions;
using Kainos.Comments.Application.Model.Database;
using Microsoft.Extensions.Logging;

namespace Kainos.Comments.Application.Services
{
    public class CensorCommentService : ICosmosExecutable
    {
        private readonly ICosmosDbService _cosmosDbService;
        private readonly ILogger<CensorCommentService> _log;

        public CensorCommentService(
            ICosmosDbService cosmosDbService,
            ILogger<CensorCommentService> log)
        {
            _cosmosDbService = cosmosDbService;
            _log = log;
        }
        public async Task ExecuteAsync(Comment comment)
        {
            var blackList = await _cosmosDbService.GetAllBadWordsAsync();
            try
            {
                foreach (var fWord in blackList)
                {
                    comment.Text = comment.Text.Replace(fWord.StopWord, CensorWord(fWord.StopWord.Length),
                        StringComparison.CurrentCultureIgnoreCase);
                }

                comment.IsCensored = true;
            }
            catch (Exception e)
            {
                _log.LogError(e.Message);
               throw new CensorCommentServiceException("Unable to censor comment");
            }

            await _cosmosDbService.UpdateCommentByIdAsync(comment.Id, comment);
        }

        public static string CensorWord(int len)
        {
            return new string('*', len);
        }
    }
}
