using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;

public class IPAPI : MonoBehaviour
{
  public string apiKey = "c6789f32aa819401307f4f3a9be3d2c7"; //Vincent' IPAPI key dk bout this confidential or not
  [HideInInspector] public string IPaddress;
  [HideInInspector] public string ipURL;

  [Header("Location Info")]
  public static string city_name;
  public static int city_Id;
  public static float latitude, longitude;

  public void GetIP()
  {
    StartCoroutine(GetIPaddress());
    string basicUrl = "http://api.ipapi.com/";
    ipURL = basicUrl + IPaddress + "?access_key=" + apiKey + "&format=1";
    // full uri to be input in website
    print("Current IP address detail: "+ ipURL);
    StartCoroutine(GetIPinfo(ipURL));
  }
  IEnumerator GetIPaddress()
  {
    string ipNoUrl = "http://bot.whatismyipaddress.com/";
    using(UnityWebRequest webRequest = UnityWebRequest.Get(ipNoUrl))
    {
      yield return webRequest.SendWebRequest();
      // download text in json format after search on website
      if(webRequest.result == UnityWebRequest.Result.ConnectionError)
        Debug.Log("Web request error :" + webRequest.error);
      else
      {
        IPaddress = webRequest.downloadHandler.text;
      }
    }
  }
  IEnumerator GetIPinfo(string url)
  {
    using(UnityWebRequest webRequest = UnityWebRequest.Get(url))
    {
      yield return webRequest.SendWebRequest();
      // download text in json format after search on website
      if(webRequest.result == UnityWebRequest.Result.ConnectionError)
        Debug.Log("Web request error :" + webRequest.error);
      else
      {
        IPDetail(webRequest.downloadHandler.text);
      }
    }
  }

  public void IPDetail(string json)
  {
    try
    {
      //Convert json string to objects
      dynamic obj = JObject.Parse(json);
      string country  = obj.country_name;
      string continent  = obj.continent_name;
      city_name = obj.city;
      city_Id = obj.location.geoname_id;
      latitude = obj.latitude;
      longitude = obj.longitude;

      print(country);
      print(city_Id);
      print(city_name);
      print(latitude+ "  " + longitude);
    } catch(Exception e)
    {
      Debug.Log(e.StackTrace);
    }
  }
}
