using NAudio.Wave;
using System.Diagnostics;

namespace Anonymizer.Classes
{
    public class Mp3ToWav
    {
        /// <summary>
        /// Convert an Mp3 file to Wav ONLY WINDOWS
        /// </summary>
        /// <param name="pathMp3Source">Path of the file to convert</param>
        /// <param name="pathWavDest">Path in which to save the converted file</param>
        public static void Convert(string pathMp3Source, string pathWavDest)
        {
            using (var reader = new Mp3FileReader(pathMp3Source))
            {
                WaveFileWriter.CreateWaveFile(pathWavDest, reader);
            }
        }
        public static void ConvertUNIX(string mp3Path, string wavPath)
        {
            Console.WriteLine($"Converting MP3 to WAV...");
            try
            {
                var processInfo = new ProcessStartInfo
                {
                    FileName = "ffmpeg",
                    Arguments = $"-i \"{mp3Path}\" \"{wavPath}\"",
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

                if (process.ExitCode != 0)
                {
                    throw new Exception($"FFmpeg conversion failed: {error}");
                }

                Console.WriteLine($"Conversion successful: {wavPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error converting MP3 to WAV: {ex.Message}");
            }
        }


    }
}