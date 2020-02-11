using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    [SerializeField] private const Transform spawnPointOne = null;;
    [SerializeField] private const Transform spawnPointTwo = null;
    [SerializeField] private const Transform spawnPointThree = null;
    [SerializeField] private const Transform spawnPointFour = null;
    [SerializeField] private const float HealthRegened = 30f;

    private void OnTriggerEnter(Collider other)
    {
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
