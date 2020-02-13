using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineScript : MonoBehaviour
{
    [SerializeField] private LayerMask tankMask;
    [SerializeField] private ParticleSystem explosionParticles;
    [SerializeField] private AudioSource explosionAudio;
    [SerializeField] private float explosionForce = 1000f;
    [SerializeField] private float maxLifeTime = 30f;
    [SerializeField] private float explosionRadius = 2f;
    [SerializeField] private GameObject tankScriptReference = null;

    private TankShooting shootingScript = null;

    private void Start()
    {
        shootingScript = tankScriptReference.GetComponent<TankShooting>();
        shootingScript.weaponsDict.Add(shootingScript.ReturnWeaponNumber(), gameObject);
        shootingScript.AddOneToWeaponNumber();
        Destroy(gameObject, maxLifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, tankMask);

        for (int i = 0; i < colliders.Length; i++)
        {
            Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();
            if (targetRigidbody.tag == "TankPlayers")
            {
                targetRigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);

                TankHealth targetHealth = targetRigidbody.GetComponent<TankHealth>();

                float damage = 30f;

                targetHealth.TakeDamage(damage);

                explosionParticles.transform.parent = null;

                explosionParticles.Play();

                explosionAudio.Play();

                Destroy(explosionParticles.gameObject, explosionParticles.main.duration);
                Destroy(gameObject);
            }
        }
    }
}
