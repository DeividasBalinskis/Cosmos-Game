using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster gm;

    [SerializeField]
    private int maxLives = 3;

    private static int _remainingLives;
    public static int RemainingLives
    {
        get { return _remainingLives; }
    }

    [SerializeField]
    private int startingMoney;
    public static int Money;

    void Awake()
    {
        if (gm == null)
        {
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        }
    }

    public Transform playerPrefab;
    public Transform spawnPoint;
    public float spawnDelay = 2;
    public Transform spawnPrefab;
    public string respawnCountdownSoundName = "RespawnCountdown";
    public string spawnSoundName = "Spawn";

    public string gameOverSoundName = "GameOver";

    public CameraShake cameraShake;

    [SerializeField]
    private GameObject gameOverUI;

    [SerializeField]
    private GameObject upgradeMenu;

    [SerializeField]
    private WaveSpawner waveSpawner;

    public delegate void UpgradeMenuCallback(bool active);
    public UpgradeMenuCallback onToggleUpgrdeMenu;

    //cache
    private AudioManager audioManager;

    void Start()
    {
        if(cameraShake == null)
        {
            Debug.LogError("no camera hkae reference ins game master");
        }

        _remainingLives = maxLives;

        Money = startingMoney;

        //caching
        audioManager = AudioManager.instance;
        if(audioManager == null)
        {
            Debug.LogError("No audioMaanager found in this scene");
        }
    }

    /*void Update()
    {
        if (Input.GetKeyDown(KeyCode.U)) 
        {
            ToggleUpgradeMenu();///////////////Maybe Pause Menu!!!!!!
        }
    }

    private void ToggleUpgradeMenu()
    {
        upgradeMenu.SetActive(!upgradeMenu.activeSelf);
        waveSpawner.enabled = !upgradeMenu.activeSelf;
        onToggleUpgrdeMenu.Invoke(upgradeMenu.activeSelf);
    }*/

    public void EndGame()
    {
        audioManager.PlaySound(gameOverSoundName);

        Debug.Log("game over");
        gameOverUI.SetActive(true);
    }

    public IEnumerator _RespawnPlayer()
    {
        audioManager.PlaySound(respawnCountdownSoundName);
        yield return new WaitForSeconds(spawnDelay);

        audioManager.PlaySound(spawnSoundName);
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        Transform clone = Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation) ;
        Destroy(clone.gameObject, 3f);
    }

    public static void KillPlayer (Player player)
    {
        Destroy(player.gameObject);
        _remainingLives -= 1;
        if(_remainingLives <= 0)
        {
            gm.EndGame();
        }
        else
        {
            gm.StartCoroutine(gm._RespawnPlayer());
        }
    }

    public static void KillEnemy(Enemy enemy)
    {
        gm._KillEnemy(enemy);
    }   

    public void _KillEnemy(Enemy _enemy)
    {

        //sound play
        audioManager.PlaySound(_enemy.deathSoundName);

        //Gain some money
        Money += _enemy.moneyDrop;
        audioManager.PlaySound("Money");

        //add particles
        GameObject _clone = Instantiate(_enemy.deathParticles.gameObject, _enemy.transform.position, Quaternion.identity);
        Destroy(_clone, 5f);

        //camera shake
        cameraShake.Shake(_enemy.shakeAmt, _enemy.shakeLenght);
        Destroy(_enemy.gameObject);
    }
}