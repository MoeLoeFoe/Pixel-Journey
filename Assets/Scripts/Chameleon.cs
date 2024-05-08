using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chameleon : MonoBehaviour
{
    [SerializeField] private float moveSpeed, jumpForce;
    private Rigidbody2D rb;
    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private int currWaypointIndex;
    private Animator anim;
    [SerializeField] private GameObject Player;
    [SerializeField] private Collider2D collKillEnemyLeft, collKillPlayerLeft,collKillEnemyRight,
        collKillPlayerRight,collToungeRight,collToungeLeft;
    private SpriteRenderer sr;
    [SerializeField]private float offset;
    private bool alrdyoffset = false;
    [SerializeField]private float ThresholdDist,ThresholdY;
    //private float widthOfEnemy, widthOfPlayer;
    //private bool playerOnSideOfEnemy = true;
    //private float offset=0.7f;
    private bool alrdyhit = false, attack = false, midattack = false;
   


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        //transform.localPosition = new Vector3(0, 0, 0);
        //widthOfEnemy = sr.bounds.size.x;
        //widthOfPlayer = Player.GetComponent<SpriteRenderer>().bounds.size.x;


    }


    void Update()
    {
        
        if (Vector2.Distance(waypoints[currWaypointIndex].transform.position, transform.position) < .5f)
        {
            currWaypointIndex++;
            alrdyoffset = false;
            if (currWaypointIndex >= waypoints.Length)
            {
                currWaypointIndex = 0;
            }
        }
        if (!attack)
        {
            if (currWaypointIndex == 1)
            {
                    sr.flipX = true;
                    if (!alrdyoffset)
                    {
                        transform.position = new Vector3(transform.position.x + offset, transform.position.y, transform.position.z);
                        alrdyoffset = true;
                    }
                    if (!alrdyhit)
                    {
                        collKillEnemyLeft.enabled = false;
                        collKillPlayerLeft.enabled = false;
                        collKillEnemyRight.enabled = true;
                        collKillPlayerRight.enabled = true;
                    }
            }
            else if (currWaypointIndex == 0)
            {
                sr.flipX = false;
                if (!alrdyoffset)
                {
                    transform.position = new Vector3(transform.position.x - offset, transform.position.y, transform.position.z);
                    alrdyoffset = true;
                }
                if (!alrdyhit)
                {
                    collKillEnemyLeft.enabled = true;
                    collKillPlayerLeft.enabled = true;
                    collKillEnemyRight.enabled = false;
                    collKillPlayerRight.enabled = false;
                }

            }
        }
        
        if (Vector2.Distance(Player.transform.position, transform.position)<ThresholdDist && 
            Mathf.Abs(transform.position.y-Player.transform.position.y)<ThresholdY&&!alrdyhit)
        {
            attack = true;
            lookAtPlayer();
            anim.Play("Attack");
        }
        else if(!alrdyhit&&!midattack)
        {
            attack = false;
            anim.Play("Run");
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
    public void midattackON()
    {
        midattack = true;
    }
    public void midattackOFF()
    {
        midattack = false;
    }
    private int lookAtPlayer()
    {
        if (Player.transform.position.x - transform.position.x<-0.1f)
        {
            sr.flipX = false;
            if (currWaypointIndex == 1&&alrdyoffset)
            {
                transform.position = transform.position - new Vector3(offset, 0,0);
                alrdyoffset = false;
            }
            
            collKillEnemyLeft.enabled = true;
            collKillPlayerLeft.enabled = true;
            collKillEnemyRight.enabled = false;
            collKillPlayerRight.enabled = false;
            return -1;
        }else if(Player.transform.position.x - transform.position.x > 0.1f)
        {
            sr.flipX = true;
            if (currWaypointIndex == 0&&alrdyoffset)
            {
                
                transform.position = transform.position + new Vector3(offset, 0, 0);
                alrdyoffset=false;
            }
            
            collKillEnemyLeft.enabled = false;
            collKillPlayerLeft.enabled = false;
            collKillEnemyRight.enabled = true;
            collKillPlayerRight.enabled = true;
            return 1;
        }
        return 0;
    }
    private void enableTounge()
    {
        int temp = lookAtPlayer();
        
        if (temp == 1)
        {
            collToungeRight.transform.SetParent(null);
            collToungeLeft.enabled = false;
            collToungeRight.enabled = true;
        }
        else
        {
            collToungeLeft.transform.SetParent(null);
            collToungeLeft.enabled = true;
            collToungeRight.enabled = false;
        }
    }
    private void disableTounge()
    {
        collToungeLeft.transform.SetParent(gameObject.transform);
        collToungeRight.transform.SetParent(gameObject.transform);
        collToungeLeft.enabled = false;
        collToungeRight.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            
            collKillPlayerRight.enabled = false;
            collKillEnemyRight.enabled = false;
            collKillEnemyLeft.enabled = false;
            collKillPlayerLeft.enabled = false;
            collToungeRight.enabled = false;
            collToungeLeft.enabled=false;
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), collKillPlayerRight);
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), collKillPlayerLeft);
        }

    }
    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
