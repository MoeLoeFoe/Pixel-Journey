using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rockheadspikes : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rockrb,otherspike;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Static;
            rockrb.bodyType = RigidbodyType2D.Static;
            otherspike.bodyType = RigidbodyType2D.Static;
        }
    }
}
