using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Widget : MonoBehaviour
{
  public GameObject note;
  public Transform widget;
  public Transform widgetPoint;
  public void CreateNote()
  {
    GameObject DupiNote = Instantiate(note, widgetPoint.position, Quaternion.identity);
    DupiNote.transform.SetParent(widget);
    widgetPoint.position += new Vector3(0, 0.5f, 0);
  }
  public void CreateAlarm()
  {
    // pass
  }
  

}
