using Newtonsoft.Json;

namespace Kainos.Comments.Application.Model.Domain
{
    public class SearchComment
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "author")]
        public string Author { get; set; }
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }
    }
}
