using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIconfig : MonoBehaviour
{
  public TextMeshProUGUI[] texts;
  public Camera bgCamera;
  Color currentUIColor;
  Color bgColor;
  //red0, orange1, yellow2, lime3, green4, cyan5, blue6, purple7, pink8, white9
  public Image[] image;
  public Color[] colors;
  void Start()
  {
    currentUIColor = Color.cyan;
    for(int i = 0; i < image.Length; i++)
    {
      image[i].color = currentUIColor;
    }
  }

  #region currentUIColorFunctions
  public void updatecurrentUIColor()
  {
    for(int i = 0; i < image.Length; i++)
    {
      image[i].color = currentUIColor;
    }

    for(int i = 0; i < texts.Length; i++)
    {
      texts[i].faceColor = currentUIColor;
    }
  }
  public void red()
  {
    currentUIColor = colors[0];
  }
  public void orange()
  {
    currentUIColor = colors[1];
  }
  public void yellow()
  {
    currentUIColor = colors[2];
  }
  public void lime()
  {
    currentUIColor = colors[3];
  }
  public void green()
  {
    currentUIColor = colors[4];
  }
  public void cyan()
  {
    currentUIColor = colors[5];
  }
  public void blue()
  {
    currentUIColor = colors[6];
  }
  public void purple()
  {
    currentUIColor = colors[7];
  }
  public void pink()
  {
    currentUIColor = colors[8];
  }
  public void white()
  {
    currentUIColor = colors[9];
  }
  #endregion

  #region BgColorFunctions
  public void defaultBgColor()
  {
    bgCamera.backgroundColor = Color.black;
  }
  public void updateBgColor()
  {
    bgCamera.backgroundColor = bgColor;
  }
  public void bgRed()
  {
    bgColor = colors[0];
  }
  public void bgOrange()
  {
    bgColor = colors[1];
  }
  public void bgYellow()
  {
    bgColor = colors[2];
  }
  public void bgLime()
  {
    bgColor = colors[3];
  }
  public void bgGreen()
  {
    bgColor = colors[4];
  }
  public void bgCyan()
  {
    bgColor = colors[5];
  }
  public void bgBlue()
  {
    bgColor = colors[6];
  }
  public void bgPurple()
  {
    bgColor = colors[7];
  }
  public void bgPink()
  {
    bgColor = colors[8];
  }
  public void bgWhite()
  {
    bgColor = colors[9];
  }
  #endregion
}
