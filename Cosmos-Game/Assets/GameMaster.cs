using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster gm;

    void Start()
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
    public AudioClip respawnAudio;

    public IEnumerator RespawnPlayer()
    {
        AudioSource.PlayClipAtPoint(respawnAudio, new Vector3(spawnPoint.position.x, spawnPoint.position.y, spawnPoint.position.z), 0.5f);
        //GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(spawnDelay);

        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        Transform clone = Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation) ;
        Destroy(clone.gameObject, 3f);
    }

    public static void KillPlayer (Player player)
    {
        Destroy(player.gameObject);
        gm.StartCoroutine(gm.RespawnPlayer());
    }
}
/*public class GameMaster : MonoBehaviour
{
    public static GameMaster gm;                // GameMaster.cs

    public int spawnDelay = 3;                  // the player respawn delay
    public Transform playerPrefab;              // the player
    public Transform spawnPoint;                // the spawn point
    public GameObject spawnParticlePrefab;

    public AudioSource respawnAudio;

    void Start()
    {
        if (gm == null)
        {
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        }

        respawnAudio = GetComponent<AudioSource>();
    }

    // Wait 2 seconds and respawn the player at the spawn point position
    public IEnumerator RespawnPlayer()
    {
        respawnAudio.Play();
        yield return new WaitForSeconds(spawnDelay);
    }
}*/