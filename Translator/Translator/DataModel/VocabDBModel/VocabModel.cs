using Newtonsoft.Json;

//The client profile model
//Author: Long-Sing Wong
namespace Translator
{
    //attribute that should be included in the easy table
    public class VocabModel
    {
        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }

        [JsonProperty(PropertyName = "SourceText")]
        public string SourceText { get; set; }

        [JsonProperty(PropertyName = "TranslateText")]
        public string TranslateText { get; set; }


    }
}
