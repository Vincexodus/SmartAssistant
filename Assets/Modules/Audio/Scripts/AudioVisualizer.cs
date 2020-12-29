using UnityEngine;

namespace Audio
{
  public class AudioVisualizer
  {
    public AudioSource source;
    public AudioProfile profile;
    public AudioVisualizer(ref AudioSource source, ref AudioProfile profile)
    {
      this.source = source;
      this.profile = profile;

      Init();
    }

    public float[] samples;
    public float[][] sampleBuffers;

    public float[] freq;
    public int freqSize;

    public float time = 0.0f;

    public void Init()
    {
      samples = new float[profile.sampleSize];
      sampleBuffers = new float[profile.sampleSize][];

      float freqInterval = AudioSettings.outputSampleRate/profile.sampleSize;
      float currFreq = 0.0f;
      freqSize = 0;

      for (int s=0; s < profile.sampleSize; s++)
      {
        currFreq += freqInterval;
        if (currFreq < profile.freqRange)
          freqSize ++;
        else break;
      }
      freq = new float[freqSize];

      for (int s=0; s < profile.sampleSize; s++)
        sampleBuffers[s] = new float[profile.bufferSize];
    }

    public void SampleSpectrum() => source.GetSpectrumData(samples, profile.channel, profile.window);

    public void RescaleSamples(float deltaTime)
    {
      for (int f=1; f < freqSize; f++)
      {
        samples[f] = Mathf.Pow(Mathf.Sqrt(samples[f]), profile.power) * profile.scale;

        // populate buffers
        for (int b=1; b < profile.bufferSize; b++)
          sampleBuffers[f][b] = sampleBuffers[f][b-1];

        // push in newest sample into buffer
        sampleBuffers[f][profile.bufferSize-1] = samples[f];
        float minBuffer = Mathf.Min(sampleBuffers[f]);
        float maxBuffer = Mathf.Max(sampleBuffers[f]);

        float average = (maxBuffer - minBuffer)/2;

        freq[f-1] = Mathf.Lerp(average, samples[f], profile.sensitivity);
      }
    }

  }
}