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

using UnityEngine;
using System.Collections;
public partial class BGCanvas : MonoBehaviour
{
  void UpdateSkyState()
  {
     //0- Morning, 1- Noon, 2-Night
    int h = int.Parse(h24);
    if(h >= 5 && h < 12) skystate = 0;
    else if(h >= 12 && h < 19) skystate = 1;
    else if(h >= 19 || h < 5) skystate = 2; // 19, 20, 21, 22 ,23, 00, 01, 02, 03, 04:59
    // else print("Time preset error");
  }
  void InitWeather()
  {
    UpdateSkyState();
    Location.text = $"{WebIP.state}, {WebIP.city}, {WebIP.country}"; // display Ip info
    IconChange();
    Debug.Log("Icon Updated !");
  }

  public void IconChange()
  {
    string main = WebIP.description;
    //Clear[0], Clouds[1], Drizzle[2], Rain[3], Thunderstorm[4], Snow[5]
    if(skystate == 0) //Morning
    {
      if(main == "clear sky") weatherIcon.sprite = Morning[0];
      else if(main == "few clouds" 
      ||main == "broken clouds"
      || main == "scattered clouds"
      || main == "overcast clouds") weatherIcon.sprite = Morning[1];
      else if(main == "light rain") weatherIcon.sprite = Morning[2];
      else if(main == "rain") weatherIcon.sprite = Morning[3];
      // else if(main == "Thunderstorm") weatherIcon.sprite = Morning[4];
      else if(main == "light snow" || main == "snow") weatherIcon.sprite = Morning[5];
      else Debug.Log("Weather State not found");
    }

    else if(skystate == 1) //Noon
    {
      if(main == "clear sky") weatherIcon.sprite = Noon[0];
      else if(main == "few clouds" 
      ||main == "broken clouds"
      || main == "scattered clouds"
      || main == "overcast clouds") weatherIcon.sprite = Noon[1];
      else if(main == "light rain") weatherIcon.sprite = Noon[2];
      else if(main == "rain") weatherIcon.sprite = Noon[3];
      // else if(main == "Thunderstorm") weatherIcon.sprite = Morning[4];
      else if(main == "light snow" || main == "snow") weatherIcon.sprite = Noon[5];
      else Debug.Log("Weather State not found");
    }
    else if(skystate == 2) //Night
    {
      if(main == "clear sky") weatherIcon.sprite = Night[0];
      else if(main == "few clouds" 
      ||main == "broken clouds"
      || main == "scattered clouds"
      || main == "overcast clouds") weatherIcon.sprite = Night[1];
      else if(main == "light rain") weatherIcon.sprite = Night[2];
      else if(main == "rain") weatherIcon.sprite = Night[3];
      // else if(main == "Thunderstorm") weatherIcon.sprite = Morning[4];
      else if(main == "light snow" || main == "snow") weatherIcon.sprite = Night[5];
      else Debug.Log("Weather State not found");
    }
    else Debug.Log("Sky state not found");
  }

  // refresh button call
  // void UpdateWeather() => weatherAPI.GetRealTimeWeather(ref temperature, ref windspeed, ref description ,ref pressure);

}