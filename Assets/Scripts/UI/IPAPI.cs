using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;

public class IPAPI : MonoBehaviour
{
  /* 
    In order to use this API, you need to register on the website.

    API source : https://ipapi.com/
    Free package : 333 calls /day and 10,000 calls/month 

    Request by IP :
    http://api.ipapi.com/{IP address}?access_key={API key}

    API response docs : https://ipapi.com/documentation
  */
  
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
    ipURL = basicUrl + IPaddress + "?access_key=" + apiKey + "&format=1"; // add IP adress for IPAPI
    // full uri to be input in website

    StartCoroutine(GetIPinfo(ipURL));
    print("Current IP address detail url: "+ ipURL);
  }
  IEnumerator GetIPaddress() // get user's IP adress - 000.000.0.000
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
  IEnumerator GetIPinfo(string url) // get user's location, info,etc using IP address
  {
    yield return new WaitForSeconds(2f);
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
      string country  = obj.country_name; // Malaysia
      string continent  = obj.continent_name;
      city_name = obj.city; // Shah Alam
      city_Id = obj.location.geoname_id;
      latitude = obj.latitude;
      longitude = obj.longitude;

      // print(country);
      // print(city_Id);
      // print(city_name);
      // print(latitude+ "  " + longitude);
    } catch(Exception e)
    {
      Debug.Log(e.StackTrace);
    }
  }
}
