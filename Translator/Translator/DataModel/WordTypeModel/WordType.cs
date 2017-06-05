using Newtonsoft.Json;

namespace Translator
{
    public class WordType
    {
        [JsonProperty(PropertyName = "word")]
        public string word { get; set; }

        [JsonProperty(PropertyName = "tag")]
        public string tag { get; set; }
    }
}
