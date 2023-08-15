using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] float health = 100f;
    [SerializeField] int scoreValue = 150;
    [Header("Effect")]
    [SerializeField] GameObject deathVFX;
    [SerializeField] AudioClip deathSFX;
    [SerializeField][Range(0, 1)] float deathSoundVolume = 0.7f;
    [SerializeField] AudioClip shootingSFX;
    [SerializeField][Range(0, 1)] float shootingSoundVolume = 0.5f;
    [Header("Projecttile")]
    float shootCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] float projectTileSpeed = 10f;
    [SerializeField] float durationOfExplosion = 1f;
    [SerializeField] GameObject laserPrefab;

    // Start is called before the first frame update
    void Start()
    {
        shootCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }
    public void SetSound(bool sound)
    {

    }
    private void CountDownAndShoot()
    {
        shootCounter -= Time.deltaTime;
        if (shootCounter <= 0)
        {
            Fire();
            shootCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }
    private void Fire()
    {
        GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectTileSpeed);
        AudioSource.PlayClipAtPoint(shootingSFX, Camera.main.transform.position, shootingSoundVolume);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.GetComponent<DamageDealer>();
        if (!damageDealer)
        {
            return;
        }
        else {
            ProcessHit(damageDealer);
        }
        
    }

    private void ProcessHit(DamageDealer damageDealer)
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
        Destroy(gameObject);
        FindObjectOfType<GameSession>().AddToScore(scoreValue);
        GameObject explosionParticles = Instantiate(deathVFX, transform.position, transform.rotation) as GameObject;
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position,deathSoundVolume);
        Destroy(explosionParticles, durationOfExplosion);
    }
}
