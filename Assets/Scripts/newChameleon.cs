using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newChameleon : MonoBehaviour
{
    [SerializeField] private float moveSpeed, jumpForce;
    private Rigidbody2D rb;
    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private int currWaypointIndex;
    private Animator anim;
    [SerializeField] private GameObject Player;
    [SerializeField]
    private Collider2D collKillEnemyLeft,  collKillEnemyRight,
        collToungeRight, collToungeLeft;
    private SpriteRenderer sr;
    [SerializeField] private float offset,radarThreshold;
    private bool alrdyoffset = false, alrdyhit = false;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        collKillEnemyLeft.enabled = false;
        collKillEnemyRight.enabled = false;
    }

    void Update()
    {
        if (!alrdyhit)
        {
            if (Vector2.Distance(waypoints[currWaypointIndex].transform.position, transform.position) < .1f)
            {
                alrdyoffset = false;
                currWaypointIndex++;
                currWaypointIndex %= 2;
            }


            if (Vector2.Distance(Player.transform.position, transform.position) < radarThreshold
                && Mathf.Abs(transform.position.x - Player.transform.position.x) > 8f)
            {

                int dir = lookAtPlayer();
                if (dir != currWaypointIndex && dir != -1)
                {
                    alrdyoffset = false;
                    currWaypointIndex++;
                    currWaypointIndex %= 2;
                }
            }


            if (currWaypointIndex == 0)
            {
                if (!alrdyoffset && sr.flipX == true)
                {
                    transform.position = new Vector3(transform.position.x - offset, transform.position.y, transform.position.z);
                    alrdyoffset = true;
                }
                sr.flipX = false;
                collKillEnemyLeft.enabled = true;
              
                collKillEnemyRight.enabled = false;
                
            }
            else
            {
                if (!alrdyoffset && sr.flipX == false)
                {
                    transform.position = new Vector3(transform.position.x + offset, transform.position.y, transform.position.z);
                    alrdyoffset = true;
                }
                sr.flipX = true;
                collKillEnemyLeft.enabled = false;
               
                collKillEnemyRight.enabled = true;
             
            }
            if (rb.bodyType == RigidbodyType2D.Dynamic)
            {
                transform.position = Vector2.MoveTowards(transform.position,
                waypoints[currWaypointIndex].transform.position,
                Time.deltaTime * moveSpeed);
            }
        }
        
        
        

    }

    private int lookAtPlayer()
    {
        if (Player.transform.position.x - transform.position.x < -0.1f)
        {
            sr.flipX = false;
            if (currWaypointIndex == 1)
            {
                
                    transform.position = transform.position - new Vector3(offset, 0, 0);
                    alrdyoffset = true;
                
                
            }

            collKillEnemyLeft.enabled = true;
      
            collKillEnemyRight.enabled = false;
          
            return 0; // looking left
        }
        else if (Player.transform.position.x - transform.position.x > 0.1f)
        {
            sr.flipX = true;
            if (currWaypointIndex == 0)
            {

                transform.position = transform.position + new Vector3(offset, 0, 0);
                alrdyoffset = false;
            }

            collKillEnemyLeft.enabled = false;
        
            collKillEnemyRight.enabled = true;
    
            return 1; // looking right
        }
        return -1;
    }

    private void makeStatic()
    {
        rb.bodyType = RigidbodyType2D.Static;
    }
    private void makeDynamic()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
    }
    private void enableToungeRight()
    {
        collToungeRight.enabled = true;
    }
    private void enableToungeLeft()
    {
        collToungeLeft.enabled = true;
    }
    private void disableToungeRight()
    {
        collToungeRight.enabled = false;
    }
    private void disableToungeLeft()
    {
        collToungeLeft.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
           
                anim.SetBool("attack", true);
                int dir = lookAtPlayer();
                if (dir == 0) //player on the left of chameleon
                {
                    anim.Play("AttackLeft");
                }
                else if (dir == 1) //player on the right of chameleon
                {
                    anim.Play("AttackRight");
                }
            
            
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.SetBool("attack", false);
        }
    }
    private void DestroySelf()
    {
        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            
            collKillEnemyRight.enabled = false;
            collKillEnemyLeft.enabled = false;
          
            collToungeRight.enabled = false;
            collToungeLeft.enabled = false;
            if (!alrdyhit)
            {
                alrdyhit = true;
                anim.Play("Hit");
                rb.constraints = RigidbodyConstraints2D.None;
                rb.velocity = new Vector2(0, jumpForce);
                collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, jumpForce);
                rb.constraints = RigidbodyConstraints2D.FreezePositionX;
                Invoke("DestroySelf", 2f);
            }
        }
        

    }
}

