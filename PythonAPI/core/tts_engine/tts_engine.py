import numpy as np
import soundfile as sf
import matplotlib.pyplot as plt

import tensorflow as tf

from tensorflow_tts.inference import AutoConfig
from tensorflow_tts.inference import TFAutoModel
from tensorflow_tts.inference import AutoProcessor


# tacotron2
processor = AutoProcessor.from_pretrained("./ljspeech_mapper.json")


# config = AutoConfig.from_pretrained("./tacotron2/conf/tacotron2.v1.yaml")
# tacotron2 = TFAutoModel.from_pretrained(
#   config=config, 
#   pretrained_path=None,
#   is_build=True,
#   name="tacotron2"
# )

# tacotron2.setup_window(win_front=6, win_back=6)
# tacotron2.setup_maximum_iterations(3000)

# tacotron2.load_weights("./tacotron2/checkpoints/model-120000.h5")
# # tf.saved_model.save(tacotron2, "./tacotron2/inference", signatures=tacotron2.inference)
# # tacotron2 = tf.saved_model.load("./tacotron2/inference")

# input_text = "what is the color of the sky?"
# input_ids = processor.text_to_sequence(input_text)


# decoder_output, mel_outputs, stop_token_prediction, alignment_history = tacotron2.inference(
#   tf.expand_dims(tf.convert_to_tensor(input_ids, dtype=tf.int32), 0),
#   tf.convert_to_tensor([len(input_ids)], tf.int32),
#   tf.convert_to_tensor([0], dtype=tf.int32)
# )

# # initialize melgan model
# melgan_config = AutoConfig.from_pretrained('./melgan/conf/melgan.v1.yaml')
# melgan = TFAutoModel.from_pretrained(
#   config=melgan_config,
#   pretrained_path="./melgan/checkpoints/generator-1500000.h5"
# )

# # melgan inference
# audio_before = melgan.inference(decoder_output)[0, :, 0]
# audio_after = melgan.inference(mel_outputs)[0, :, 0]

# # save to file
# sf.write('./audio_before.wav', audio_before, 22050, "PCM_16")
# sf.write('./audio_after.wav', audio_after, 22050, "PCM_16")

# # initialize fastspeech2 model.
# fs_config = AutoConfig.from_pretrained('./fastspeech2/conf/fastspeech2.v1.yaml')
# fastspeech2 = TFAutoModel.from_pretrained(
#     config=fs_config, 
#     pretrained_path="./fastspeech2/checkpoints/model-150000.h5",
#     is_build=True,
#     name="fastspeech2"
# )


# # initialize melgan model
# melgan_config = AutoConfig.from_pretrained('./melgan/conf/melgan.v1.yaml')
# melgan = TFAutoModel.from_pretrained(
#   config=melgan_config,
#   pretrained_path="./melgan/checkpoints/generator-1500000.h5"
# )


# # inference
# processor = AutoProcessor.from_pretrained(pretrained_path="./ljspeech_mapper.json")

# input_text = "how much wood would a woodchuck chuck if a woodchuck could chuck wood?"
# input_ids = processor.text_to_sequence(input_text)
# # fastspeech2 inference

# mel_before, mel_after, duration_outputs, _, _ = fastspeech2.inference(
#   input_ids=tf.expand_dims(tf.convert_to_tensor(input_ids, dtype=tf.int32), 0),
#   speaker_ids=tf.convert_to_tensor([0], dtype=tf.int32),
#   speed_ratios=tf.convert_to_tensor([1.0], dtype=tf.float32),
#   f0_ratios =tf.convert_to_tensor([1.0], dtype=tf.float32),
#   energy_ratios =tf.convert_to_tensor([1.0], dtype=tf.float32),
# )

# # melgan inference
# audio_before = melgan.inference(mel_before)[0, :, 0]
# audio_after = melgan.inference(mel_after)[0, :, 0]

# # save to file
# sf.write('./audio_before.wav', audio_before, 22050, "PCM_16")
# sf.write('./audio_after.wav', audio_after, 22050, "PCM_16")


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


# inference

ids = processor.text_to_sequence("how much wood would a woodchuck chuck if a woodchuck could chuck wood?")
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