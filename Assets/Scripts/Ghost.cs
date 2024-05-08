using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    private GameObject Player;
    [SerializeField] private float moveSpeed, jumpForce;
    private float velX,velY;
    private Rigidbody2D rb;
    private Animator anim;
    [SerializeField] private Collider2D collKillEnemy, collKillPlayer;
    private SpriteRenderer sr;
    private int cnt = 0;
    private bool alrdyhit = false;
    private float Threshold = 10f;
    [SerializeField] private AudioSource KillEnemySound;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        Player = GameObject.FindGameObjectWithTag("Player");
    }

   
    void Update()
    {
        velX = rb.velocity.x;
        velY = rb.velocity.y;
        if (Vector2.Distance(Player.transform.position, transform.position)<Threshold&&!alrdyhit)
        {
            transform.position = Vector2.MoveTowards(transform.position,
            Player.transform.position,
            Time.deltaTime * moveSpeed);
        }
        if (Player.transform.position.x - transform.position.x > 0.1f)
        {
            sr.flipX = true;
        }else if(Player.transform.position.x - transform.position.x < -0.1f)
        {
            sr.flipX = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("Player"))
        {
            if (cnt == 0)
            {
                anim.Play("Disappear");
                cnt++;
            }
            else
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
                    collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, jumpForce * 2);
                    rb.constraints = RigidbodyConstraints2D.FreezePositionX;
                    Invoke("DestroySelf", 2f);
                }
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rb.velocity = new Vector2(velX, velY);
        }
    }
    public void playAppear()
    {
        anim.Play("Appear");
    }
    public void playIdle()
    {
        anim.Play("Idle");
    }
    
    public void enableCollkillPlayer()
    {
        collKillPlayer.enabled = true;
    }
    public void disableCollkillPlayer()
    {
        collKillPlayer.enabled = false;
    }
    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
