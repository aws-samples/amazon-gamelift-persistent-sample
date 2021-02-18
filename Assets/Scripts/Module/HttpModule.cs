/*
 * Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
 * SPDX-License-Identifier: MIT-0
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this
 * software and associated documentation files (the "Software"), to deal in the Software
 * without restriction, including without limitation the rights to use, copy, modify,
 * merge, publish, distribute, sublicense, and/or sell copies of the Software, and to
 * permit persons to whom the Software is furnished to do so.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
 * PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
 * OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
 * SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */
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