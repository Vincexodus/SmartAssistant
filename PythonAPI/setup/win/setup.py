import subprocess
import os

print("\nMake sure you have brew installed before continuing the setup.")
input("press enter to continue...")

def pipinstall(package):
  os.system(f"sudo pip3 install {package}")


with open("./requirements.txt", "r") as f:
  packages = f.read().split("\n")
  for p in packages:
    pipinstall(p)

os.system(f"brew install ffmpeg")
os.system(f"brew install portaudio")

pipinstall("pyaudio")

import nltk
nltk.download("punkt")