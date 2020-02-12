using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
    public int playerNumber = 1;       
    public Rigidbody shellOne = null;
    public Rigidbody shellTwo = null;
    public Rigidbody shellThree = null;
    public Transform fireTransform = null;
    public Transform mineTransform = null;
    public Slider aimSlider = null;           
    public AudioSource shootingAudio = null;  
    public AudioClip chargingClip = null;     
    public AudioClip fireClip = null;
   
    public float minLaunchForce = 15f; 
    public float maxLaunchForce = 30f; 
    public float maxChargeTime = 0.75f;

    private string fireButton;         
    private float currentLaunchForce;  
    private float chargeSpeed;         
    private bool fired;
    private int shellToFire = 1;

    private void OnEnable()
    {
        currentLaunchForce = minLaunchForce;
        aimSlider.value = minLaunchForce;
    }

    private void Start()
    {
        fireButton = "Fire" + playerNumber;

        chargeSpeed = (maxLaunchForce - minLaunchForce) / maxChargeTime;
    }
    
    private void Update()
    {
        // Track the current state of the fire button and make decisions based on the current launch force.
        aimSlider.value = minLaunchForce;

        if (currentLaunchForce >= maxLaunchForce && !fired)
        {
            // at max charge, not yet fired
            currentLaunchForce = maxLaunchForce;
            Fire();
        }
        else if (Input.GetButtonDown(fireButton))
        {
            // have we pressed fire for the first time?
            fired = false;
            currentLaunchForce = minLaunchForce;

            shootingAudio.clip = chargingClip;
            shootingAudio.Play();
        }
        else if (Input.GetButton(fireButton) && !fired)
        {
            // Holding the fire button, having not yet fired
            currentLaunchForce += chargeSpeed * Time.deltaTime;

            aimSlider.value = currentLaunchForce;
        }
        else if (Input.GetButtonUp(fireButton) && !fired)
        {
            //we released the button, having not fired yet
            Fire();
        }
    }

    private void Fire()
    {
        fired = true;

        if (shellToFire == 1)
        {
            Rigidbody shellInstance = Instantiate(shellOne, fireTransform.position, fireTransform.rotation);
            shellInstance.velocity = currentLaunchForce * fireTransform.forward;

            shootingAudio.clip = fireClip;
            shootingAudio.Play();

            currentLaunchForce = minLaunchForce;
            shellToFire++;
        }
        else if (shellToFire == 2)
        {
            Rigidbody shellInstance = Instantiate(shellTwo, fireTransform.position, fireTransform.rotation);
            shellInstance.velocity = currentLaunchForce * fireTransform.forward;

            shootingAudio.clip = fireClip;
            shootingAudio.Play();

            currentLaunchForce = minLaunchForce;
            shellToFire++;
        }
        else if (shellToFire == 3)
        {
            Rigidbody shellInstance = Instantiate(shellThree, mineTransform.position, mineTransform.rotation);

            shootingAudio.clip = fireClip;
            shootingAudio.Play();

            currentLaunchForce = minLaunchForce;

            shellToFire = 1; ;
        }
    }
}