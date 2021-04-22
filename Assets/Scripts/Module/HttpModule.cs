using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class HttpModule
{
    public struct HttpModel
    {
        public string statusCode;
        public string body;
    }

    public static IEnumerator PutRequest(string url, object body, Action<string> callback)
    {
        var json = JsonUtility.ToJson(body);
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);

        using (UnityWebRequest webRequest = UnityWebRequest.Put(url, bytes))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                LogModule.WriteToLogFile("[HttpModule] " + webRequest.error);
            }
            else
            {
                LogModule.WriteToLogFile("[HttpModule] " + webRequest.downloadHandler.text);
                callback(webRequest.downloadHandler.text);
            }
        }
    }
}