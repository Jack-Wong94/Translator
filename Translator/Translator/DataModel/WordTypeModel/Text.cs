using Newtonsoft.Json;
using System.Collections.Generic;

namespace Translator
{
    public class Text
    {
        [JsonProperty(PropertyName = "text")]
        public List<List<List<WordType>>> data { get; set; }


    }
}
