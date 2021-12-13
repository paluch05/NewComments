using System;
using Azure;

namespace Kainos.Comments.Application.Configuration
{
    public class Configuration
    {
    public string CosmosDbConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string CommentContainerName { get; set; }
    public string BlackListContainerName { get; set; }
    public string QueueConnectionString { get; set; }
    public string QueueName { get; set; }
    public string AdminKey { get; set; }
    public string QueryKey { get; set; }
    public string SearchEndpoint { get; set; }
    public string IndexName { get; set; }
    }
}
