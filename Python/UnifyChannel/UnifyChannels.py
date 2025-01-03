from pydub import AudioSegment
import os
import sys
import argparse

def get_check_args():
    # Creating an Argument Parser
    parser = argparse.ArgumentParser(description="The following script allows you to convert a stereo audio file to mono. \nTo run the script you must provide all the required parameters!")

    # Adding desired arguments
    parser.add_argument('-s', '--source', help='Path of the file to convert',required=True)
    parser.add_argument('-d', '--dest', help="Path in which to save the converted file",required=True)

    # Parsing the arguments from the command line
    try:
        arguments = parser.parse_args()
    except Exception as e:
        print("Error parsing arguments:", e)
        sys.exit(1)
    return arguments

def unifyChannels():
    # Reading parameters
    args = get_check_args()

    originalFilePath = args.source
    finalFilePath = args.dest
    
    originalAudio = AudioSegment.from_file(originalFilePath,format="wav")
    print("Actual Channels: ", originalAudio.channels)
    if originalAudio.channels ==2:
        finalAudio = originalAudio.set_channels(1)
    else:
        finalAudio=originalAudio

    # Generate new file
    finalAudio.export(finalFilePath, format="wav")

if __name__ == "__main__":
    unifyChannels()
    print("OK")