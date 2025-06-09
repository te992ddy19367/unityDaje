using Unity.Mathematics;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private Transform target;
    private Transform enemyPosition;
    public float speed;
    private float direction;
    /*
    private float timeBtwShoots;
    public float PausebtwShots;
    public AudioSource Shoot;
    public GameObject projectile;
    */
    private Transform player;
    private Rigidbody2D rb;
    public bool hit = false;
    private float timerHit = 0;
    public float health;
    public float knockbackDirection;
    public float knockbackPower;
    public bool isHit;
    public float timerGotHit;
    public ParticleSystem deathParticles;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        enemyPosition = GameObject.FindGameObjectWithTag("Enemy").transform;
        rb = GetComponent<Rigidbody2D>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        float moveHorizontal = direction;



        Vector3 movement = new Vector3(moveHorizontal, 0.0f);
        if (!hit && !isHit)
        {
            rb.linearVelocity = movement * speed;
        }
        else if (hit)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }

        if (hit && timerHit >= 1)
        {
            hit = false;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            timerHit = 0;
        }
        if (hit)
        {
            timerHit += Time.deltaTime;
        }

        if (target.position.x > enemyPosition.position.x)
        {

            direction = 1;

        }

        if (target.position.x < enemyPosition.position.x)
        {

            direction = -1;

        }
        if (isHit)
        {
            timerGotHit += Time.deltaTime;
        }
        if (isHit && timerGotHit > 1)
        {
            isHit = false;
            timerGotHit = 0;
        }

        /*
            if (timeBtwShoots <= 0)
            {
                Instantiate(projectile, transform.position, Quaternion.identity);
                //Shoot.Play();
                timeBtwShoots = PausebtwShots;
            }
            else
            {
                timeBtwShoots -= Time.deltaTime;
            }
            */
        IsDead();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            hit = true;
        }

    }

    public void IsHit(float damage)
    {
        isHit = true;
        // If the enemy is on the right of the player (hit from the left)
        if (player.position.x > transform.position.x)
        {
            knockbackDirection = -1; // Knockback goes to the left
            Debug.Log("hit da destra");
        }
        // If the enemy is on the left of the player (hit from the right)
        else
        {
            knockbackDirection = 1; // Knockback goes to the right
            Debug.Log("hit da sinistra");
        }

        rb.AddForce(new Vector2(knockbackDirection * knockbackPower, 0), ForceMode2D.Impulse); // Apply force both horizontally and vertically
        health -= damage;
    }

    public void IsDead()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
            SpawnDeathparticles();
        }
            
    }

    void SpawnDeathparticles()
    {
        // Instanzia le particelle
        ParticleSystem particleSystem = Instantiate(deathParticles, transform.position, quaternion.identity);  
        particleSystem.transform.Rotate(-90, 0, 0);  
    }
}
