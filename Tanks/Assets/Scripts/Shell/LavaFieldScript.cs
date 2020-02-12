using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaFieldScript : MonoBehaviour
{

    [SerializeField] private const float lifeTime = 3f;
    [SerializeField] private LayerMask tankMask;

    private void Start()
    {
        Destroy(gameObject, 3f);
    }

    private void OnTriggerStay(Collider other)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 15f, tankMask);

        for (int i = 0; i < colliders.Length; i++)
        {
            Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();

            if (!targetRigidbody)
                continue;

            TankHealth targetHealth = targetRigidbody.GetComponent<TankHealth>();

            if (!targetHealth)
                continue;

            float damage = 0.5f;

            targetHealth.TakeDamage(damage);
        }
    }
}
