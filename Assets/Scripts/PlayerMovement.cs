using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    bool devMode = false;
    bool jump = false;

    private Button right,left,up;
 
    moveButton movebtnscriptright, movebtnscriptleft;

    [SerializeField] private GameObject jumpparticles;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce,trampForce;
    private Rigidbody2D rb;
    private Animator anim,jumpanim;
    private SpriteRenderer sr;
    private BoxCollider2D coll;
    private float dirx;
    public int jumpCounter=0;
    private bool  stickToPlatform = true,appearing=true,DidFallAnimAlrdy=false;
    MoveState state=0;

    [SerializeField] private AudioSource jumpsound, appearingsound,trampsound;

    [SerializeField] private LayerMask Ground;
    [SerializeField] private LayerMask Wall;
    [SerializeField] private LayerMask NoJmpGrnd;
    [SerializeField] private LayerMask Sticky;
    [SerializeField] private LayerMask Icey;
    [SerializeField] private LayerMask tramp;
    [SerializeField] private LayerMask platform;


    [SerializeField] private Transform frontCheck;
    private bool wallSliding;
    private float wallSlidingSpeed=1f;
    private float checkRadius = 0.58f;

    [SerializeField] private ParticleSystem runningPs;
    
    private enum MoveState { idle,running,jumping,falling,doublejump,wallsliding}
    
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        jumpanim = jumpparticles.GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
        anim.Play("Appear");
        appearingsound.Play();

        GameObject[] movecontrolBtns = GameObject.FindGameObjectsWithTag("movecontrol");
        for(int i = 0; i < movecontrolBtns.Length; i++)
        {
            if (movecontrolBtns[i].name.Equals("right"))
            {
                right = movecontrolBtns[i].GetComponent<Button>();
            }else if (movecontrolBtns[i].name.Equals("left"))
            {
                left= movecontrolBtns[i].GetComponent<Button>();
            }
            else if (movecontrolBtns[i].name.Equals("up"))
            {
                up= movecontrolBtns[i].GetComponent<Button>();
            }
        }
       
       
        up.onClick.AddListener(jumpnow);
        movebtnscriptright = right.GetComponent<moveButton>();
        movebtnscriptleft = left.GetComponent<moveButton>();
    }

   
    void Update()
    {
   
        if (!appearing)
        {
           
            UpdateSlidingBasedOnWallType();
            if (devMode)
            {
                dirx = Input.GetAxisRaw("Horizontal");
            }
            
            if (IsGrounded() && !DidFallAnimAlrdy && rb.velocity.y<0.1f)
            {
                jumpanim.Play("FallParticle");
                DidFallAnimAlrdy = true;
                jumpanim.SetBool("alrdy", true);

            }
            
            rb.velocity = new Vector2(dirx * moveSpeed, rb.velocity.y);
            
            if (IsJumpableSurface())
            {
                jumpCounter = 0;
            }
            if ((Input.GetButtonDown("Jump") || jump )&& jumpCounter < 2)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpCounter++;
                jumpsound.Play();
                DidFallAnimAlrdy = false;
                jumpanim.SetBool("alrdy", false);
            }
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            
            UpdateAnimation();
            jump = false;
            if (movebtnscriptleft.buttonPressed == true)
            {
                if (IsGrounded())
                {
                    runningPs.Play();
                    if (runningPs.transform.rotation.y % 360 == 0)
                    {
                        runningPs.transform.Rotate(0, 180, 0);
                    }
                    
                }
                dirx = -1;
            }
            else if (movebtnscriptright.buttonPressed == true)
            {
                if (IsGrounded())
                {
                   
                    runningPs.Play();
                    if (runningPs.transform.rotation.y % 360 == 180)
                    {
                        runningPs.transform.Rotate(0, 180, 0);
                    }
                    
                }
                    
                dirx = 1;
            }
            else
            {
                dirx = 0;
            }
            

           
            
        }
    }
   

   
    public void jumpnow()
    {
        jump = true;
    }

    
    //returns Ground if no wall is being touched
    private LayerMask DetectWallType()
    {
        if(Physics2D.OverlapCircle(frontCheck.position, checkRadius, Wall)){
           
            return Wall;
        }else if (Physics2D.OverlapCircle(frontCheck.position, checkRadius, Sticky)){
           
            return Sticky;
        }else if (Physics2D.OverlapCircle(frontCheck.position, checkRadius, Icey)){
            
            return Icey;
        }
        return Ground;
    }
    private void UpdateSlidingBasedOnWallType()
    {
        if (!IsGrounded() && (DetectWallType() == Wall || DetectWallType() == Sticky || DetectWallType() == Icey))
        {
            wallSliding = true;
            if (DetectWallType() == Wall)
            {
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y,-wallSlidingSpeed,float.MaxValue));
            }
            if (DetectWallType() == Sticky)
            {
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed/8, float.MaxValue));

            }
            if (DetectWallType() == Icey)
            {
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed*10, float.MaxValue));
                
            }

        }
        else
        {
            wallSliding = false;
        }

        
        

        
    }

    
    public void updateAppearingBool()
    {
        appearing = false;
    }
    public void playIdle()
    {
        anim.Play("Idle");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fan"))
        {
            rb.gravityScale = -2f;
        }
        if (collision.gameObject.CompareTag("Platform"))
        {
            stickToPlatform = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fan"))
        {
            rb.gravityScale = 4.3f;
        }
        if (collision.gameObject.CompareTag("Platform"))
        {
            stickToPlatform = true;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            
            if(stickToPlatform)
            {
                transform.SetParent(collision.gameObject.transform);
            }
            
        }
        if (collision.gameObject.CompareTag("Trampoline"))
        {
            rb.velocity = new Vector2(0,trampForce);
            collision.gameObject.GetComponent<Animator>().SetTrigger("bounce");
            trampsound.Play();
           
            
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            transform.SetParent(null);
        }
    }

    private bool IsJumpableSurface()
    {
        bool icey = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, Icey);
        bool sticky = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, Sticky);
        return (icey || sticky || IsGrounded())&&((int)state!=2);

    }
    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .01f, Ground) ||
            Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .01f, tramp) ||
            Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .01f, platform); 
    }

    public void makeDynamic()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
    }
    private bool IsGroundedNonJmp()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, NoJmpGrnd);
    }
    private void UpdateAnimation()
    {
        
        if (dirx > 0)
        {
            state = MoveState.running;
            sr.flipX = false;
        }
        else if (dirx < 0)
        {
            state = MoveState.running;
            sr.flipX = true;
        }
        else
        {
            state = MoveState.idle;            
        }


        if (wallSliding)
        {
            if (rb.velocity.y > .1f)
            {
                state = MoveState.jumping;
                
            }
            else if(rb.velocity.y < -.1f)
            {
                state = MoveState.wallsliding;
                jumpCounter = 0;
            }
        }else if(rb.velocity.y < -.1f)
        {
            state = MoveState.falling;
        }

        if (rb.velocity.y > .1f)
        {
            if (jumpCounter == 2)
            {
                state = MoveState.doublejump;
                
                
            }
            else 
            {
                state = MoveState.jumping;
            }
           
        }



        anim.SetInteger("state", (int)state);
       
       
    }
}
