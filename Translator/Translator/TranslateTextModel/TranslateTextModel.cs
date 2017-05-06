using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace Translator
{
    /// <summary>
    /// This model deserialize the json object into translated text and show the language code
    /// It could be used to serialize the object into json and send it to a database.
    /// </summary>
    public class TranslateTextModel
    {
        //Primary use it to store the translated text from the json string.
        [JsonProperty(PropertyName = "text")]
        public string[] TranslatedText { get; set; }

        [JsonProperty(PropertyName = "lang")]
        public string Lang { get; set; }
    }
}
