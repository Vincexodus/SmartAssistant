import numpy as np
import soundfile as sf
import matplotlib.pyplot as plt

import tensorflow as tf

from tensorflow_tts.inference import AutoConfig
from tensorflow_tts.inference import TFAutoModel
from tensorflow_tts.inference import AutoProcessor

processor = AutoProcessor.from_pretrained("./ljspeech_mapper.json")


config = AutoConfig.from_pretrained("./tacotron2/conf/tacotron2.v1.yaml")
tacotron2 = TFAutoModel.from_pretrained(
  config=config, 
  pretrained_path=None,
  is_build=True,
  name="tacotron2"
)

tacotron2.setup_window(win_front=6, win_back=6)
tacotron2.setup_maximum_iterations(3000)

tacotron2.load_weights("./tacotron2/checkpoints/model-120000.h5")
# tf.saved_model.save(tacotron2, "./tacotron2/inference", signatures=tacotron2.inference)
# tacotron2 = tf.saved_model.load("./tacotron2/inference")

input_text = "everything is copacitic"
input_ids = processor.text_to_sequence(input_text)


decoder_output, mel_outputs, stop_token_prediction, alignment_history = tacotron2.inference(
  tf.expand_dims(tf.convert_to_tensor(input_ids, dtype=tf.int32), 0),
  tf.convert_to_tensor([len(input_ids)], tf.int32),
  tf.convert_to_tensor([0], dtype=tf.int32)
)

# initialize melgan model
melgan_config = AutoConfig.from_pretrained('./melgan/conf/melgan.v1.yaml')
melgan = TFAutoModel.from_pretrained(
  config=melgan_config,
  pretrained_path="./melgan/checkpoints/generator-1500000.h5"
)

# melgan inference
audio_before = melgan.inference(decoder_output)[0, :, 0]
audio_after = melgan.inference(mel_outputs)[0, :, 0]

# save to file
sf.write('./audio_before.wav', audio_before, 22050, "PCM_16")
sf.write('./audio_after.wav', audio_after, 22050, "PCM_16")