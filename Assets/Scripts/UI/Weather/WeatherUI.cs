using UnityEngine;
using System.Collections;
public partial class BGCanvas : MonoBehaviour
{
  IEnumerator InitWeather()
  {
    iPAPI.GetIP(); // get IP info-Location, Longitude,Latitude etc
    yield return new WaitForSeconds(5f); // waiting for data to be assigned to static vars

    Location.text = $"{IPAPI.city_name.ToString()}, {IPAPI.country_name.ToString()}"; // display Ip info
    weatherAPI.weather = new WeatherStatus();
    weatherAPI.GetRealTimeWeather(ref temperature, ref windspeed, ref description, ref pressure); // get weather info
    Debug.Log("Weather Updated !");
  }

  // refresh button call
  // void UpdateWeather() => weatherAPI.GetRealTimeWeather(ref temperature, ref windspeed, ref description ,ref pressure);

}