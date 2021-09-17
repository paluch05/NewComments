using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
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
            var blackList = _cosmosDbService.GetAllBadWords().Result.Select(r => r.StopWord).ToList();
            
            foreach (var fWord in blackList)
            {
                comment.Text = comment.Text.Replace(fWord, CensorWord(fWord.Length));
            }

            comment.IsCensored = true;

           await _cosmosDbService.UpdateCommentByIdAsync(comment.Id, comment);
        }

        public static string CensorWord(int len)
        {
            return new string('*', len);
        }
    }
}
