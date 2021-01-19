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

processor = AutoProcessor.from_pretrained("./ljspeech_mapper.json")

# initialize fastspeech model.
fs_config = AutoConfig.from_pretrained('./fastspeech/conf/fastspeech.v1.yaml')
fastspeech = TFAutoModel.from_pretrained(
  config=fs_config,
  pretrained_path="./fastspeech/checkpoints/model-195000.h5"
)

# initialize melgan model
melgan_config = AutoConfig.from_pretrained('./melgan/conf/melgan.v1.yaml')
melgan = TFAutoModel.from_pretrained(
  config=melgan_config,
  pretrained_path="./melgan/checkpoints/generator-1500000.h5"
)


ids = processor.text_to_sequence("how much wood would a woodchuck chuck if a woodchuck could chuck wood?")
print(ids)
ids = tf.expand_dims(ids, 0)
# fastspeech inference

masked_mel_before, masked_mel_after, duration_outputs = fastspeech.inference(
  ids,
  speaker_ids=tf.zeros(shape=[tf.shape(ids)[0]], dtype=tf.int32),
  speed_ratios=tf.constant([1.0], dtype=tf.float32)
)

# melgan inference
audio_before = melgan.inference(masked_mel_before)[0, :, 0]
audio_after = melgan.inference(masked_mel_after)[0, :, 0]

# save to file
sf.write('./audio_before.wav', audio_before, 22050, "PCM_16")
sf.write('./audio_after.wav', audio_after, 22050, "PCM_16")