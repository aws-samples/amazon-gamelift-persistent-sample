using MLAPI;
using System.Collections;
using UnityEngine;

public class Boss : NetworkBehaviour
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
