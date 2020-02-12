using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    [SerializeField] private const int numRoundsToWin = 5;
    [SerializeField] private const float startDelay = 3f;
    [SerializeField] private const float endDelay = 3f;
    [SerializeField] private CameraControl cameraControl = null;
    [SerializeField] private Text messageText = null;
    [SerializeField] private GameObject tankPrefab = null;
    [SerializeField] private GameObject healthPackPrefab = null;
    [SerializeField] private HealthPack healthPackScript = null;
    [SerializeField] private TankManager[] tanks = null;

    private void Start()
    {
        startWait = new WaitForSeconds(startDelay);
        endWait = new WaitForSeconds(endDelay);

        SpawnAllTanks();
        SetCameraTargets();

        StartCoroutine(GameLoop());
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

    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());

        if (gameWinner != null)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            StartCoroutine(GameLoop());
        }
    }

    private IEnumerator RoundStarting()
    {
        ResetAllTanks();
        DisableTankControl();

        cameraControl.SetStartPositionAndSize();

        roundNumber++;
        messageText.text = "Round " + roundNumber;

        yield return startWait;
    }

    private IEnumerator RoundPlaying()
    {
        EnableTankControl();

        messageText.text = string.Empty;
      
        while (!OneTankLeft())
        {
            if(shellInstance == null || !shellInstance.activeSelf)
            {
                SpawnHealthPack();
            }
            yield return null;
        }
    }

    private IEnumerator RoundEnding()
    {
        DisableTankControl();

        //removes all weapons types still on field; Mines, LavaFields
        weapons = GameObject.FindGameObjectsWithTag("Weapons");
        foreach (GameObject weapon in weapons)
        {
            Destroy(weapon);
        }

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

        yield return endWait;
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