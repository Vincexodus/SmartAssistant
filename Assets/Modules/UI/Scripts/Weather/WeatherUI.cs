using TMPro;
using UnityEngine;

public partial class BGCanvas : MonoBehaviour
{
  void InitWeather()
  {
    weatherAPI.GetRealTimeWeather(ref temperature);
    weatherAPI.weather = new WeatherStatus();

    UpdateWeather();
  }

  void UpdateWeather() => weatherAPI.GetRealTimeWeather(ref temperature);

}