using System;
using System.Collections;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

using TMPro;

public class WeatherAPI : MonoBehaviour
{
  //Free package :60 calls/minute and 1,000,000 calls/month
  // credits to youtube channel : RumpledCode 
  /*
		In order to use this API, you need to register on the website.

		Source: https://openweathermap.org

    Request by cityname: 
    api.openweathermap.org/data/2.5/weather?q={city name}&appid={API key}
    api.openweathermap.org/data/2.5/weather?q={city name},{state code}&appid={API key}

		Request by cityId: 
    api.openweathermap.org/data/2.5/weather?id={city id}&appid={API key}

		Request by lat-long: 
    api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={your api key}

		Api response docs: https://openweathermap.org/current
	*/
  public int id;

  public string apiKey = "bead25a731aaedda8c9f75407ff3d110"; //Vincent' api key dk bout this confidential or not
  public string cityName;
  public string cityId;
  public bool useCoords = false;
  public string latitude;
  public string longitude;

  public WeatherStatus weather;

  public void GetRealTimeWeather(ref TextMeshProUGUI temperature)
  {
    string uri = "api.openweathermap.org/data/2.5/weather?";
    if(useCoords)
    {
      uri += "lat=" + latitude + "&lon=" + longitude + "&appid=" + apiKey;
    }else
    {
      uri += "id=" + cityId + "&appid=" + apiKey;
    }
    // full uri to be input in website
    StartCoroutine(GetCurrentWeather(uri, temperature));
  }

  IEnumerator GetCurrentWeather(string uri, TextMeshProUGUI temperature)
  {
    using(UnityWebRequest webRequest = UnityWebRequest.Get(uri))
    {
      yield return webRequest.SendWebRequest();
      // download text in json format after search on website
      if(webRequest.result == UnityWebRequest.Result.ConnectionError)
        Debug.Log("Web request error :" + webRequest.error);
      else
      {
        ParseJson(webRequest.downloadHandler.text);
        temperature.text = weather.Celsius().ToString();
        print("Weather ID : " + weather.weatherId);
        print("Wind speed : " + weather.windSpeed + "m/s");
        print("Temp : " + weather.Celsius() + "°C");
        print("Temp : " + weather.Fahrenheit() + "°F");
        print("Weather description : " + weather.description);
        print("Icon : " + weather.weatherIcon);
        print("Pressure : " + weather.pressure + "hPa");
      }
    }
  }

  // Class to decode json not sure
  public void ParseJson(string json)
  {
    try
    {
      //Convert a string representation of number to an integer
      dynamic obj = JObject.Parse(json);

      //assign values of result to variables of WeatherStatus script
      weather.weatherId = obj.weather[0].id;
      weather.main = obj.weather[0].main;
      weather.description = obj.weather[0].description;
      weather.weatherIcon = obj.weather[0].icon; //image of the condition of weather: sunny, rainny
      weather.temperature = obj.main.temp;
      weather.pressure = obj.main.pressure;
      weather.windSpeed = obj.wind.speed;
    } catch(Exception e)
    {
      Debug.Log(e.StackTrace);
    }
  }

}
