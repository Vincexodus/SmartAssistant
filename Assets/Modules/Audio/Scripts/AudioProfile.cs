using UnityEngine;

namespace Audio
{
  [System.Serializable]
  public class AudioProfile
  {
    public int channel;
    public FFTWindow window;
    public int sampleSize;
    public int bufferSize;
    public float power;
    public float scale;
    public float sensitivity;
    public int smoothingIterations;
    [Range(100, 20000)]
    public float freqRange;
  }
}