using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAlwaysMove : MonoBehaviour
{
    [SerializeField] Transform[] points;
    [SerializeField] float speed;
    private int i = 0;
    void Start()
    {
        transform.position = points[0].position;
        StartCoroutine(Move());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator Move()
    {
        while (true) {


            if (Vector2.Distance(transform.position, points[i].position) < 0.01f)
            {
                i = NewTarget(i);
            }
            transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
            yield return null;


        }
    }
    private int NewTarget (int currentTarget )
    {
        if (currentTarget + 1 < points.Length )
        {
            return currentTarget + 1;
        }
        else
        {
            return currentTarget - 1;
        }
    }
    private void OnDrawGizmos()
    {
        for (int i = 0; i < points.Length - 1; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(points[i].position, points[i + 1].position);
        }
    }
}
