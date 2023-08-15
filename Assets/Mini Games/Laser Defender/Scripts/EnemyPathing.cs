using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    WaveConfig waveConfig;
    List<Transform> wayPoints ;
    int waypointIndex = 0;
    private void Start()
    {
        wayPoints = waveConfig.GetWayPoints();
        transform.position = wayPoints[waypointIndex].transform.position;
    }
    private void Update()
    {
        Move(); 
    }

    private void Move()
    {
        if (waypointIndex < wayPoints.Count)
        {
            var targetPath = wayPoints[waypointIndex].transform.position;
            var movementThisFrame = waveConfig.getMoveSpeed() * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPath, movementThisFrame);
            if (transform.position == targetPath)
            {
                waypointIndex++;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SetWaveConfig (WaveConfig waveConfig)
    {
        this.waveConfig = waveConfig;
    }
}
