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

using System;
using System.Collections.Generic;
using UnityEngine;
using SmartAssistant.Core;
using SmartAssistant.Speech.TTS;
using SmartAssistant.Speech.STT;

namespace SmartAssistant
{
  public class SocketHandler : MonoBehaviour
  {
    public Socket socket;
    public TextToSpeech textToSpeech;
    public SpeechToText speechToText;

    void Start()
    {
      Core.Socket.socketClientActions[0] = SocketSpeak;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public string SocketSpeak(string text)
    {
      textToSpeech.Speak(text);
      return "";
    }

    // public void Action1(string input)
    // {
    //   print("Action 1 called");
    //   print($"The input is: {input}");
    // }

    public string Action2(string inputTxt)
    {
      print($"The input is: {inputTxt}");
      return "Test";
    }
  }
}