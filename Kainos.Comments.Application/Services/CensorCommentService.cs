using System;
using System.Threading.Tasks;
using Kainos.Comments.Application.Cosmos;
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
            
            foreach (var fWord in blackList)
            {
                comment.Text = comment.Text.Replace(fWord.StopWord, CensorWord(fWord.StopWord.Length), StringComparison.CurrentCultureIgnoreCase);
            }
            comment.IsCensored = true;

           await _cosmosDbService.UpdateCommentByIdAsync(comment.Id, comment);
        }

        public static string CensorWord(int len)
        {
            return new string('*', len);

            // TDD, najpierw napisac test jakbym chciala zeby metoda dzialala,
            // caps, sprowadzic do jednej wielkosci poprobowac TDD i testami dojsc jak uzyskac efekt not caseSensitive
            // uchronic przed wstrzykiwaniem skryptow, cross site scripting (?), ochrona funkcji przed atakami np <> i ustalic regexem
        }
    }
}
