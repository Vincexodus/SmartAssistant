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

  void Awake()
  {
    audioVisualizer = new AudioVisualizer(ref audioSource, ref audioProfile);
    spectrumSmoother = new SpectrumSmoother(ref audioVisualizer.freqSize, ref smoothingIterations);
  }

  // Update is called once per frame
  void Update()
  {
    audioVisualizer.SampleSpectrum();
    audioVisualizer.RescaleSamples(Time.deltaTime);
  }

  void OnDrawGizmos()
  {
    if (Application.isPlaying)
    {
      Gizmos.color = Color.cyan;
      Gizmos.color *= new Color(1, 1, 1, 0.5f);
      for (int s=0; s < audioVisualizer.freq.Length; s++)
        Gizmos.DrawCube(transform.position + new Vector3(width*s + xOffset, 0, 0), new Vector3(width, audioVisualizer.freq[s], width));
    }
    
  }
}
