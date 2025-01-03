using Anonymizer;
using Anonymizer.Classes;

Constants.Initialize();
clearTempFolder();
clearOutputFolder();
Main main = new Main();

static void clearTempFolder()
{
    if (Directory.Exists(Constants.path_TempFiles))
        Directory.Delete(Constants.path_TempFiles, true);
    Directory.CreateDirectory(Constants.path_TempFiles);
}
static void clearOutputFolder()
{
    if (Directory.Exists(Constants.path_ModifiedFiles))
        Directory.Delete(Constants.path_ModifiedFiles, true);
    Directory.CreateDirectory(Constants.path_ModifiedFiles);
}