using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryPig : MonoBehaviour
{

    [SerializeField] private float moveSpeed, jumpForce;
    private Rigidbody2D rb;
    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private int currWaypointIndex;
    private Animator anim;
    [SerializeField] private Collider2D collKillEnemy, collKillPlayer;
    private SpriteRenderer sr;
    private bool alrdyhit = false;
    private int cnt = 0;
    [SerializeField] private AudioSource KillEnemySound;
    [SerializeField] private GameObject smoke1, smoke2;
    private Animator smokeanim1, smokeanim2;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        smokeanim1 = smoke1.GetComponent<Animator>();
        smokeanim2 = smoke2.GetComponent<Animator>();
    }


    void Update()
    {
       
        if (Vector2.Distance(waypoints[currWaypointIndex].transform.position, transform.position) < .5f)
        {
            currWaypointIndex++;
            if (currWaypointIndex >= waypoints.Length)
            {
                currWaypointIndex = 0;
            }
        }
        
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

    
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            cnt++;
            anim.SetInteger("state", cnt+1);
            if (cnt == 1)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, jumpForce);
                moveSpeed *= 2;
                anim.Play("RedRun");
                smokeanim1.Play("AngrySmoke");
                smokeanim2.Play("AngrySmoke");
            }
            else
            {
                collKillPlayer.enabled = false;
                collKillEnemy.enabled = false;
                rb.bodyType = RigidbodyType2D.Dynamic;
                if (!alrdyhit)
                {
                    alrdyhit = true;
                    anim.Play("Hit");
                    KillEnemySound.Play();
                    rb.constraints = RigidbodyConstraints2D.None;
                    collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 6f);
                    rb.constraints = RigidbodyConstraints2D.FreezePositionX;
                    Invoke("DestroySelf", 2f);
                }
            }
        }
    }
    public void incCnt()
    {
        cnt++;
    }
    
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), collKillPlayer);
        }

        

    }
    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
