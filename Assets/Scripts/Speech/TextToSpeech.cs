/*
This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software Foundation,
Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301, USA.

The Original Code is Copyright (C) 2020 Voxell Technologies.
All rights reserved.
*/

using UnityEngine;
using TensorFlowLite;
using System;

namespace SmartAssistant.Speech.TTS
{
  public partial class TextToSpeech : MonoBehaviour
  {
    public string TTSFilepath;
    public int speakerID = 1;
    public float speedRatio = 1.0f;
    [SerializeField]
    public InterpreterOptions interpreterOptions;
    public Interpreter interpreter;

    private Interpreter.TensorInfo[] _inputDetails;

    void Start()
    {
      interpreterOptions = new InterpreterOptions(){threads = 2};
      interpreter = new Interpreter(FileUtil.LoadFile(TTSFilepath), interpreterOptions);

      for (int d=0; d < 3; d++)
        _inputDetails[d] = interpreter.GetInputTensorInfo(d);

      InitSpeechProcessor();
    }

    private Array[] PrepareInput(ref int[] inputIDs, ref int speakerID, ref float speedRatio)
    {
      Array[] inputData = new Array[_inputDetails.Length];

      int[,] formatedInputIDS = new int[_inputDetails[0].shape[0], _inputDetails[0].shape[1]];
      for (int i=0; i < inputIDs.Length; i++) formatedInputIDS[0, i] = inputIDs[i];
      inputData[0] = formatedInputIDS;
      inputData[1] = new int[1]{speakerID};
      inputData[2] = new float[1]{speedRatio};

      return inputData;
    }

    public void TTSInference(string text)
    {
      // convert text to speech here and play it
      int[] inputIDs = TextToSequence(ref text);
      interpreter.ResizeInputTensor(0, new int[]{1, inputIDs.Length});
      interpreter.ResizeInputTensor(1, new int[]{1});
      interpreter.ResizeInputTensor(2, new int[]{1});
      interpreter.AllocateTensors();

      Array[] inputData = PrepareInput(ref inputIDs, ref speakerID, ref speedRatio);

      for (int d=0; d < _inputDetails.Length; d++)
        interpreter.SetInputTensorData(d, inputData[d]);

      interpreter.Invoke();
    }
  }
}