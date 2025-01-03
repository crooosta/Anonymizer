using System.Text.Json;
namespace Anonymizer.Classes
{
    public class VoskAPI
    {
        public static string smallModelPath = Constants.SmallModel;
        public static string bigModelPath = Constants.BigModel;

        public static RecognitionResult Recognize(string pathFile, bool useBigModel)
        {
            int lastPercentuale = 0;
            Vosk.Vosk.SetLogLevel(-1);

            // Create Model object for the recognition choosing between the big model and the small model
            Vosk.Model model = useBigModel ? new Vosk.Model(bigModelPath) : new Vosk.Model(smallModelPath);
            
            // Initialize Vosk Recognizer with the chosen model and a sample rate of 8000 Hz
            Vosk.VoskRecognizer recognizer = new Vosk.VoskRecognizer(model, SampleRateGetter.GetUNIX(pathFile));

            recognizer.SetMaxAlternatives(0);
            recognizer.SetWords(true);
            // Objects to store the result
            RecognitionResult finalResult = new RecognitionResult();
            Console.WriteLine($"Analyzing file... 0%");

            using (Stream source = File.OpenRead(pathFile))
            {
                // Defines a 4096-byte buffer to read the file in chunks
                byte[] buffer = new byte[4096];
                int bytesRead;

                long totalFileSize = source.Length;
                long totalBytesRead = 0;

                // Loop to read the audio file in chunks
                while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
                {
                    // Sends the data buffer to the Vosk recognizer
                    // If 'AcceptWaveform' returns true, it means an entire word or phrase was recognized
                    if (recognizer.AcceptWaveform(buffer, bytesRead))
                    {
                        // Retrieves the partial recognition result as a JSON string
                        // Deserializes it into a partial 'RecognitionResult' object
                        // And appends it to the final result
                        finalResult.Append(
                            JsonSerializer.Deserialize<RecognitionResult>(
                                recognizer.Result()
                                )
                            );
                    }
                    totalBytesRead += bytesRead;
                    int percentuale = (int)(totalBytesRead * 100 / totalFileSize);
                    if (percentuale % 10 == 0 && percentuale != lastPercentuale)
                    {
                        lastPercentuale = percentuale;
                        Console.WriteLine($"Analyzing file... {percentuale}%");
                    }

                }
            }

            // Retrieves the final result (remaining data) from the speech recognizer
            // Deserializes it into a 'RecognitionResult' object
            // And appends it to the overall result
            finalResult.Append(
                JsonSerializer.Deserialize<RecognitionResult>(
                    recognizer.FinalResult()
                    )
                );
            Console.WriteLine($"File Fully Analyzed");

            return finalResult;
        }
    }

}