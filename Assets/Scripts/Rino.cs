using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rino : MonoBehaviour
{
    [SerializeField] private float moveSpeed, jumpForce;
    private float velX;
    private Rigidbody2D rb;
    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private int currWaypointIndex;
    private Animator anim;
    [SerializeField] private Collider2D collKillEnemy, collKillPlayer;
    private SpriteRenderer sr;
    private bool alrdyhit = false,hitwall=false;
    [SerializeField] private AudioSource KillEnemySound;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    
    void Update()
    {
        velX = rb.velocity.x;
        if (Vector2.Distance(waypoints[currWaypointIndex].transform.position, transform.position) < .5f)
        {
            hitwall = true;
            anim.SetBool("hitwall",true);
            currWaypointIndex++;
            if (currWaypointIndex >= waypoints.Length)
            {
                currWaypointIndex = 0;
            }
        }
        if (!hitwall)
        {
            if (currWaypointIndex == 1)
            {
                sr.flipX = true;
            }
            else if (currWaypointIndex == 0)
            {
                sr.flipX = false;
            }

            if (!alrdyhit)
            {
                transform.position = Vector2.MoveTowards(transform.position,
                waypoints[currWaypointIndex].transform.position,
                Time.deltaTime * moveSpeed);
            }
        }
        
    }
    private void turnHitwallOFF()
    {
        hitwall = false;
        anim.SetBool("hitwall", false);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collKillPlayer.enabled = false;
            collKillEnemy.enabled = false;
            
            if (!alrdyhit)
            {
                alrdyhit = true;
                anim.Play("Hit");
                KillEnemySound.Play();
                rb.constraints = RigidbodyConstraints2D.None;
                rb.velocity = new Vector2(0, jumpForce);
                collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, jumpForce);
                rb.constraints = RigidbodyConstraints2D.FreezePositionX;
                Invoke("DestroySelf", 2f);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")&& collision.gameObject.layer!=3)
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), collKillPlayer);
        }else if(collision.gameObject.CompareTag("Enemy") && collision.gameObject.layer==3)
        {
            hitwall = true;
            anim.SetBool("hitwall", true);
            currWaypointIndex++;
            if (currWaypointIndex >= waypoints.Length)
            {
                currWaypointIndex = 0;
            }
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            rb.velocity = new Vector2(velX, rb.velocity.y);
        }

    }
    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
