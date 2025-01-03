using System.Reflection.Metadata;
using System.Text.Json;

namespace Anonymizer.Classes
{
    public class Record
    {
        public string operatorName { get; set; }
        public string Mp3FilePath { get; set; }
        public string WavFilePath { get; set; }
        public string WavMonoFilePath { get; set; }
        public string FinalFilePath { get; set; }
        public bool isAlreadyWav { get; set; }
        public bool isAlreadyWavMono { get; set; }

        public static List<Record> GetRecordListToAnalyze()
        {
            string jsonFile = File.ReadAllText(Constants.path_ToAnalyze);
            return Sanitize(JsonSerializer.Deserialize<List<Record>>(jsonFile));
        }

        public static List<Record> Sanitize(List<Record> records)
        {
            records.ForEach(rec =>
            {
                string nomeFile =  rec.Mp3FilePath;
                rec.Mp3FilePath = Path.Combine(Constants.path_OriginalFiles, nomeFile);
                rec.WavFilePath = rec.isAlreadyWav ? rec.Mp3FilePath : Path.Combine(Constants.path_TempFiles, nomeFile.Replace(".mp3",".wav"));
                rec.WavMonoFilePath = rec.isAlreadyWavMono ? rec.WavFilePath : Path.Combine(Constants.path_TempFiles, nomeFile.Replace(".mp3","_mono.wav"));
                rec.FinalFilePath= Path.Combine(Constants.path_ModifiedFiles, nomeFile.Replace(".mp3",".wav"));
            });
            return records;
        }

    }

}
