using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LaserDefenderPlayer : MonoBehaviour
{
    // config param
    [Header("Player")]
    [SerializeField] float speed = 10f;
    [SerializeField] float padding = 0.5f;
    [SerializeField] int health = 100;
    [SerializeField] AudioClip deathSFX;
    [SerializeField] [Range(0,1)] float volumeDeathSound = 0.7f;
    [SerializeField] AudioClip shootingSFX;
    [SerializeField][Range(0,1)] float volumeShootinSound = 0.25f;
    [Header("Projecttile")]
    [SerializeField] float projectTileSpeed = 10f;
    [SerializeField] float projecttileFirePeriod = 0.1f;
    [SerializeField] GameObject laserPrefab;

    Coroutine fireCoroutine;
    float minX;
    float maxX;
    float minY;
    float maxY;
    // Start is called before the first frame update
    void Start()
    {
        SetUpMoveBoundries();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer damageDealer = collision.GetComponent<DamageDealer>();
        proccessHit(damageDealer);
    }

    private void proccessHit (DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        FindObjectOfType<Level>().LoadGameOver();
        Destroy(gameObject);
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, volumeDeathSound);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            fireCoroutine = StartCoroutine(FireContinuously());
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(fireCoroutine);
        }
       
    }

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * speed;
        var newXPos = Math.Clamp(transform.position.x + deltaX,minX,maxX);
        var newYPos = Math.Clamp(transform.position.y + deltaY,minY,maxY);
        transform.position = new Vector2(newXPos, newYPos);
    }
    private void SetUpMoveBoundries()
    {
        Camera gameCamera = Camera.main;
        minX = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        maxX = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        minY = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        maxY = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }
    private IEnumerator FireContinuously()
    {
        while (true) {
            GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectTileSpeed);
            AudioSource.PlayClipAtPoint(shootingSFX, Camera.main.transform.position, volumeShootinSound);
            yield return new WaitForSeconds(projecttileFirePeriod);
        }
    }
    public int GetHealth()
    {
        return health;
    }

}
