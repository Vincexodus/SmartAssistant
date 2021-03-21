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

import tensorflow as tf
import onnx
import keras2onnx
import yaml

from tensorflow_tts.configs import FastSpeechConfig
from tensorflow_tts.configs import MelGANGeneratorConfig

from tensorflow_tts.models import TFFastSpeech
from tensorflow_tts.models import TFMelGANGenerator

from tensorflow_tts.inference import AutoProcessor, AutoConfig, TFAutoModel

processor = AutoProcessor.from_pretrained(pretrained_path="ljspeech_mapper.json")
input_text = "i love you so much."
input_ids = processor.text_to_sequence(input_text)

config = AutoConfig.from_pretrained("./fastspeech/conf/fastspeech.v1.yaml")
fastspeech = TFAutoModel.from_pretrained(
  config=config, 
  pretrained_path="./fastspeech/checkpoints/model-195000.h5",
  is_build=True,
  name="fastspeech"
)

mel_before, mel_after, duration_outputs = fastspeech.inference(
    input_ids=tf.expand_dims(tf.convert_to_tensor(input_ids, dtype=tf.int32), 0),
    speaker_ids=tf.convert_to_tensor([0], dtype=tf.int32),
    speed_ratios=tf.convert_to_tensor([1.0], dtype=tf.float32),
)
tf.saved_model.save(fastspeech, "fastspeech", signatures=fastspeech.inference)

# fastspeech_onnx = keras2onnx.convert_keras(
#   fastspeech,
#   name="fastspeech", # the converted ONNX model internal name                     
#   # target_opset=9, # the ONNX version to export the model to
#   channel_first_inputs=input_ids
#   )

# onnx.save_model(fastspeech_onnx, "fastspeech.onnx")
