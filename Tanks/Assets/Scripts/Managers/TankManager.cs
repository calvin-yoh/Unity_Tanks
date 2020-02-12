using System;
using UnityEngine;

[Serializable]
public class TankManager
{
    public Color playerColor;            
    public Transform spawnPoint = null;         
    [HideInInspector] public int playerNumber = 0;             
    [HideInInspector] public string coloredPlayerText = null;
    [HideInInspector] public GameObject instance = null;          
    [HideInInspector] public int wins = 0;                     

    private TankMovement movement = null;       
    private TankShooting shooting = null;
    private GameObject canvasGameObject = null;

    public void Setup()
    {
        movement = instance.GetComponent<TankMovement>();
        shooting = instance.GetComponent<TankShooting>();
        canvasGameObject = instance.GetComponentInChildren<Canvas>().gameObject;

        movement.playerNumber = playerNumber;
        shooting.playerNumber = playerNumber;

        coloredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(playerColor) + ">PLAYER " + playerNumber + "</color>";

        MeshRenderer[] renderers = instance.GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = playerColor;
        }
    }

    public void DisableControl()
    {
        movement.enabled = false;
        shooting.enabled = false;

        canvasGameObject.SetActive(false);
    }

    public void EnableControl()
    {
        movement.enabled = true;
        shooting.enabled = true;

        canvasGameObject.SetActive(true);
    }

    public void Reset()
    {
        instance.transform.position = spawnPoint.position;
        instance.transform.rotation = spawnPoint.rotation;

        instance.SetActive(false);
        instance.SetActive(true);
    }
}
