using System;
using Newtonsoft.Json;

namespace Kainos.Comments.Application.Model.Database
{
    public class Comment
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "author")]
        public string Author { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "creationDate")]
        public DateTime CreationDate { get; set; }

        [JsonProperty(PropertyName = "isCensored")]
        public bool IsCensored { get; set; }
    }
}