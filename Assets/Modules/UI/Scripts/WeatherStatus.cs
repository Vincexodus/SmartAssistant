// Conditions explained: https://openweathermap.org/weather-conditions

public class WeatherStatus
{
	public int weatherId;
	public string main;
	public string description;
	public float temperature; // in degree celsius
	public float pressure;
	public float windSpeed;
  public string weatherIcon;

	public float Celsius () 
  {
		return temperature -273.15f;
	}

	public float Fahrenheit () 
  {
		return Celsius() *1.8f +32f;
	}
}