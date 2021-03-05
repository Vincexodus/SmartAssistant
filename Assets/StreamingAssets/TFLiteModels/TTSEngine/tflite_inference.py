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

import numpy as np
import tensorflow as tf
import matplotlib.pyplot as plt

from tensorflow_tts.inference import AutoProcessor

def visualize_mel_spectrogram(mels):
  mels = tf.reshape(mels, [-1, 80]).numpy()
  fig = plt.figure(figsize=(10, 8))
  ax1 = fig.add_subplot(311)
  ax1.set_title(f'Predicted Mel-after-Spectrogram')
  im = ax1.imshow(np.rot90(mels), aspect='auto', interpolation='none')
  fig.colorbar(mappable=im, shrink=0.65, orientation='horizontal', ax=ax1)
  plt.show()
  plt.close()

# Load the TFLite model and allocate tensors.
interpreter = tf.lite.Interpreter(model_path='fastspeech_quant.tflite', num_threads=2)
melgan = tf.lite.Interpreter(model_path="melgan_quant.tflite")

# Get input and output tensors.
input_details = interpreter.get_input_details()
output_details = interpreter.get_output_details()

melgan_input_details = melgan.get_input_details()
melgan_output_details = melgan.get_output_details()

# Prepare input data.
def prepare_input(input_ids):
  input_ids = tf.expand_dims(tf.convert_to_tensor(input_ids, dtype=tf.int32), 0)
  print(input_ids)
  return (input_ids,
          tf.convert_to_tensor([0], tf.int32),
          tf.convert_to_tensor([1.0], dtype=tf.float32),
          tf.convert_to_tensor([1.0], dtype=tf.float32),
          tf.convert_to_tensor([1.0], dtype=tf.float32))

# Test the model on random input data.
def infer(input_text):
  processor = AutoProcessor.from_pretrained(pretrained_path="ljspeech_mapper.json")
  input_ids = processor.text_to_sequence(input_text.lower())
  print(input_ids)
  interpreter.resize_tensor_input(input_details[0]['index'], 
                                  [1, len(input_ids)])
  interpreter.resize_tensor_input(input_details[1]['index'], 
                                  [1])
  interpreter.resize_tensor_input(input_details[2]['index'], 
                                  [1])
  interpreter.allocate_tensors()
  input_data = prepare_input(input_ids)
  for i, detail in enumerate(input_details):
    input_shape = detail['shape_signature']
    print(input_shape, input_data[i])
    interpreter.set_tensor(detail['index'], input_data[i])

  interpreter.invoke()

  # The function `get_tensor()` returns a copy of the tensor data.
  # Use `tensor()` in order to get a pointer to the tensor.
  return (interpreter.get_tensor(output_details[0]['index']),
          interpreter.get_tensor(output_details[1]['index']))

input_text = "what the fuck"

decoder_output_tflite, mel_output_tflite = infer(input_text)
print(decoder_output_tflite.shape, mel_output_tflite.shape)


# Get input and output tensors.
input_details = interpreter.get_input_details()
output_details = interpreter.get_output_details()

melgan_input_details = melgan.get_input_details()
melgan_output_details = melgan.get_output_details()

for fast in input_details:
  print(fast)
for fast in output_details:
  print(fast)

for mel in melgan_input_details:
  print(mel)
for mel in melgan_output_details:
  print(mel)

# audio_before_tflite = melgan(decoder_output_tflite)[0, :, 0]
# audio_after_tflite = melgan(mel_output_tflite)[0, :, 0]

# visualize_mel_spectrogram(decoder_output_tflite)
# visualize_mel_spectrogram(mel_output_tflite)