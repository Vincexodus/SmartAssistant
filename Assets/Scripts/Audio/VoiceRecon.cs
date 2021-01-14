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

using System.Collections; 
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceRecon : MonoBehaviour
{
  private KeywordRecognizer keywordRecognizer;
  [SerializeField]
  private string[] keywords;
  private Dictionary<string,Action> keywordDict; //format of dictionary
  
  void Start()
  {
    keywordDict = new Dictionary<string,Action>(); // new dictionary
    keywordDict.Add("hello", greet);

    keywordRecognizer = new KeywordRecognizer(keywordDict.Keys.ToArray());
    keywordRecognizer.OnPhraseRecognized += OnKeyWordRecognize;
    keywordRecognizer.Start();
  }

  private void OnKeyWordRecognize(PhraseRecognizedEventArgs speech)
  {
    Debug.Log("Key Word :" + speech.text);
    keywordDict[speech.text].Invoke();
  }

  private void greet()
  {
    print("hello there");
  }
}
