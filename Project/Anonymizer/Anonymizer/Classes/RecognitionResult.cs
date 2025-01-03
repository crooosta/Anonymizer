using System.Text.Json.Serialization;

namespace Anonymizer.Classes
{
    public class RecognitionResult
    {
        [JsonPropertyName("result")]
        public List<WordClass> words { get; set; }
        
        [JsonPropertyName("text")]
        public string transcription { get; set; }

        public RecognitionResult()
        {
            words = new List<WordClass>();
            transcription = "";
        }

        /// <summary>
        /// Method used to fill the word list and to create the transcription
        /// </summary>
        /// <param name="partialResult"></param>
        public void Append(RecognitionResult partialResult)
        {
            if (partialResult.words != null)
            {
                this.words.AddRange(partialResult.words);
                this.transcription += " " + partialResult.transcription;
            }
        }
    }
}
