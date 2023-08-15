using System.Collections;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Transform[] points;
    [SerializeField] int startingPoint;
    [SerializeField] bool alwaysMove;
    bool hasReachedEnd = false;
    private bool canMove = false;
    private Rigidbody2D rb;
    private Animator anim;
    private int i;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = points[startingPoint].position;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator MovePlatform()
    {
        while (canMove)
        {
            if (Vector2.Distance(transform.position, points[i].position) < 0.01f)
            {
                i = NewTarget(i);
            }
            if (Vector2.Distance(transform.position, points[points.Length - 1].position) < 0.01f)
            {
                hasReachedEnd = true;
            }
            transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
            if (canMove && hasReachedEnd && Vector2.Distance(transform.position, points[startingPoint].position) < 0.01f)
            {
                canMove = false;
                anim.SetBool("canMove", canMove);
                hasReachedEnd = false;
            }
            yield return null;
        }
    }
    private int NewTarget(int currentTarget)
    {
        if (currentTarget + 1 < points.Length && !hasReachedEnd)
        {
            return currentTarget + 1;
        }
        else
        {
            return currentTarget - 1;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !canMove && !hasReachedEnd)
        {
            canMove = true;
            anim.SetBool("canMove", canMove);
            StartCoroutine(MovePlatform());
        }
        if (collision.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
    private void OnDrawGizmos()
    {
        for (int i=0;i<points.Length - 1; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(points[i].position, points[i + 1].position);
        }
    }

}
