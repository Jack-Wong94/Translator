using Newtonsoft.Json;

namespace Translator
{
    /// <summary>
    /// Json model of the word type. It includes the tag of the word (i.e. noun, verb) and the word itself
    /// </summary>
    public class WordType
    {
        [JsonProperty(PropertyName = "word")]
        public string word { get; set; }

        [JsonProperty(PropertyName = "tag")]
        public string tag { get; set; }
    }
}
