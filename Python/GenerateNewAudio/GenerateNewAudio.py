import json
from pydub import AudioSegment
import argparse
import sys

originalFilePath=""
finalFilePath = ""
replacementAudioFilePath=""
replacementList=[]

class Replacement:
    def __init__(self, start, end):
        self.start = start
        self.end = end

def generaFileAudio():
    global originalFilePath, finalFilePath, replacementAudioFilePath, replacementList

    # Creating AudioSegment object from original file
    originalAudio = AudioSegment.from_file(originalFilePath, format="wav")

    # Initializing new audio
    newAudio = AudioSegment.empty()

    # Support var
    end_prec=0

    for replacement in replacementList:

        # Evaluating interval to replace
        start =int(replacement.start * 1000)
        end =  int(replacement.end * 1000)

        if len(replacementAudioFilePath)>0:
            # Replace using the provided file (e.g. a beep)
            replacementAudio = AudioSegment.from_file(replacementAudioFilePath,format="wav")
        else:
            # Replace using silence
            replacementAudio = AudioSegment.silent(duration=end-start)

        newAudio += originalAudio[end_prec:start] + replacementAudio
        end_prec=end

    # If end_prec != audio duration I add the missing piece (from the last piece replaced at the end of the file)
    # newAudio += originalAudio[end_prec:originalAudio.duration_seconds*1000]
    newAudio += originalAudio[end_prec:]
    # Save the new audio in the provided path
    newAudio.export(finalFilePath, format="wav")

    print("OK")

def get_check_args():
    # Creo un parser degli argomenti
    parser = argparse.ArgumentParser(description="The following script allows you to replace a phrase within an audio file (in WAV format) with silence, or with a beep.\nTo execute the script it is necessary to provide all the required parameters!")

    # Aggiungo gli argomenti desiderati
    parser.add_argument('-s', '--source', help='Path of the file to modify',required=True)
    parser.add_argument('-d', '--dest', help='Path in which to save the modified file',required=True)
    parser.add_argument('-j', '--json', help='Json list containing the ranges to replace. E.g.: [{"start":0.56,"end":1.98},{"start":30.43,"end":41.98}]. ',required=True)
    parser.add_argument('-r', '--replace', help="Replacement Audio File Path (Leave blank for Silence)", default="")

    # Parso gli argomenti dalla riga di comando
    try:
        argomenti = parser.parse_args()
    except Exception as e:
        print("Errore durante il parsing degli argomenti:", e)
        sys.exit(1)
    return argomenti
def main():
    # Prelevo i parametri per lo script
    args = get_check_args()

    global originalFilePath, finalFilePath, replacementAudioFilePath, replacementList

    originalFilePath = args.source
    finalFilePath = args.dest
    replacementAudioFilePath=args.replace
    jsonReplacements = args.json
    # jsonReplacements='[{"start":12,"end":14},{"start":25,"end":27}]'
   
    #deserializzo in lista di sostituzioni l'argomento json
    try:
        data = json.loads(jsonReplacements)
    except Exception as e:
            print("KO-Errore durante la deserializzazione dell'argomento [-j][--json]. Argomento: " + jsonReplacements + " - Errore:", e)
            return
    replacementList = [Replacement(item['start'], item['end']) for item in data]
   
    if len(replacementList) ==0 :
        print("KO-Empty replacement list")
        return
    generaFileAudio()

if __name__ == "__main__":
    main()
