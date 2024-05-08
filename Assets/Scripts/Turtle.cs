using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : MonoBehaviour
{
    private GameObject Player;
    
    [SerializeField] private float Threshold;
    private Animator anim;
    private float dist;
    
    void Start()
    {
        anim = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    
    void Update()
    {
        dist = Vector2.Distance(Player.gameObject.transform.position, transform.position);
       
        if (dist < Threshold)
        {
           
            anim.SetBool("near", true);
        }
        else
        {
           
            anim.SetBool("near", false);
        }

        
        
    }
}
