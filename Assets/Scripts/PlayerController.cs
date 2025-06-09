using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private bool isGrounded;
    private Rigidbody2D rb;
    private bool hit;
    public float knockbackPower;
    public HealthScript playerHealth;
    public Transform attackObject;
    public LayerMask EnemyMask;
    public float timeBtwnAttacks;
    public float timerGotHit;
    public float timerHit;

    public float attackrange;
    public float damage;
    public float knockbackDirection;
    public ParticleSystem attackParticles;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerHealth = GetComponent<HealthScript>();
    }

    void Update()
    {
        // Handle horizontal movement
        if (!hit)
        {
            float moveInput = Input.GetAxis("Horizontal");
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, y: rb.linearVelocity.y);
        }
        else if (hit && timerGotHit < 1)
        {
            timerGotHit += Time.deltaTime;
        }
        else if (timerGotHit >= 1)
        {
            hit = false;
            timerGotHit = 0;
        }

        if (Input.GetAxis("Horizontal") < 0)
        {
            transform.localScale = new Vector2(-1, transform.localScale.y);
            

        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            transform.localScale = new Vector2(1, transform.localScale.y);
            

        }

        // Handle jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        if (timeBtwnAttacks <= 0)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                SpawnAttackparticles();
                Collider2D[] enemiesToHit = Physics2D.OverlapCircleAll(attackObject.position, attackrange, EnemyMask);
                for (int i = 0; i < enemiesToHit.Length; i++)
                {
                    Debug.Log("Enemy hit: " + enemiesToHit[i].name);
                    enemiesToHit[i].GetComponent<EnemyScript>().IsHit(damage);
                }
                timeBtwnAttacks = timerHit;
            }

        }
        else
        {
            timeBtwnAttacks -= Time.deltaTime;
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player is on the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        // Determine the direction of the knockback based on the position of the enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // If the enemy is on the right of the player (hit from the left)
            if (collision.transform.position.x > transform.position.x)
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

            // Apply knockback force
            playerHealth.health -= 20;
            rb.AddForce(new Vector2(knockbackDirection * knockbackPower, 3f), ForceMode2D.Impulse); // Apply force both horizontally and vertically
            hit = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Check if the player is no longer on the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(attackObject.position, attackrange);
    }
    void SpawnAttackparticles()
    {
    // Instanzia le particelle
    ParticleSystem particleSystem = Instantiate(attackParticles, transform.position, quaternion.identity);

    // Flip delle particelle: se il giocatore sta andando a sinistra, inverte la scala X delle particelle
    if (transform.localScale.x < 0) // Se il giocatore sta andando a sinistra
    {
            particleSystem.transform.Rotate(0, 0, 180);// Flip sull'asse z
    }
    
    }
    
}