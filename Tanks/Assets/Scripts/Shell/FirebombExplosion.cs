using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebombExplosion : MonoBehaviour
{
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private ParticleSystem explosionParticles = null;
    [SerializeField] private AudioSource explosionAudio = null;
    [SerializeField] private const float maxLifeTime = 2f;
    [SerializeField] private GameObject lavafield = null;
    [SerializeField] private GameObject instance = null;

    private TankShooting shootingScript = null;

    private void Start()
    {
        shootingScript = instance.GetComponent<TankShooting>();
        Destroy(gameObject, maxLifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<Rigidbody>())
        {
            GameObject shellInstance = Instantiate(lavafield, transform.position, Quaternion.identity);
            
            shootingScript.weaponsDict.Add(shootingScript.ReturnWeaponNumber(), shellInstance);
            shootingScript.AddOneToWeaponNumber();

            explosionParticles.transform.parent = null;

            explosionParticles.Play();

            explosionAudio.Play();

            Destroy(explosionParticles.gameObject, explosionParticles.main.duration);
            Destroy(gameObject);
        }
    }
}
