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
using System.Collections;
using UnityEngine;

public class Boss : NetworkedBehaviour
{
    private Vector3 BossVector;

    // Start is called before the first frame update
    void Start()
    {
    }

    public override void NetworkStart()
    {
        // This is called when the object is spawned. Once this gets invoked. The object is ready for RPC and var changes.
        if (IsServer)
        {
            // Server Moves object. Client just renders object's transform.
            PickPosition();
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnDestroy()
    {
        ClientModule.Singleton.PlayerStatus = ClientModule.PlayStatus.WIN;      //Boss Destroy => Player Win
        ServerModule.Singleton.CloseServer();
    }

    void OnTriggerEnter(Collider other)
    {
        if (IsServer)
        {
            GameObject target = other.gameObject;

            if (transform.localScale.magnitude >= target.transform.localScale.magnitude)
            {
                //Boss Destroy Everything!
                Destroy(target);
            }
            else
            {
                //Boss Defeated
                Destroy(gameObject);
            }
        }
    }

    private void PickPosition()
    {
        if (IsServer)
        {
            float z = transform.position.z;
            float x = Random.Range
                (Camera.main.ScreenToWorldPoint(new Vector3(0, 0, z)).x, Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, z)).x);
            float y = Random.Range
                (Camera.main.ScreenToWorldPoint(new Vector3(0, 0, z)).y, Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, z)).y);

            BossVector = new Vector3(x, y, z);
            StartCoroutine(Wandering());
        }
    }

    IEnumerator Wandering()
    {
        float i = 0.0f;
        float rate = 1.0f / 2.0f;
        Vector3 currentPos = transform.position;

        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            transform.position = Vector3.Lerp(currentPos, BossVector, i);
            yield return null;
        }

        float randomFloat = Random.Range(0.0f, 1.0f); // Create %50 chance to wait
        if (randomFloat < 0.5f)
            StartCoroutine(WaitForSomeTime());
        else
            PickPosition();
    }

    IEnumerator WaitForSomeTime()
    {
        yield return new WaitForSeconds(5.0f);
        PickPosition();
    }
}
