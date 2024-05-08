using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    [SerializeField] private BoxCollider2D coll;
    [SerializeField] private AudioSource deathsound;
    [SerializeField] private PlayerMovement PM;


    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Trap") || collision.gameObject.CompareTag("Enemy"))
        {

            Die();
            
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            Die();
            
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fire"))
        {
            if (collision.gameObject.GetComponent<SpriteRenderer>().sprite.name != "Off")
            {
                Die();
                
            }
        }
    }
   
    
    public void Die()
    {
        coll.enabled = false;
        deathsound.Play();
        animator.Play("Death");
        rb.bodyType = RigidbodyType2D.Static;
        
        Invoke("PutPlayerCheckpoint", 1f);

    }
    
    private Vector2 ExtractPosFromString()
    {
        int i = 0;
        string x="", y="";
        string str=PlayerPrefs.GetString("LastCheckPos");
      
        while (str.Substring(i, 1) != "*")
        {
         
            x += str.Substring(i, 1);
            i++;
        }
  
        y = str.Substring(i+1, str.Length-x.Length-1);
        
        return new Vector2(float.Parse(x),float.Parse(y));
    }
    private void PutPlayerCheckpoint()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        coll.enabled = true;
        animator.Play("Idle");
        transform.position = ExtractPosFromString();
        PM.jumpCounter = 0;
    }








   
    
}
