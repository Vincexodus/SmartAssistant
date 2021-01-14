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
public partial class BGCanvas : MonoBehaviour
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
  [Header("Weather")]
  public TextMeshProUGUI temperature;
  public TextMeshProUGUI windspeed;
  public TextMeshProUGUI description;
  public TextMeshProUGUI pressure;
  public TextMeshProUGUI Location;
  float currentTime = 0f;
  public float timer = 0f;
  public float delay = 1800f;
  #endregion

  [Header("Weather Usage")]
  string h24 = "00";
  public int skystate = 0;
  public Image weatherIcon;
  public Sprite[] Morning;
  public Sprite[] Noon;
  public Sprite[] Night;
  
  [Header("Additional scripts")]
  public IPAPI iPAPI;
  public WeatherAPI weatherAPI;

  //Morning : 5am-11:59am, Noon : 12pm-6:59pm, Night : 7pm-4:59am
  //05 <= Morning < 12, 12<= Noon < 19, 19 <= Night < 05
  
  void Start()
  {

    UpdateSkyState();
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
    UpdateSkyState();
  }

  void UpdateDateTime()
  {
    h24 = DateTime.Now.ToString("HH"); // 24 hour format
    string day = DateTime.Now.ToString("dd");
    string mth = DateTime.Now.ToString("MMM");
    string yr = DateTime.Now.ToString("yyyy");

    string sec = DateTime.Now.ToString("ss");
    string min = DateTime.Now.ToString("mm");
    string hr = DateTime.Now.ToString("hh");

    datetimeText.text = $"{day}<sup>th</sup> <sub>of</sub> {mth.ToLower()} {yr}\n{hr}:{min}:{sec}";
  }

  void UpdateSkyState()
  {
     //0- Morning, 1- Noon, 2-Night
    int h = int.Parse(h24);
    if(h >= 5 && h < 12) skystate = 0;
    else if(h >= 12 && h < 19) skystate = 1;
    else if(h >= 19 || h < 5) skystate = 2; // 19, 20, 21, 22 ,23, 00, 01, 02, 03, 04:59
    // else print("Time preset error");
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
