using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaFieldScript : MonoBehaviour
{
    // Start is called before the first frame update
    public float m_LifeTime = 3f;
    public LayerMask m_TankMask;

    void Start()
    {
        Destroy(gameObject, 3f);
    }

    private void OnTriggerStay(Collider other)
    {
        // Find all the tanks in an area around the shell and damage them.
        
        Collider[] colliders = Physics.OverlapSphere(transform.position, 15f, m_TankMask);

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
