using UnityEngine;

public class ShellExplosion : MonoBehaviour
{
    [SerializeField] private LayerMask tankMask;
    [SerializeField] private ParticleSystem explosionParticles;
    [SerializeField] private AudioSource explosionAudio;
    [SerializeField] private const float maxDamage = 100f;
    [SerializeField] private const float explosionForce = 1000f;
    [SerializeField] private const float maxLifeTime = 2f;
    [SerializeField] private const float explosionRadius = 5f;              

    private void Start()
    {
        Destroy(gameObject, maxLifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Find all the tanks in an area around the shell and damage them.
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, tankMask);

        for (int i = 0; i < colliders.Length; i++)
        {
            Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();
            if (targetRigidbody.tag == "TankPlayers")
            {
                targetRigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);

                TankHealth targetHealth = targetRigidbody.GetComponent<TankHealth>();

                float damage = CalculateDamage(targetRigidbody.position);

                targetHealth.TakeDamage(damage);

                explosionParticles.transform.parent = null;

                explosionParticles.Play();

                explosionAudio.Play();

                Destroy(explosionParticles.gameObject, explosionParticles.main.duration);
                Destroy(gameObject);
            }
        }
    }

    private float CalculateDamage(Vector3 targetPosition)
    {
        Vector3 explosionToTarget = targetPosition - transform.position;

        float explosionDistance = explosionToTarget.magnitude;

        float relativeDistance = (explosionRadius - explosionDistance) / explosionRadius;

        float damage = relativeDistance * maxDamage;

        damage = Mathf.Max(0f, damage);

        return damage;
    }
}