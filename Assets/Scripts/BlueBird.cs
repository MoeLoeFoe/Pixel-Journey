using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBird : MonoBehaviour
{
    private float Xboost=3, Yboost=3, jumpForce=10,velX,velY;
    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    [SerializeField] private Collider2D collKillEnemy, collKillPlayer;
    private bool alrdyhit = false;
    [SerializeField] private int dir;
    [SerializeField] private AudioSource KillEnemySound;
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        velX = rb.velocity.x;
        velY = rb.velocity.y;

        if (dir==-1&&!alrdyhit)
        {
            rb.gravityScale = 1f;
            sr.flipX = false;
            rb.velocity = new Vector2(-Xboost, rb.velocity.y);
        }
        else if (dir == 1 && !alrdyhit)
        {
            rb.gravityScale = 0.55f;
            sr.flipX = true;
            rb.velocity = new Vector2(Xboost, rb.velocity.y);
        }
    }
    public void jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, Yboost);
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
                rb.gravityScale = 3f;
                rb.constraints = RigidbodyConstraints2D.None;
                rb.velocity = new Vector2(0, 2f);
                collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, jumpForce);
                rb.constraints = RigidbodyConstraints2D.FreezePositionX;
                Invoke("DestroySelf", 2f);
            }
        }

        if (collision.gameObject.CompareTag("border"))
        {
            
            if (rb.velocity.x > 0.1f)
            {
                dir = -1;
            }
            else
            {
                dir = 1;
            }
        }
    }
    private void DestroySelf()
    {
        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), collKillPlayer);
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            rb.velocity = new Vector2(velX, velY);
        }

    }

}
