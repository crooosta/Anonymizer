# Anonymizer
Replacing words in phone recording files using speech recognition via Vosk API

Download models from Vosk Official Repository: https://alphacephei.com/vosk/models

## 1. Configure the appsettings.json file
You have to indicate your local paths for:
  1. File Folders
  2. Python Scripts
  3. Recognition Models

## 2. Fill the records.json file
You have to insert a record for each file to analyze, indicating: 
  1. operatorName -> Target Phrase to replace
  2. Mp3FilePath -> Name of the file
  3. isAlreadyWav -> boolean to indicate if Anonymizer has to convert the file
  4. isAlreadyWav -> boolean to indicate if Anonymizer has to unify the channels
