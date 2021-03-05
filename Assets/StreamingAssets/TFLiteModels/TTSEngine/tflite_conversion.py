# This program is free software; you can redistribute it and/or
# modify it under the terms of the GNU General Public License
# as published by the Free Software Foundation; either version 2
# of the License, or (at your option) any later version.

# This program is distributed in the hope that it will be useful,
# but WITHOUT ANY WARRANTY; without even the implied warranty of
# MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
# GNU General Public License for more details.

# You should have received a copy of the GNU General Public License
# along with this program; if not, write to the Free Software Foundation,
# Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301, USA.

# The Original Code is Copyright (C) 2020 Voxell Technologies.
# All rights reserved.

import soundfile as sf
import tensorflow as tf
import yaml

from tensorflow_tts.processor import LJSpeechProcessor
from tensorflow_tts.processor.ljspeech import valid_symbols

from tensorflow_tts.configs import FastSpeechConfig, FastSpeech2Config
from tensorflow_tts.configs import MelGANGeneratorConfig
from tensorflow_tts.inference import TFAutoModel, AutoConfig, AutoProcessor

from tensorflow_tts.models import TFFastSpeech, TFFastSpeech2
from tensorflow_tts.models import TFMelGANGenerator

# initialize melgan model
with open("./melgan/conf/melgan.v1.yaml") as f:
  melgan_config = yaml.load(f, Loader=yaml.Loader)
melgan_config = MelGANGeneratorConfig(**melgan_config["melgan_generator_params"])
melgan = TFMelGANGenerator(config=melgan_config, name="melgan_generator")
melgan._build()
melgan.load_weights("./melgan/checkpoints/generator-1670000.h5")

# initialize FastSpeech model.
with open("./fastspeech/conf/fastspeech.v1.yaml") as f:
  config = yaml.load(f, Loader=yaml.Loader)
config = FastSpeech2Config(**config["fastspeech_params"])
fastspeech = TFFastSpeech(
  config=config, name="fastspeech",
  enable_tflite_convertible=True)
fastspeech._build()
fastspeech.load_weights("./fastspeech/checkpoints/model-195000.h5")

# TF Lite for fastspeech
fastspeech_concrete_function = fastspeech.inference_tflite.get_concrete_function()

converter = tf.lite.TFLiteConverter.from_concrete_functions([fastspeech_concrete_function])
converter.optimizations = [tf.lite.Optimize.DEFAULT]
# converter.target_spec.supported_ops = [
#   tf.lite.OpsSet.TFLITE_BUILTINS,
#   tf.lite.OpsSet.SELECT_TF_OPS
#   ]
tflite_model = converter.convert()

# Save the TF Lite model.
with open("fastspeech_quant.tflite", "wb") as f:
  f.write(tflite_model)

print("Model size is %f MBs." % (len(tflite_model) / 1024 / 1024.0) )


# TF Lite for Melgan
melgan_concrete_function = melgan.inference_tflite.get_concrete_function()

converter = tf.lite.TFLiteConverter.from_concrete_functions([melgan_concrete_function])
converter.optimizations = [tf.lite.Optimize.DEFAULT]
# converter.target_spec.supported_ops = [
#   tf.lite.OpsSet.TFLITE_BUILTINS,
#   tf.lite.OpsSet.SELECT_TF_OPS
#   ]
tflite_model = converter.convert()

# Save the TF Lite model.
with open("melgan_quant.tflite", "wb") as f:
  f.write(tflite_model)

print("Model size is %f MBs." % (len(tflite_model) / 1024 / 1024.0) )