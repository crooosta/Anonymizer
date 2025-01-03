using NAudio.Wave;
using System.Diagnostics;

namespace Anonymizer.Classes
{
    public class SampleRateGetter
    {
        /// <summary>
        /// Get Sample rateof a file ONLY WINDOWS
        /// </summary>
        /// <param name="pathFile">Path of the file</param>
        public static int Get(string pathFile)
        {
            using (var reader = new Mp3FileReader(pathFile))
            {
                // Ottieni il sample rate
                int sampleRate = reader.Mp3WaveFormat.SampleRate;
                return sampleRate;
            }
        }
        public static int GetUNIX(string pathFile)
        {
                var processInfo = new ProcessStartInfo
                {
                    FileName = Constants.pathGetSampleRate,
                    Arguments = $"-s \"{pathFile}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using var process = new Process { StartInfo = processInfo };
                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit();
                int sampleRate = 0;
                Int32.TryParse(output, out sampleRate);
                return sampleRate;

        }
    }
}