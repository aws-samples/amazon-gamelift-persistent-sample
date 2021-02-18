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
using MLAPI;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MobileModule : MonoBehaviour
{
    public GameObject FoodPrefab;

    // Start is called before the first frame update
    void Start()
    {
        NetworkingManager.Singleton.OnServerStarted += OnServerStarted;
        NetworkingManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkingManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 150, 50), "Host"))
        {
            Debug.Log("On Click Button");
            NetworkingManager.Singleton.StartHost();
        }
        if (GUI.Button(new Rect(10, 80, 150, 50), "End"))
        {
            Debug.Log("On Click Disconnect");
            NetworkingManager.Singleton.StopHost();
        }
    }

    private void OnServerStarted()
    {
        //This runs at Server side
        Debug.Log("[ServerModule] On Server Started");
        StartCoroutine(SpawnFood());
    }

    private void OnClientConnected(ulong clientId)
    {
        //Called when Client connected
        Debug.Log("[ServerModule] On Client Connected - " + clientId);
    }

    private void OnClientDisconnect(ulong clientId)
    {
        //Called when Client disconnected
        StopCoroutine(SpawnFood());
    }

    private IEnumerator SpawnFood()
    {
        for (int i = 0; i < 25; i++)
        {
            if (NetworkingManager.Singleton.IsHost)
            {
                int x = UnityEngine.Random.Range(0, Camera.main.pixelWidth);
                int y = UnityEngine.Random.Range(0, Camera.main.pixelHeight);

                Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(x, y, 0));
                pos.z = 0;

                //Implement Custom Prefab Spawning
                GameObject foodObj = Instantiate(FoodPrefab, pos, Quaternion.identity);
                //Spawn Food Management
                foodObj.GetComponent<NetworkedObject>().Spawn();
                yield return new WaitForSeconds(4.0f);
            }
        }
    }
}
