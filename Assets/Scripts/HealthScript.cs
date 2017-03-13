using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class HealthScript : NetworkBehaviour
{
    [SyncVar]
    public float Health;

	// Use this for initialization
	void Start () {
        Health = 100;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void TakeDemage(float amount)
    {
        Health -= amount;
        if (Health <= 0)
        {
            Health = 0;
            Destroy(gameObject);
        }
    }
}
