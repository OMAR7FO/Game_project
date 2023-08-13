using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    [SerializeField] float timeToWaitAfterDeath = 2;
    [SerializeField] public GameObject deathParticles;
    public Transform respawnPoint;
    public GameObject playerPrefab;
    public CinemachineStateDrivenCamera cam;
    private void Awake()
    {
        int numOfLevelManager = FindObjectsOfType<LevelManager>().Length;
        if (numOfLevelManager > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ProcessPlayerDeath()
    {
        StartCoroutine(WaitAndDie());
    }
    IEnumerator WaitAndDie()
    {
        yield return new WaitForSeconds(timeToWaitAfterDeath);
        Respawn();
    }

    public void Respawn()
    {
        GameObject player = Instantiate(playerPrefab, respawnPoint.position, Quaternion.identity);
        cam.Follow = player.transform;
    }
}
