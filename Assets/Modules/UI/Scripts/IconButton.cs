using UnityEngine;
public class IconButton : MonoBehaviour
{
  //switch scenes on button trigger
  //button colour change when selected
  
  public GameObject placeholder;
  
  void Start()
  {
    placeholder.SetActive(false);
  }

  public void onAndOff()
  {
    if(placeholder.activeSelf == false) placeholder.SetActive(true);
    else placeholder.SetActive(false);
  }
}
