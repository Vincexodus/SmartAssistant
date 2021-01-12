using UnityEngine;
using System.Collections;
public partial class BGCanvas : MonoBehaviour
{
  IEnumerator InitWeather()
  {
    iPAPI.GetIP(); // get IP info-Location, Longitude,Latitude etc

    // this may varies with connection
    yield return new WaitForSeconds(7f); // waiting for data to be assigned to static vars
    Location.text = $"{IPAPI.city_name.ToString()}, {IPAPI.country_name.ToString()}"; // display Ip info
    weatherAPI.weather = new WeatherStatus();
    weatherAPI.GetRealTimeWeather(ref temperature, ref windspeed, ref description, ref pressure); // get weather info
    
    yield return new WaitForSeconds(7f); // waiting for data transfer
    IconChange();
    print( WeatherAPI.weatherMain);
    Debug.Log("Weather Updated !");
  }

  public void IconChange()
  {
    string main = WeatherAPI.weatherMain;
    //Clear[0], Clouds[1], Drizzle[2], Rain[3], Thunderstorm[4], Snow[5]
    if(skystate == 0) //Morning
    {
      if(main == "Clear") weatherIcon.sprite = Morning[0];
      else if(main == "Clouds") weatherIcon.sprite = Morning[1];
      else if(main == "Drizzle") weatherIcon.sprite = Morning[2];
      else if(main == "Rain") weatherIcon.sprite = Morning[3];
      else if(main == "Thunderstorm") weatherIcon.sprite = Morning[4];
      else if(main == "Snow") weatherIcon.sprite = Morning[5];
      else Debug.Log("Weather State not found");
    }

    else if(skystate == 1) //Noon
    {
      if(main == "Clear") weatherIcon.sprite = Noon[0];
      else if(main == "Clouds") weatherIcon.sprite = Noon[1];
      else if(main == "Drizzle") weatherIcon.sprite = Noon[2];
      else if(main == "Rain") weatherIcon.sprite = Noon[3];
      else if(main == "Thunderstorm") weatherIcon.sprite = Noon[4];
      else if(main == "Snow") weatherIcon.sprite = Noon[5];
      else Debug.Log("Weather State not found");
    }
    else if(skystate == 2) //Night
    {
      if(main == "Clear") weatherIcon.sprite = Night[0];
      else if(main == "Clouds") weatherIcon.sprite = Night[1];
      else if(main == "Drizzle") weatherIcon.sprite = Night[2];
      else if(main == "Rain") weatherIcon.sprite = Night[3];
      else if(main == "Thunderstorm") weatherIcon.sprite = Night[4];
      else if(main == "Snow") weatherIcon.sprite = Night[5];
      else Debug.Log("Weather State not found");
    }
    else Debug.Log("Sky state not found");
  }

  // refresh button call
  // void UpdateWeather() => weatherAPI.GetRealTimeWeather(ref temperature, ref windspeed, ref description ,ref pressure);

}