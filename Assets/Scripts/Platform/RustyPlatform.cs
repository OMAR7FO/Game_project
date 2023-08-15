using System.Collections;
using UnityEngine;

public class RustyPlatform : MonoBehaviour
{
    //initial duration before the platform dissapear
    public float initialDuration = 2f;
    //initial duration to reset the time after the platform dissapear
    public float resetDuration = 5f;

    public float shakeMagnitude = 0.1f;
    private bool isActive = false;
    private bool isDissapear = false;
    private float timer;
    private float showTimer;
    private Vector2 originalPosition;
    #region Component
    private BoxCollider2D boxCollider;
    private SpriteRenderer SR;
    #endregion
    void Start()
    {
        timer = initialDuration;
        showTimer = resetDuration;
        originalPosition = transform.position;
        boxCollider = GetComponent<BoxCollider2D>();
        SR = GetComponent<SpriteRenderer>();
    }

    
    void Update()
    {
        if (isActive) {
            StartCoroutine(ShakeCube());
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                DropPlatform();
            }
        }
        else if (isDissapear)
        {
            StopCoroutine(ShakeCube());
            showTimer -= Time.deltaTime;
            if (showTimer <= 0f)
            {
                ShowPlatform();
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!isActive)
            {
                isActive = true;
            }
        }
    }

    private void ShowPlatform()
    {
        boxCollider.enabled = true;
        SR.enabled = true;
        showTimer = resetDuration;
        isDissapear = false;
    }

    private void DropPlatform()
    {
        boxCollider.enabled = false;
        SR.enabled = false;
        isDissapear = true;
        isActive = false;
        timer = initialDuration;
    }
    private IEnumerator ShakeCube()
    {
        float elapsedTime = initialDuration;
        while (elapsedTime >= 0)
        {
            float offsetX = Random.Range(-shakeMagnitude, shakeMagnitude);
            float offsetY = Random.Range(-shakeMagnitude, shakeMagnitude);
            transform.position = originalPosition + new Vector2(offsetX, offsetY);
            elapsedTime -= Time.deltaTime;
            yield return null;
        }
        transform.position = originalPosition;
        print("hello ");
    }
}
