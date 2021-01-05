using UnityEngine;
using UnityEngine.UI;
public class UIconfig : MonoBehaviour
{
  //switch scenes on button trigger
  //button colour change when selected
  
  public GameObject placeholder;
  
    // Start is called before the first frame update
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
