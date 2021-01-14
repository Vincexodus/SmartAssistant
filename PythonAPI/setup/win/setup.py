import subprocess
import sys

def pipinstall(package):
  subprocess.check_call([sys.executable, "-m", "pip", "install", package])

def pipwininstall(package):
  subprocess.check_call([sys.executable, "-m", "pipwin", "install", package])

with open("./requirements.txt", "r") as f:
  packages = f.read().split("\n")
  for p in packages:
    pipinstall(p)

with open("./pipwinrequirements.txt", "r") as f:
  packages = f.read().split("\n")
  for p in packages:
    pipwininstall(p)

import nltk
nltk.download("punkt")