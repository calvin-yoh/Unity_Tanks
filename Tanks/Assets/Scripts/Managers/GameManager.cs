using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private int roundNumber = 0;
    private int healthPackSpawnPoint = 0;
    private WaitForSeconds startWait = null;     
    private WaitForSeconds endWait = null;       
    private TankManager roundWinner = null;
    private TankManager gameWinner = null;
    private GameObject[] weapons = null;
    private GameObject[] healthPacks = null;
    private System.Random rnd = new System.Random();
    private GameObject shellInstance = null;
    private TankShooting shootingScript = null;

    [SerializeField] private const int numRoundsToWin = 5;
    [SerializeField] private float startTimer = 3f;
    [SerializeField] private float endTimer = 3f;
    [SerializeField] private float baseTimer = 3f;
    [SerializeField] private float gameWinTimer = 3f;
    [SerializeField] private GameObject tankScriptHolder = null;

    [SerializeField] private CameraControl cameraControl = null;
    [SerializeField] private Text messageText = null;
    [SerializeField] private GameObject tankPrefab = null;
    [SerializeField] private GameObject healthPackPrefab = null;
    [SerializeField] private HealthPack healthPackScript = null;
    [SerializeField] private TankManager[] tanks = null;

    private void Start()
    {
        shootingScript = tankScriptHolder.GetComponent<TankShooting>();
        SpawnAllTanks();
        SetCameraTargets();
    }

    private void Update()
    {
        if (startTimer == baseTimer)
        {
            RoundStarting();
            startTimer -= Time.deltaTime;
        }
        else if (startTimer >= 0)
        {
            startTimer -= Time.deltaTime;
        }          
        else if (OneTankLeft())
        {
            if (endTimer == baseTimer)
            {
                RoundEnding();
                endTimer -= Time.deltaTime;
            }
            else if (endTimer >= 0)
            {
                endTimer -= Time.deltaTime;
            }
            else if (endTimer < 0)
            {
                startTimer = baseTimer;
                endTimer = baseTimer;
            }
        }
        else 
        {
            RoundPlaying();        
        }

        if (gameWinner != null && gameWinTimer < 0)
        {
            SceneManager.LoadScene(0);
        }
        else if(gameWinner != null)
        {
            gameWinTimer -= Time.deltaTime;
        }
    }

    private void SpawnAllTanks()
    {
        for (int i = 0; i < tanks.Length; i++)
        {
            tanks[i].instance =
                Instantiate(tankPrefab, tanks[i].spawnPoint.position, tanks[i].spawnPoint.rotation) as GameObject;
            tanks[i].playerNumber = i + 1;
            tanks[i].Setup();
        }
    }

    private void SetCameraTargets()
    {
        Transform[] targets = new Transform[tanks.Length];

        for (int i = 0; i < targets.Length; i++)
        {
            targets[i] = tanks[i].instance.transform;
        }
        cameraControl.targets = targets;
    }

    private void RoundStarting()
    {
        ResetAllTanks();
        DisableTankControl();

        cameraControl.SetStartPositionAndSize();

        roundNumber++;
        messageText.text = "Round " + roundNumber;
    }

    private void RoundPlaying()
    {
        EnableTankControl();

        messageText.text = string.Empty;
      
        if(!OneTankLeft())
        {
            if(shellInstance == null || !shellInstance.activeSelf)
            {
                SpawnHealthPack();
            }
        }
    }

    private void RoundEnding()
    {
        DisableTankControl();

        //removes all weapons types still on field; Mines, LavaFields
        foreach (KeyValuePair<float, GameObject> kvp in shootingScript.weaponsDict)
        {
            Debug.Log(kvp.Key);
            Debug.Log(kvp.Value);
            Destroy(kvp.Value);
        }
        shootingScript.ResetWeaponNumber();
        shootingScript.ResetWeaponDictionary();

        roundWinner = null;

        roundWinner = GetRoundWinner();

        if (roundWinner != null)
        {
            roundWinner.wins++;
        }

        gameWinner = GetGameWinner();

        string message = EndMessage();
        messageText.text = message;

        weapons = null;
    }

    private bool OneTankLeft()
    {
        int numTanksLeft = 0;

        for (int i = 0; i < tanks.Length; i++)
        {
            if (tanks[i].instance.activeSelf)
                numTanksLeft++;
        }

        return numTanksLeft <= 1;
    }

    private TankManager GetRoundWinner()
    {
        for (int i = 0; i < tanks.Length; i++)
        {
            if (tanks[i].instance.activeSelf)
                return tanks[i];
        }
        return null;
    }

    private TankManager GetGameWinner()
    {
        for (int i = 0; i < tanks.Length; i++)
        {
            if (tanks[i].wins == numRoundsToWin)
                return tanks[i];
        }
        return null;
    }

    private string EndMessage()
    {
        string message = "DRAW!";

        if (roundWinner != null)
            message = roundWinner.coloredPlayerText + " WINS THE ROUND!";

        message += "\n\n\n\n";

        for (int i = 0; i < tanks.Length; i++)
        {
            message += tanks[i].coloredPlayerText + ": " + tanks[i].wins + " WINS\n";
        }

        if (gameWinner != null)
            message = gameWinner.coloredPlayerText + " WINS THE GAME!";

        return message;
    }

    private void ResetAllTanks()
    {
        for (int i = 0; i < tanks.Length; i++)
        {
            tanks[i].Reset();
        }
    }

    private void EnableTankControl()
    {
        for (int i = 0; i < tanks.Length; i++)
        {
            tanks[i].EnableControl();
        }
    }

    private void DisableTankControl()
    {
        for (int i = 0; i < tanks.Length; i++)
        {
            tanks[i].DisableControl();
        }
    }

    private void SpawnHealthPack()
    {
        healthPackSpawnPoint = rnd.Next(1, 5);
        switch (healthPackSpawnPoint)
        {
            case 1:
                shellInstance = Instantiate(healthPackPrefab, healthPackScript.spawnPointOne.position, healthPackScript.spawnPointOne.rotation);
                break;
            case 2:
                shellInstance = Instantiate(healthPackPrefab, healthPackScript.spawnPointTwo.position, healthPackScript.spawnPointTwo.rotation);
                break;
            case 3:
                shellInstance = Instantiate(healthPackPrefab, healthPackScript.spawnPointThree.position, healthPackScript.spawnPointThree.rotation);
                break;
            case 4:
                shellInstance= Instantiate(healthPackPrefab, healthPackScript.spawnPointFour.position, healthPackScript.spawnPointFour.rotation);
                break;
        }
    }
}