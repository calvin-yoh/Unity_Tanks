using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{

    public Transform h_spawnPointOne;
    public Transform h_spawnPointTwo;
    public Transform h_spawnPointThree;
    public Transform h_spawnPointFour;

    public float HealthRegened = -30f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // Find all the tanks in an area around the shell and damage them.

        Rigidbody targetRigidbody = other.GetComponent<Rigidbody>();

        if (!targetRigidbody)
        {
            return;
        }

        TankHealth targetHealth = targetRigidbody.GetComponent <TankHealth>();

        if (targetHealth.getHealth() >= 70)
        {
            targetHealth.TakeDamage(targetHealth.getHealth() - 100);
        }
        else
        {
            targetHealth.TakeDamage(HealthRegened);
        }
        Destroy(gameObject);

    }
}
