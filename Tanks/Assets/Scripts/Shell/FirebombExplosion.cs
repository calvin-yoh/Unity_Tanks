using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebombExplosion : MonoBehaviour
{
    public LayerMask m_GroundMask;
    public ParticleSystem m_ExplosionParticles;
    public AudioSource m_ExplosionAudio;
    public float m_MaxLifeTime = 2f;
    public Transform m_Lavafield;
    //public FireTiles m_fireTiles;



    private void Start()
    {
        Destroy(gameObject, m_MaxLifeTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        // Find all the tanks in an area around the shell and damage them.

        if (!other.GetComponent<Rigidbody>())
        {

            Instantiate(m_Lavafield, transform.position, Quaternion.identity);
            m_ExplosionParticles.transform.parent = null;

            m_ExplosionParticles.Play();

            m_ExplosionAudio.Play();

            Destroy(m_ExplosionParticles.gameObject, m_ExplosionParticles.duration);
            Destroy(gameObject);
        }
    }

}
