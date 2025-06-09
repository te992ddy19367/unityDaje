using System;
using System.Diagnostics;
using Unity.Mathematics;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public float speed;
    

    private Transform startPosition;

    private Transform playert;
    private Vector2 target;

    private GameObject player;
    
    public ParticleSystem ProjectileExplosion;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playert = GameObject.FindGameObjectWithTag("Player").transform;

        target = new Vector2(playert.position.x, playert.position.y);

        player = GameObject.FindGameObjectWithTag("Player");

        startPosition = transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (transform.position.x == target.x && transform.position.y == target.y)
        {
            DestroyProjectile();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DestroyProjectile();
        }
            
    }

    private void DestroyProjectile()
    {
        Instantiate(ProjectileExplosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    
}
