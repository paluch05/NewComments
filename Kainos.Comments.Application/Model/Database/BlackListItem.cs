using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Newtonsoft.Json;

namespace Kainos.Comments.Application.Model.Database
{
    public class BlackListItem
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "stopWord")]
        public string StopWord { get; set; }
    }
}
