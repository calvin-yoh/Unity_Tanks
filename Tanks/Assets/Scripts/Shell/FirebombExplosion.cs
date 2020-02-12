using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebombExplosion : MonoBehaviour
{
    public LayerMask groundMask;
    public ParticleSystem explosionParticles = null;
    public AudioSource explosionAudio = null;
    public float maxLifeTime = 2f;
    public Transform lavafield = null;

    private void Start()
    {
        Destroy(gameObject, maxLifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<Rigidbody>())
        {
            Instantiate(lavafield, transform.position, Quaternion.identity);
            explosionParticles.transform.parent = null;

            explosionParticles.Play();

            explosionAudio.Play();

            Destroy(explosionParticles.gameObject, explosionParticles.duration);
            Destroy(gameObject);
        }
    }
}
