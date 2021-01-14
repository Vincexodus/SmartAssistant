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
using System.Collections.Generic;
using UnityEngine;

using Audio;

public class AudioVisualizerDebug : MonoBehaviour
{
  public AudioSource audioSource;
  public AudioProfile audioProfile;
  public AudioVisualizer audioVisualizer;
  public SpectrumSmoother spectrumSmoother;

  public int smoothingIterations = 10;
  public float width;
  public float xOffset;

  int freqRange;
  int bandSize;

  void Awake()
  {
    audioVisualizer = new AudioVisualizer(ref audioSource, ref audioProfile);
    spectrumSmoother = new SpectrumSmoother(ref audioVisualizer.freqSize, ref smoothingIterations);
    bandSize = audioProfile.bandSize;
  }

  // Update is called once per frame
  void Update()
  {
    if (audioProfile.freqRange != freqRange)
    {
      audioVisualizer.Init();
      freqRange = audioProfile.freqRange;
    }
    if (audioProfile.bandSize != bandSize)
    {
      audioVisualizer.Init();
      bandSize = audioProfile.bandSize;
    }

    audioVisualizer.SampleSpectrum();
    audioVisualizer.RescaleSamples(Time.deltaTime);
  }

  void OnDrawGizmos()
  {
    if (Application.isPlaying)
    {
      Gizmos.color = Color.cyan;
      Gizmos.color *= new Color(1, 1, 1, 0.5f);
      for (int s=0; s < audioVisualizer.band.Length; s++)
        Gizmos.DrawCube(transform.position + new Vector3(width*s + xOffset, 0, 0), new Vector3(width, audioVisualizer.freq[s], width));
    }
    
  }
}
