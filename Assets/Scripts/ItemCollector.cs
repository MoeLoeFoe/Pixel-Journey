using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    private int FruitCounter = 0,total=0;
    public int MonsterCounter = 0;
    private Text PointscntText;
    [SerializeField] private AudioSource itemcollectsound;

    private void Start()
    {
        PointscntText = GameObject.FindGameObjectWithTag("GC").gameObject.transform.Find("PointCntTxt").GetComponent<Text>();
    }
    private void Update()
    {
        total = FruitCounter * 17 + MonsterCounter * 34;
        PointscntText.text = "Points: " + total;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Cherry"))
        {
            Destroy(collision.gameObject);
            itemcollectsound.Play();
            FruitCounter++;
            
        }
    }
}
