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
from tensorflow_tts.inference import AutoConfig
from tensorflow_tts.inference import TFAutoModel
from tensorflow_tts.inference import AutoProcessor

processor = AutoProcessor.from_pretrained("../ljspeech_mapper.json")

# initialize fastspeech2 model.
config = AutoConfig.from_pretrained('../fastspeech2/conf/fastspeech2.v1.yaml')
fastspeech2 = TFAutoModel.from_pretrained(
    config=config, 
    pretrained_path="../fastspeech2/checkpoints/model-150000.h5",
    is_build=True,
    name="fastspeech2"
)

# initialize melgan model
melgan_config = AutoConfig.from_pretrained('../melgan/conf/melgan.v1.yaml')
melgan = TFAutoModel.from_pretrained(
  config=melgan_config,
  pretrained_path="../melgan/checkpoints/generator-1670000.h5"
)

input_text = "Hello, my name is Vox and I am a smart assistant!"
input_ids = processor.text_to_sequence(input_text)

# fastspeech2 inference
mel_before, mel_after, duration_outputs, _, _ = fastspeech2.inference(
  input_ids=tf.expand_dims(tf.convert_to_tensor(input_ids, dtype=tf.int32), 0),
  speaker_ids=tf.convert_to_tensor([0], dtype=tf.int32),
  speed_ratios=tf.convert_to_tensor([1.0], dtype=tf.float32),
  f0_ratios =tf.convert_to_tensor([1.0], dtype=tf.float32),
  energy_ratios =tf.convert_to_tensor([1.0], dtype=tf.float32),
)

# melgan inference
audio_before = melgan.inference(mel_before)[0, :, 0]
audio_after = melgan.inference(mel_after)[0, :, 0]

# save to file
sf.write('./audio_before.wav', audio_before, 22050, "PCM_16")
sf.write('./audio_after.wav', audio_after, 22050, "PCM_16")