using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewRockHead : MonoBehaviour
{
    [SerializeField] private float upForce, threshold; // 10 is standard for force and 7 is standard for threshold.
   
    private Rigidbody2D rockRB,spike1RB,spike2RB;
    private bool alrdyjumped = false, wasAtTopBefore = false;
    private Animator anim;
    [SerializeField] private GameObject spike1,spike2;
    private BoxCollider2D coll;
    [SerializeField] private LayerMask Ground;
    private float yOfRockhead, yOfPlayer;
    GameObject Player;


    void Start()
    {
        rockRB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spike1RB = spike1.GetComponent<Rigidbody2D>();
        spike2RB = spike2.GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        Player = GameObject.FindGameObjectWithTag("Player");

        spike1RB.bodyType = RigidbodyType2D.Static;
        spike2RB.bodyType = RigidbodyType2D.Static;
        yOfRockhead = transform.position.y;
        


    }

    
    void Update()
    {
        
        yOfPlayer = Player.gameObject.transform.position.y;
       


        if (IsPlayerNear() && !alrdyjumped && !playerUnderRock())
        {
            spike1RB.bodyType = RigidbodyType2D.Dynamic;
            spike2RB.bodyType = RigidbodyType2D.Dynamic;
            jump();
            anim.SetTrigger("jumped");
            alrdyjumped = true;
        }
        if (rockRB.velocity.y<0.1f && rockRB.velocity.y> -0.1f && IsGrounded()==false)
        {
            anim.SetTrigger("idle");
            wasAtTopBefore = true;
        }
        if (IsGrounded() &&wasAtTopBefore)
        {
            spike1RB.bodyType = RigidbodyType2D.Static;
            spike2RB.bodyType = RigidbodyType2D.Static;
        }
        spike1RB.velocity = rockRB.velocity;
        spike2RB.velocity = rockRB.velocity;

    }

    private bool playerUnderRock()
    {
        if (yOfRockhead >= yOfPlayer)
        {
            return true;
        }
        return false;
    }
    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, Ground);
    }
    private void jump()
    {
        rockRB.velocity = new Vector2(0, upForce);
       
    }
    

    private bool IsPlayerNear()
    {
        
        if (Vector2.Distance(Player.gameObject.transform.position,transform.position)<threshold)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
