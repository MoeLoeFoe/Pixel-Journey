using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainedSpike : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private float timer = 0f;
    private int phase = 0;

    
    private void FixedUpdate()
    {
       
        timer += Time.fixedDeltaTime;
        if (timer > 1f)
        {
            phase++;
            phase %= 4;
            timer = 0f;
        }
        
        switch (phase)
        {
            case 0:
                transform.Rotate(0f, 0f, moveSpeed * (1 - timer));
                break;
            case 1:
                transform.Rotate(0f, 0f, -moveSpeed * timer);
                break;
            case 2:
                transform.Rotate(0f, 0f, -moveSpeed * (1 - timer));
                break;
            case 3:
                transform.Rotate(0f, 0f, moveSpeed * timer);
                break;
        }
    }





}
