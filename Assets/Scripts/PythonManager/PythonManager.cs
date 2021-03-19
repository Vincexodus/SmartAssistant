using UnityEngine;
using System.Diagnostics;
using System.IO;
using System.Threading;

public class PythonManager : MonoBehaviour
{
  public string pythonBinPath;
  public string pythonScriptPath;
  // Start is called before the first frame update
  void Start()
  {
    Thread task = new Thread(new ParameterizedThreadStart(ExecuteBinary));
    task.Start(pythonScriptPath);

    // Process.Start("wordpad");
  }

  // Update is called once per frame
  void Update()
  {}

  public void ExecuteBinary(object cmd)
  {
    string command = (string)cmd;
    ProcessStartInfo start = new ProcessStartInfo();
    start.FileName = pythonBinPath;
    start.Arguments = string.Format($"\"{cmd}\"");
    start.UseShellExecute = false;
    start.CreateNoWindow = false;
    start.RedirectStandardOutput = true;
    start.RedirectStandardError = true;
    using (Process process = Process.Start(start))
    {
      StreamReader reader = process.StandardOutput;
      string stderr = process.StandardError.ReadToEnd(); // Here are the exceptions from our Python script
      string result = reader.ReadToEnd(); // Here is the result of StdOut(for example: print "test")
      print(result);
      process.WaitForExit();
    }
  }
}
