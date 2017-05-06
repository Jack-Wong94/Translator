using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace Translator
{
    public class TranslateTextModel
    {
        [JsonProperty(PropertyName = "text")]
        public string[] TranslatedText { get; set; }

        [JsonProperty(PropertyName = "lang")]
        public string Lang { get; set; }
    }
}
