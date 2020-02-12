using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour
{
    public float startingHealth = 100f;          
    public Slider slider = null;                        
    public Image fillImage = null;                      
    public Color fullHealthColor = Color.green;  
    public Color zeroHealthColor = Color.red;    
    public GameObject explosionPrefab = null;
    
    private AudioSource explosionAudio = null;          
    private ParticleSystem explosionParticles = null;   
    private float currentHealth = 0;  
    private bool dead = false;            


    private void Awake()
    {
        explosionParticles = Instantiate(explosionPrefab).GetComponent<ParticleSystem>();
        explosionAudio = explosionParticles.GetComponent<AudioSource>();

        explosionParticles.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        currentHealth = startingHealth;
        dead = false;

        SetHealthUI();
    }
   
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        SetHealthUI();

        if (currentHealth <= 0f && !dead)
        {
            OnDeath();
        }
    }

    public void RestoreHealth(float amount)
    {
        if ((currentHealth + amount) >= 100)
        {
            currentHealth = 100;
        }
        else
        {
            currentHealth += amount;
        }  
        SetHealthUI();   
    }

    private void SetHealthUI()
    {
        slider.value = currentHealth;

        fillImage.color = Color.Lerp(zeroHealthColor, fullHealthColor, currentHealth / startingHealth);
    }

    private void OnDeath()
    {
        dead = true;

        explosionParticles.transform.position = transform.position;

        explosionParticles.gameObject.SetActive(true);

        explosionParticles.Play();

        explosionAudio.Play();

        gameObject.SetActive(false);
    }

    public float GetHealth()
    {
        return currentHealth;      
    }
}