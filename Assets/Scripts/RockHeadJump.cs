using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockHeadJump : MonoBehaviour
{

    [SerializeField] private float jumpForce;
    private Rigidbody2D rb;
    private int cnt = 0;
    private Animator anim;
    private float startposx, startposxtrap1, startposxtrap2;
    [SerializeField] private GameObject player;

    [SerializeField] private GameObject[] trapsidle;
    private Rigidbody2D spike1RB, spike2RB;
    private bool spikeon = false;
   

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        //turnOffTrapsJumping();
        turnOffTrapsIdle();
        startposx= transform.position.x;
        startposxtrap1 = trapsidle[0].transform.position.x;
        startposxtrap2 = trapsidle[1].transform.position.x;
        spike1RB = trapsidle[0].GetComponent<Rigidbody2D>();
        spike2RB = trapsidle[1].GetComponent<Rigidbody2D>();
        spike1RB.constraints = RigidbodyConstraints2D.FreezeRotation;
        spike2RB.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void Update()
    {
        transform.position = new Vector2(startposx,transform.position.y);
        
        if (spikeon)
        {
            spike1RB.velocity = new Vector2(spike1RB.velocity.x,rb.velocity.y);
            spike2RB.velocity = new Vector2(spike2RB.velocity.x, rb.velocity.y);
            trapsidle[0].transform.position = new Vector2(startposxtrap1, trapsidle[0].transform.position.y);
            trapsidle[1].transform.position = new Vector2(startposxtrap2, trapsidle[1].transform.position.y);
        }
        
        
    }

    public void turnOffTrapsIdle()
    {
        
        spikeon = false;
        trapsidle[0].SetActive(false);
        trapsidle[1].SetActive(false);
    }
    //public void turnOffTrapsJumping()
    //{
    //    trapsjumping[0].SetActive(false);
    //    trapsjumping[1].SetActive(false);

    //}
    public void turnOntrapsIdle()
    {
        spikeon = true;
        trapsidle[0].SetActive(true);
        trapsidle[1].SetActive(true);
    }
    //public void turnOnTrapsJumping()
    //{
    //    trapsjumping[0].SetActive(true);
    //    trapsjumping[1].SetActive(true);
    //}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            cnt++;
            if (cnt == 1)
            {
                rb.velocity = new Vector2(0, jumpForce);
                anim.SetTrigger("jump");
                anim.SetBool("jumped",true);
            }

           
           
        }
    }
}
