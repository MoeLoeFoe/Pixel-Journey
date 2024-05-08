using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bunny : MonoBehaviour
{
    [SerializeField] private float runBoost,jumpForce;
    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    [SerializeField]private GameObject[] waypoints;
    private int currWaypointIndex=0;
    [SerializeField] private Collider2D collKillEnemy, collKillPlayer;
    private bool alrdyhit = false;
    [SerializeField] private AudioSource KillEnemySound;
    private enum moveState { Idle, Run, Jump, Fall }
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        InvokeRepeating("jump", 1f, 1.5f);
       
        
    }

    void Update()
    {
        
        if (Vector2.Distance(waypoints[currWaypointIndex].transform.position, transform.position) < 3f)
        {
            currWaypointIndex++;
            if (currWaypointIndex >= waypoints.Length)
            {
                currWaypointIndex = 0;
            }
        }
        if (currWaypointIndex == 0)
        {
            rb.velocity = new Vector2(-runBoost, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(runBoost, rb.velocity.y);
        }
        UpdateAnimation();
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
                CancelInvoke("jump");
                rb.constraints = RigidbodyConstraints2D.None;
                rb.velocity = new Vector2(0, jumpForce);
                collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, jumpForce);
                rb.constraints = RigidbodyConstraints2D.FreezePositionX;
                Invoke("DestroySelf", 2f);
            }
        }
    }
    
    private void jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), collKillPlayer);
        }
        
    }
    private void UpdateAnimation()
    {
        moveState currState = moveState.Idle;
        if (rb.velocity.x > 0.1f)
        {
            currState = moveState.Run;
            sr.flipX = true;

        }
        else if (rb.velocity.x<-0.1f)
        {
            currState = moveState.Run;
            sr.flipX = false;
        }
        else if (rb.velocity.x == 0)
        {
            currState = moveState.Idle;
        }


        if (rb.velocity.y > 0.1f)
        {
            currState = moveState.Jump;
        }
        if (rb.velocity.y < -0.1f)
        {
            currState = moveState.Fall;
        }


        anim.SetInteger("state", (int)currState);
    }
    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
