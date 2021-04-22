
using UnityEngine;

public class LogModule
{

    public static void WriteToLogFile(string message)
    {
#if UNITY_EDITOR
        Debug.Log(message);
#else
        using (System.IO.StreamWriter logFile = new System.IO.StreamWriter("gamelift-log.txt", true))
        {
            logFile.WriteLine(message);
        }
#endif
    }
}
