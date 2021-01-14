using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Configuration parameter
    [Header("Enemy Stats")]
    [SerializeField] float health = 100f;
    [SerializeField] int scoreValue = 150;

    [Header("Projectile")]
    [SerializeField] float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.3f;
    [SerializeField] float maxTimeBetweenShots = 3f;

    [Header("VNSFX")]
    [SerializeField] AudioClip SFXAttack;
    [SerializeField] AudioClip SFXDeath;
    [SerializeField] [Range(0,1)] float SFXVolume = 0.2f;
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed = 5f;
    [SerializeField] GameObject deathVFX;
    // Start is called before the first frame update
    void Start()
    {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime; //FrameRate Independent
        if (shotCounter <= 0f)
        {
            Fire();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Fire()
    {
        GameObject enemyLaser = Instantiate(laserPrefab, new Vector2(transform.position.x, transform.position.y - 0.4f),
                  Quaternion.identity) as GameObject;
        enemyLaser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);

        if (SFXAttack != null) AudioSource.PlayClipAtPoint(SFXAttack, Camera.main.transform.position, SFXVolume);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        ProcessHit(damageDealer);
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
        FindObjectOfType<GameSession>().AddtoScore(scoreValue);
        Destroy(gameObject);
        if (SFXDeath != null) AudioSource.PlayClipAtPoint(SFXDeath, Camera.main.transform.position, SFXVolume);
        GameObject explosion = Instantiate(deathVFX, transform.position, transform.rotation);
        Destroy(explosion, 0.5f);
    }
}
