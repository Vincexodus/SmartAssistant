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
using TMPro;
using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

using System.Collections;

[ExecuteInEditMode]
public class BGCanvas : MonoBehaviour
{
  #region Device Performance
  // refering to resource monitor of computer **WinOs only**
  PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
  PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available MBytes");
  #endregion

  #region  Display Texts
  [Header("D&T")]
  public TextMeshProUGUI datetimeText;
  [Header("Perfomance")]
  public TextMeshProUGUI performance;
  
  // float currentTime = 0f;
  public float timer = 0f;
  public float delay = 1800f;
  #endregion

  [HideInInspector]
  public string h24 = "00";
  
  [Header("Additional scripts")]
  public WebIP web;
  public WeatherUI UI;

  void Start()
  {
    UpdateWeather();
    // currentTime = 0f;
    // print("VRAM " + SystemInfo.graphicsMemorySize + " MB");
    // print("Processor Frequency " +SystemInfo.processorFrequency + " Mhz");
    // print("RAM " + SystemInfo.systemMemorySize + " MB");
    datetimeText.richText = true;
  }

  // Update is called once per frame
  void Update()
  {
    // currentTime = 1*Time.time;
    // if(currentTime >= timer)
    // {
    //   print("weather reload");
    //   // InitWeather();
    //   timer += delay;
    // }
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

  public void UpdateWeather()
  {
    web.Main();
    UI.UpdateSkyState();
    UI.IconChange();
    UI.UpdateText();
    print("weather Updated!");
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
