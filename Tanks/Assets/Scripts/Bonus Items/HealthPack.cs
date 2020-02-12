using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    public Transform spawnPointOne = null;
    public Transform spawnPointTwo = null;
    public Transform spawnPointThree = null;
    public Transform spawnPointFour = null;
    public const float HealthRegened = 30f;

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody targetRigidbody = other.GetComponent<Rigidbody>();

        if (!targetRigidbody)
        {
            return;
        }

        TankHealth targetHealth = targetRigidbody.GetComponent <TankHealth>();
        targetHealth.RestoreHealth(HealthRegened);
        Destroy(gameObject);
    }
}
