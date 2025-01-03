using System.Text.Json.Serialization;

namespace Anonymizer.Classes
{
    public class WordClass
    {
        [JsonPropertyName("conf")]
        public double Confidence { get; set; }

        [JsonPropertyName("end")]
        public double End { get; set; }

        [JsonPropertyName("start")]
        public double Start { get; set; }

        [JsonPropertyName("word")]
        public string Word { get; set; }
    }
}