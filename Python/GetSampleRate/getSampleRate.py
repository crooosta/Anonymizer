from pydub.utils import mediainfo
import os
import sys
import argparse

def get_check_args():
    # Creating an Argument Parser
    parser = argparse.ArgumentParser(description="The following script allows you to get teh sample rate of an audio file. \nTo run the script you must provide all the required parameters!")

    # Adding desired arguments
    parser.add_argument('-s', '--source', help='Path of the file',required=True)

    # Parsing the arguments from the command line
    try:
        arguments = parser.parse_args()
    except Exception as e:
        print("Error parsing arguments:", e)
        sys.exit(1)
    return arguments

def get_sample_rate():
    # Reading parameters
    args = get_check_args()

    info = mediainfo(args.source)
    return int(info['sample_rate'])

if __name__ == "__main__":
   print(get_sample_rate())
    