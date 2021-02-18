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
using MLAPI.Messaging;
using MLAPI.NetworkedVar;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : NetworkedBehaviour
{
    private float Speed = 5.0f;
    public string Food = "Food";
    public string Enemy = "Player";
    public string Boss = "Boss";
    public float Increase = 0.1f;

    [SyncedVar]
    private int Score = 0;

    // Allows you to change the channel position updates are sent on. Its prefered to be UnreliableSequenced for fast paced.
    public string Channel = "MLAPI_DEFAULT_MESSAGE";

    public override void NetworkStart()
    {
        // This is called when the object is spawned. Once this gets invoked. The object is ready for RPC and var changes.
        ServerInit();
    }

    private void Update()
    {
        if (IsClient)
        {
            PlayerControl();
            PlayerCommand();
        }
    }

    private void OnGUI()
    {
        string text = "<b><i>Player " + NetworkId + "(" + Score + ")</i></b>";
        var guiPos = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        var textSiz = GUI.skin.label.CalcSize(new GUIContent(text));
        var rect = new Rect(0, 0, textSiz.x, textSiz.y);
        rect.x = guiPos.x - rect.width/2;
        rect.y = Screen.height - guiPos.y - rect.height/2;
        GUI.contentColor = Color.black;
        GUI.Label(rect, text);
    }

    private void ServerInit()
    {
        ClientModule.Singleton.PlayerStatus = ClientModule.PlayStatus.PLAY;
    }

    private void OnTriggerEnter(Collider other)
    {
        InvokeServerRpc(DoEatServerRpc, other.gameObject);
    }

    private void OnDestroy()
    {
        ClientModule.Singleton.PlayerStatus = ClientModule.PlayStatus.LOSE;
    }

    private void PlayerControl()
    {
        float keyHorizontal = Input.GetAxis("Horizontal");
        float keyVertical = Input.GetAxis("Vertical");
        Vector3 moveVector =
            new Vector3(keyHorizontal * Speed * Time.deltaTime / transform.localScale.x, keyVertical * Speed * Time.deltaTime / transform.localScale.y, 0);
        transform.Translate(moveVector);

        ////Mouse Move
        //Vector3 Target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Target.z = 0;
        //transform.position = Vector3.MoveTowards(transform.position, Target, Speed * Time.deltaTime / transform.localScale.x);
    }

    private void PlayerCommand()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LogModule.WriteToLogFile("Space Enter");
            string activeSceneName = SceneManager.GetActiveScene().name;
            ClientModule.Singleton.PlayerStatus = ClientModule.PlayStatus.MOVE;
            if (activeSceneName == "Map001")
            {
                ClientModule.Singleton.MoveToWorld("Map002");
            }
            else if (activeSceneName == "Map002")
            {
                ClientModule.Singleton.MoveToWorld("Map001");
            }
        }
    }

    [ServerRPC(RequireOwnership = true)]
    private void DoEatServerRpc(GameObject target)
    {
        // ServerRPC : Invoke by Client, Run on Server.
        if (transform.localScale.magnitude >= target.transform.localScale.magnitude)
        {
            Vector3 newSiz = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
            if (target.tag == Food)
            {
                int multiply = transform.localScale.x < 5.0 ? 1 : 0;
                newSiz += new Vector3(Increase * multiply, Increase * multiply, Increase * multiply);
                Score += 10;
            }
            else if (target.tag == Enemy || target.tag == Boss)
            {
                int multiply = transform.localScale.x < 10.0 ? 10 : 0;
                newSiz += new Vector3(Increase * multiply, Increase * multiply, Increase * multiply);
                Score += 100;
            }
            Destroy(target);
            transform.localScale = newSiz;
            InvokeClientRpcOnEveryone(DoEatClientRpc, target, newSiz);
        }
    }

    [ClientRPC]
    public void DoEatClientRpc(GameObject target, Vector3 size)
    {
        // This code gets ran on the clients at the request of the server.
        // ClientRPC : Invoke by server, Run on Client.
        Destroy(target);
        transform.localScale = size;
    }
}
