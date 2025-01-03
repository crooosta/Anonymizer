using System.Diagnostics;
using System.Text.Json;
using Anonymizer.Classes;

namespace Anonymizer
{
    public partial class Main
    {
        public Main()
        {

            List<Record> records = Record.GetRecordListToAnalyze();

            Console.WriteLine($"Found {records.Count} records to analyze");

            foreach (Record record in records)
            {
                Console.WriteLine("Start to analyze record: " + record.Mp3FilePath);

                // Convert to WAV
                if (!record.isAlreadyWav)
                    Mp3ToWav.ConvertUNIX(record.Mp3FilePath, record.WavFilePath);

                // Unify the channels
                if (!record.isAlreadyWavMono)
                    UnifyChannels(record);

                // Get the recognition
                RecognitionResult recognitionResult = VoskAPI.Recognize(record.WavMonoFilePath, false);

                // Find the occurrences of the target phrase
                List<Occurence> occurences = FindOccurrences(recognitionResult, record.operatorName);

                if (occurences.Count > 0)
                    GenerateFinalAudio(record, occurences);

                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                Console.Clear();

            }

        }

        public static List<Occurence> FindOccurrences(RecognitionResult result, string targetPhrase)
        {
            Console.WriteLine($"Finding occurrences of '{targetPhrase}'...");

            // Initializing a list to store all the occurences
            List<Occurence> foundOccurences = new List<Occurence>();

            string[] targetWords = targetPhrase.ToLower().Split(' ');

            // Search for all the occurences of the first target word
            var startingPoints = result.words.Where(r => r.Word == targetWords[0]).ToList();

            // For each occurences of the first target word, search the words sequence
            foreach (var start in startingPoints)
            {
                // Find the starting index of this occurrence
                int startIndex = result.words.IndexOf(start);

                // Find Result objects matching consecutive target words
                List<WordClass> matchingResults = result.words.Skip(startIndex)
                                             .Take(targetWords.Length)
                                             .Where((r, index) => r.Word == targetWords[index])
                                             .ToList();

                // If we found all consecutive target words
                if (matchingResults.Count == targetWords.Length)
                {
                    // Chain the words into a sentence
                    foundOccurences.Add(new Occurence()
                    {
                        start = matchingResults.First().Start,
                        end = matchingResults.Last().End,
                        words = matchingResults
                    });
                }
            }

            Console.WriteLine($"Found {foundOccurences.Count} occurrences of '{targetPhrase}'");

            return foundOccurences;
        }

        /// <summary>
        /// Generate the Wav file with one Mono channel
        /// </summary>
        /// <param name="record"></param>
        private static void UnifyChannels(Record record)
        {
            Console.WriteLine($"Unifying Channels...");
            try
            {
                string source = record.WavFilePath;
                string dest = record.WavMonoFilePath;

                var processInfo = new ProcessStartInfo
                {
                    FileName = Constants.pathUnifyChannels,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    ArgumentList = {
                    "-s", source,
                    "-d", dest
                }
                };


                using var process = new Process { StartInfo = processInfo };
                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    throw new Exception($"Channels unification failed: {error}");
                }

                Console.WriteLine($"Channels successful unified.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error unifying channels: {ex.Message}");
            }
        }

        /// <summary>
        /// Generates the final audio file after replacing the occurrences of the target phrase with silence or beeps
        /// </summary>
        /// <param name="record"></param>
        /// <param name="occurences"></param>
        private static void GenerateFinalAudio(Record record, List<Occurence> occurences)
        {
            Console.WriteLine($"Generating Final audio with replacements...");
            try
            {
                string source = record.WavFilePath;
                string dest = record.FinalFilePath;

                List<OccurenceNoWordList> occurencesClean = OccurenceNoWordList.Convert(occurences);

                var processInfo = new ProcessStartInfo
                {
                    FileName = Constants.pathGenerateNewAudio,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    ArgumentList ={
                    "-s", source,
                    "-d", dest,
                    "-r", "",
                    "-j", JsonSerializer.Serialize(occurencesClean)
                }

                };

                using var process = new Process { StartInfo = processInfo };
                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    throw new Exception($"Generation failed: {error}");
                }

                Console.WriteLine($"Final file successful generated: {dest}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating final file: {ex.Message}");
            }
        }
    }
}


