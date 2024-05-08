using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class flagRising : MonoBehaviour
{
    private bool move = false;
    private int cnt = 0;
    private float speed = 1f;
    [SerializeField] private GameObject topOfPole,startWP;
    [SerializeField] private Text ptstxt;
    [SerializeField] private TextMeshProUGUI timertxt;
    [SerializeField] private AudioSource sound;

    private Animator anim;
    private void Awake()
    {
        if (PlayerPrefs.GetInt("Checkpoint", 0) == 1)
        {
            startWP.transform.position = transform.position;
            

        }
       
    }
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        float step = speed * Time.deltaTime;
        if (move)
        {
            transform.position = Vector2.MoveTowards(transform.position, topOfPole.transform.position,step);
        
        }
    }

   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            if (cnt == 0)
            {
                cnt++;
                sound.Play();
                move = true;
                anim.Play("FlagWave");
                PlayerPrefs.SetInt("Checkpoint", 1);

                PlayerPrefs.SetString("LastCheckPos", transform.position.x.ToString() + "*" + transform.position.y.ToString());
            }
            
        }
    }
}
