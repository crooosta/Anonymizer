using Microsoft.Extensions.Configuration;

namespace Anonymizer.Classes
{
    public class Constants
    {
        public static string path_ToAnalyze = "";
        public static string path_ModifiedFiles = "";
        public static string path_OriginalFiles = "";
        public static string path_TempFiles = "";
        public static string pathUnifyChannels = "";
        public static string pathGetSampleRate = "";
        public static string pathGenerateNewAudio= "";
        public static string BigModel = "";
        public static string SmallModel = "";

        public static void Initialize()
        {
            var configuration = new ConfigurationBuilder()
                   .SetBasePath(AppContext.BaseDirectory)
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                   .Build();

            Constants.path_ToAnalyze = configuration["path_ToAnalyze"];
            Constants.path_OriginalFiles = configuration["path_OriginalFiles"];
            Constants.path_ModifiedFiles = configuration["path_ModifiedFiles"];
            Constants.path_TempFiles = configuration["path_TempFiles"];
            Constants.pathUnifyChannels = configuration["pathUnifyChannels"];
            Constants.pathGetSampleRate = configuration["pathGetSampleRate"];
            Constants.pathGenerateNewAudio = configuration["pathGenerateNewAudio"];
            Constants.BigModel = configuration["BigModel"];
            Constants.SmallModel = configuration["SmallModel"];
        }

    }
}