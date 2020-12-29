using TMPro;
using System;
using System.Diagnostics;
using UnityEngine;

using System.Collections;

[ExecuteInEditMode]
public class BGCanvas : MonoBehaviour
{
  // refering to resource monitor of computer **WinOs only**
  PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
  PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available MBytes");
  public TextMeshProUGUI datetimeText;
  public TextMeshProUGUI performance;
  public TextMeshProUGUI temperature;

  // Start is called before the first frame update
  void Start()
  {
    print("VRAM " + SystemInfo.graphicsMemorySize + " MB");
    print("Processor Frequency " +SystemInfo.processorFrequency + " Mhz");
    print("RAM " + SystemInfo.systemMemorySize + " MB");
    datetimeText.richText = true;
    UpdateDateTime();
    UpdateWeather();
  }

  // Update is called once per frame
  void Update()
  {
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

  void UpdateWeather()
  {
    int temp = 30; //example have not utilize openweatherapi

    temperature.text = $"{temp} °C";
  }

}
