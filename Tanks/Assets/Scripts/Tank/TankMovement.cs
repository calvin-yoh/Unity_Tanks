﻿using UnityEngine;

public class TankMovement : MonoBehaviour
{
    public int playerNumber = 1;         
    public float speed = 12f;            
    public float turnSpeed = 180f;       
    public AudioSource movementAudio = null;    
    public AudioClip engineIdling = null;       
    public AudioClip engineDriving = null;      
    public float pitchRange = 0.2f;

    private string movementAxisName = null;     
    private string turnAxisName = null;         
    private Rigidbody rigidbody = null;         
    private float movementInputValue = 0;    
    private float turnInputValue = 0;        
    private float originalPitch = 0;         

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable ()
    {
        rigidbody.isKinematic = false;
        movementInputValue = 0f;
        turnInputValue = 0f;
    }

    private void OnDisable ()
    {
        rigidbody.isKinematic = true;
    }

    private void Start()
    {
        movementAxisName = "Vertical" + playerNumber;
        turnAxisName = "Horizontal" + playerNumber;

        originalPitch = movementAudio.pitch;
    }
    
    private void Update()
    {
        movementInputValue = Input.GetAxis(movementAxisName);
        turnInputValue = Input.GetAxis(turnAxisName);

        EngineAudio();
    }

    private void EngineAudio()
    {
        if (Mathf.Abs(movementInputValue) < 0.1f && Mathf.Abs(turnInputValue) < 0.1f)
        {
            //Tank is currently idle
            if (movementAudio.clip == engineDriving)
            {
                movementAudio.clip = engineIdling;
                movementAudio.pitch = Random.Range(originalPitch - pitchRange, originalPitch + pitchRange);
                movementAudio.Play();
            }
        }
        else
        {
            //Tank is currently moving
            if (movementAudio.clip == engineIdling)
            {
                movementAudio.clip = engineDriving;
                movementAudio.pitch = Random.Range(originalPitch - pitchRange, originalPitch + pitchRange);
                movementAudio.Play();
            }
        }
    }

    private void FixedUpdate()
    {
        // Move and turn the tank.
        Move();
        Turn();
    }

    private void Move()
    {

        Vector3 movement = transform.forward * movementInputValue * speed * Time.deltaTime;

        rigidbody.MovePosition(rigidbody.position + movement);
    }

    private void Turn()
    {
        float turn = turnInputValue * turnSpeed * Time.deltaTime;

        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

        rigidbody.MoveRotation(rigidbody.rotation * turnRotation);
    }
}