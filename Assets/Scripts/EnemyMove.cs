using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed,jumpForce;
    private Rigidbody2D rb;
    [SerializeField] private GameObject[] waypoints;
    [SerializeField]private int currWaypointIndex;
    private Animator anim;
    [SerializeField] private Collider2D collKillEnemy,collKillPlayer;
    private SpriteRenderer sr;
    [SerializeField] private AudioSource KillEnemySound;

    private ItemCollector PlayerItemCollector;
    //private float widthOfEnemy, widthOfPlayer;
    //private bool playerOnSideOfEnemy = true;
    //private float offset=0.7f;
    private bool alrdyhit = false;
    
    
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        //widthOfEnemy = sr.bounds.size.x;
        //widthOfPlayer = Player.GetComponent<SpriteRenderer>().bounds.size.x;


        PlayerItemCollector = GameObject.FindGameObjectWithTag("Player").GetComponent<ItemCollector>();
        
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





    //private void CheckPlayerPosRelaToEnemy()
    //{
    //    if (Player.transform.position.x > widthOfEnemy / 2 + widthOfPlayer / 2 + transform.position.x-offset ||
    //Player.transform.position.x < offset+transform.position.x + (-widthOfEnemy) / 2 + (-widthOfPlayer / 2))
    //    {
    //        playerOnSideOfEnemy = true;
    //    }
    //    else
    //    {
    //        playerOnSideOfEnemy = false;
    //    }
    //}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collKillPlayer.enabled = false;
            collKillEnemy.enabled = false;
            rb.bodyType = RigidbodyType2D.Dynamic;
            if (!alrdyhit)
            {
                PlayerItemCollector.MonsterCounter++;
                alrdyhit = true;
                anim.Play("Hit");
                KillEnemySound.Play();
                rb.constraints = RigidbodyConstraints2D.None;
                rb.velocity = new Vector2(0, jumpForce);
                collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, jumpForce*2);
                rb.constraints = RigidbodyConstraints2D.FreezePositionX;
                Invoke("DestroySelf", 2f);
            }
        }
    }
    
    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
