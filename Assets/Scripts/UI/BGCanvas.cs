using TMPro;
using System;
using System.Diagnostics;
using UnityEngine;

using System.Collections;

[ExecuteInEditMode]
public partial class BGCanvas : MonoBehaviour
{
  #region "Device Performance"
  // refering to resource monitor of computer **WinOs only**
  PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
  PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available MBytes");
  #endregion

  [Header("D&T")]
  public TextMeshProUGUI datetimeText;
  [Header("Perfomance")]
  public TextMeshProUGUI performance;
  [Header("Weather")]
  public TextMeshProUGUI temperature;
  public TextMeshProUGUI windspeed;
  public TextMeshProUGUI description;
  public TextMeshProUGUI pressure;
  public WeatherAPI weatherAPI;
  public IPAPI iPAPI;
  float currentTime = 0f;
  public float timer = 0f;
  public float delay = 1800f;

  // Start is called before the first frame update
  void Start()
  {
    StartCoroutine(InitWeather());
    currentTime = 0f;
    // print("VRAM " + SystemInfo.graphicsMemorySize + " MB");
    // print("Processor Frequency " +SystemInfo.processorFrequency + " Mhz");
    // print("RAM " + SystemInfo.systemMemorySize + " MB");
    datetimeText.richText = true;
  }

  // Update is called once per frame
  void Update()
  {
    currentTime = 1*Time.time;
    if(currentTime >= timer)
    {
      print("weather reload");
      // InitWeather();
      timer += delay;
    }
    UpdateDateTime();
  }

  void UpdateDateTime()
  {
    string day = DateTime.Now.ToString("dd");
    string mth = DateTime.Now.ToString("MMM");
    string yr = DateTime.Now.ToString("yyyy");

    string sec = DateTime.Now.ToString("ss");
    string min = DateTime.Now.ToString("mm");
    string hr = DateTime.Now.ToString("hh");

    datetimeText.text = $"{day}<sup>th</sup> <sub>of</sub> {mth.ToLower()} {yr}\n{hr}:{min}:{sec}";
  }

  void UpdatePerformance()
  {
    cpuCounter.NextValue();
    ramCounter.NextValue();
    // System.Threading.Thread.Sleep(1000);
    // yield return new WaitForSeconds(1);
    int cpu = (int)cpuCounter.NextValue();
    int ram = (int)ramCounter.NextValue();

    print(cpu);
    print(ram);

    performance.text = $"CPU : {cpu} %\nGPU : 0 \nRAM : {ram} %";
  }

}
