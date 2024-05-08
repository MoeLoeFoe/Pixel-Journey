using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firebox : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private string OnStateNameInAnimator, OffStateNameInAnimator;
    [SerializeField] private float startfire; // 2f is standard
    void Start()
    {
        anim = GetComponent<Animator>();
        InvokeRepeating("TurnOn", startfire,5f);
    }
    private void TurnOn()
    {
        anim.Play(OnStateNameInAnimator);
        Invoke("TurnOff", 2.5f);
    }

    private void TurnOff()
    {
        anim.Play(OffStateNameInAnimator);
    }

    
}
