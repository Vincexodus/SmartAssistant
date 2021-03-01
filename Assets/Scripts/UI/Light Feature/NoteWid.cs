using UnityEngine;

public class NoteWid : MonoBehaviour
{
  public GameObject notePad;
  bool move = false;
  bool scale = false;
  KeyCode set = KeyCode.Mouse0;
  Vector3 screenPos;
  Vector3 worldPos;
  Vector3 mouseStart;
  Vector3 mouseEnd;

  public void DeleteNote()
  {
    Destroy(gameObject);
  }
  
  public void Repostion()
  {
    move = true;
    //move with mouse position
    
  }
  void Start()
  {
    move = false;
    scale = false;
  }
  void Update()
  {
    screenPos = Camera.main.WorldToScreenPoint(transform.position); //object pos to screen
    worldPos =  Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPos.z)); //real mouse position 
    if(move)
    {
      transform.position = worldPos;
    }
    if(Input.GetKeyDown(set))
    {
      move = false;
    }
    if(Input.GetKeyDown(set) && scale)
    {
      mouseStart = worldPos;
    }
    if(Input.GetKeyUp(set) &&scale)
    {
      mouseEnd = worldPos;
      float Xratio = Mathf.Abs(mouseEnd.x) - Mathf.Abs(mouseStart.x);
      float Yratio = Mathf.Abs(mouseEnd.y) - Mathf.Abs(mouseStart.y);
      notePad.transform.localScale += new Vector3(Xratio/100, Yratio/100, 0);
    }
  }
  public void Resize()
  {
    scale = true;
    //get offwset of mouse to object
    //scale
  }
  
  public void Save()
  {
    scale = false;
    move = false;
    //disable functions
    // false booleans
  }
}
