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
        NetworkManager.Singleton.OnServerStarted += OnServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;

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
            NetworkManager.Singleton.StartHost();
        }
        if (GUI.Button(new Rect(10, 80, 150, 50), "End"))
        {
            Debug.Log("On Click Disconnect");
            NetworkManager.Singleton.StopHost();
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
            if (NetworkManager.Singleton.IsHost)
            {
                int x = UnityEngine.Random.Range(0, Camera.main.pixelWidth);
                int y = UnityEngine.Random.Range(0, Camera.main.pixelHeight);

                Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(x, y, 0));
                pos.z = 0;

                //Implement Custom Prefab Spawning
                GameObject foodObj = Instantiate(FoodPrefab, pos, Quaternion.identity);
                //Spawn Food Management
                foodObj.GetComponent<NetworkObject>().Spawn();
                yield return new WaitForSeconds(4.0f);
            }
        }
    }
}
