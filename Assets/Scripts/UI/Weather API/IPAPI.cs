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
  public static string country_name;
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
  }
  IEnumerator GetIPaddress() // get user's IP adress - 000.000.0.000
  {
    string ipNoUrl = "https://bot.whatismyipaddress.com/";
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
      country_name  = obj.country_name; // Malaysia
      string continent  = obj.continent_name;
      city_name = obj.city; // Shah Alam
      city_Id = obj.location.geoname_id;
      latitude = obj.latitude;
      longitude = obj.longitude;

    } catch(Exception e)
    {
      Debug.Log(e.StackTrace);
    }
  }
}
