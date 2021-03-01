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
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Linq;
using Newtonsoft.Json;
using HtmlAgilityPack;

public class WebIP : MonoBehaviour
{
  public static float latitude, longitude;
  public static string city, state, country;
  public static int cityCode;
  public static string temperature;
  public static string description, humid, windSpeed, windDir;

  //https://www.bing.com/search?q=region+weather

  public void IPHtml() // get details of IP adress- location, etc.
  {
    HtmlWeb web = new HtmlWeb();
    var htmlDoc = web.Load("https://mylocation.org/");
    HtmlNode[] nodes = htmlDoc.DocumentNode.SelectNodes("//td").ToArray();
    latitude = float.Parse(nodes[3].InnerHtml);
    longitude = float.Parse(nodes[5].InnerHtml);
    // for(int i = 2; i< 12; i++) //2-11
    // {
    //   HtmlNode a = nodes[i];
    //   if(i%2 == 1)
    //   {
    //     print(i + a.InnerHtml);
    //   }
    // }
    city = nodes[11].InnerHtml;
    state = nodes[9].InnerHtml;
    country = nodes[7].InnerHtml;
    string coords =$"Lat= {latitude} , long = {longitude}";
    string location =$"Location = {city}, {state}, {country}";
    print(location);
  }
  public void WeatherHtml()
  {
    // cityCode = 1733046;
    // compare city name to get code in city list
    string url = "https://www.nearweather.com/location/" + cityCode;
    HtmlWeb web = new HtmlWeb();
    var htmlDoc = web.Load(url);
    HtmlNode[] tempNode = htmlDoc.DocumentNode.SelectNodes("//*[@class = 'wn-box wn-temperature']").ToArray();
    temperature = tempNode[0].InnerText;

    HtmlNode[] detailNode = htmlDoc.DocumentNode.SelectNodes("//*[@class = 'wn-conditions-text']").ToArray();
    string[] condition = detailNode[0].InnerText.Split(' ');
    description = condition[2] + " " + condition[3];

    string[] humidity = detailNode[1].InnerText.Split(' ');
    humid = humidity[1];

    string[] windspeed = detailNode[2].InnerText.Split(' ');
    windSpeed = windspeed[2];

    string[] windDirection = detailNode[3].InnerText.Split(' ');
    windDir = windDirection[2];
    print(temperature + " " + description + " " + humid + " " + windSpeed + " " + windDir );
  }
  public void ReadJson()
  {
    var currentDirectory = Environment.CurrentDirectory;
    string path = currentDirectory +  @"\Assets\StreamingAssets\CityList.json";
    using(StreamReader r = new StreamReader(path))
    {
      string json = r.ReadToEnd();
      List<CityList> cityList = JsonConvert.DeserializeObject<List<CityList>>(json);
      foreach(var item in cityList)
      {
        if(item.name == state) // city may have similar first names
        {
          cityCode = item.id;
        }
      }
    }
    print(cityCode);
  }
  public void Main()
  {
    IPHtml();
    ReadJson();
    WeatherHtml();
  }
}

  [Serializable]
  public class CityList
  {
    public int id;
    public string name;
    public string state;
    public string country;
    // public string coord; //idk which data type {a,b} not necessary
  }


