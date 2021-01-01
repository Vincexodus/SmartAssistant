using UnityEngine;

public partial class BGCanvas : MonoBehaviour
{
  void InitWeather()
  {
    weatherAPI.GetRealTimeWeather(ref temperature, ref windspeed, ref description, ref pressure);
    weatherAPI.weather = new WeatherStatus();
  }

  // refresh button call
  void UpdateWeather() => weatherAPI.GetRealTimeWeather(ref temperature, ref windspeed, ref description ,ref pressure);

}