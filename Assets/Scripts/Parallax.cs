using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Parallax : MonoBehaviour
{
    private float startpos,total;
    public float parallaxEffect;
    
    void Start()
    {
        startpos = transform.position.y;
        total = startpos;
    }

   
    void Update()
    {
        if (transform.position.y - startpos >= 6)
        {
 
            transform.position = new Vector3(transform.position.x, startpos, transform.position.z);
            total = startpos;
        }
        total += parallaxEffect;
        transform.position = new Vector3(transform.position.x, total, transform.position.z);
       
    }
}
