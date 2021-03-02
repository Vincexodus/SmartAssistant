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

using System;
using System.Web;
using System.Collections;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

using TMPro;

public class WeatherAPI : MonoBehaviour
{
  public string apiKey = "bead25a731aaedda8c9f75407ff3d110"; //Vincent' openweatherapi key dk bout this confidential or not
  public bool useCoords = false;
  public string cityName;
  public string cityId;
  public string latitude;
  public string longitude;
  public WeatherStatus weather;
  public static string weatherMain;

  public void GetRealTimeWeather(
    ref TextMeshProUGUI temperature, 
    ref TextMeshProUGUI windSpeed, 
    ref TextMeshProUGUI description, 
    ref TextMeshProUGUI pressure)
  {
    string uri = "api.openweathermap.org/data/2.5/weather?";
    if(useCoords)
    {
      uri += "lat=" + IPAPI.latitude + "&lon=" + IPAPI.longitude + "&appid=" + apiKey;
    }else
    {
      uri += "id=" + IPAPI.city_Id + "&appid=" + apiKey;
    }
    // full uri to be input in website
    StartCoroutine(GetCurrentWeather(uri, temperature, windSpeed, description, pressure));
  }
  IEnumerator GetCurrentWeather(string uri, 
    TextMeshProUGUI temperature, 
    TextMeshProUGUI windSpeed, 
    TextMeshProUGUI description, 
    TextMeshProUGUI pressure)
  {
    yield return new WaitForSeconds(2f);
    using(UnityWebRequest webRequest = UnityWebRequest.Get(uri))
    {
      yield return webRequest.SendWebRequest();
      // download text in json format after search on website
      if(webRequest.result == UnityWebRequest.Result.ConnectionError)
        Debug.Log("Web request error :" + webRequest.error);
      else
      {
        ParseJson(webRequest.downloadHandler.text);
        temperature.text = $"{weather.Celsius().ToString("0.0")} °C\n{weather.Fahrenheit().ToString("0.0")} °F";
        windSpeed.text = $"{weather.windSpeed.ToString()} m/s";
        description.text = weather.description;
        pressure.text = $"{weather.pressure.ToString()} hPa";
        weatherMain = weather.main;
        // print("Weather ID : " + weather.weatherId);
      }
    }
  }

  public void ParseJson(string json)
  {
    try
    {
      //Convert a string representation of number to an integer
      dynamic obj = JObject.Parse(json);

      //assign values of result to variables of WeatherStatus script
      weather.weatherId = obj.weather[0].id;
      weather.main = obj.weather[0].main;
      weather.description = obj.weather[0].description;//image of the condition of weather: sunny, rainny
      weather.temperature = obj.main.temp;
      weather.pressure = obj.main.pressure;
      weather.windSpeed = obj.wind.speed;
    } catch(Exception e)
    {
      Debug.Log(e.StackTrace);
    }
  }

}
