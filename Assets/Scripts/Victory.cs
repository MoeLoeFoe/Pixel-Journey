using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 
using TMPro;
public class Victory : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerString,timeStrInVictory,TotalPntsInVictory;
    [SerializeField] private Text PointCnttopleft;
    [SerializeField] GameObject LvlCompleteCanvas,One,Two,Three;
    private int twoStars = 135, threeStars = 105;
    [SerializeField] private AudioSource Victorysound,VictoryBGMusic;
    [SerializeField] private Canvas gamecanvas,movecanvas;
   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerPrefs.SetInt("Checkpoint", 0);
        PlayerPrefs.SetString("pts", "Points: 0");
        PlayerPrefs.SetString("timer", "00:00");

        gamecanvas.enabled = false;
        movecanvas.enabled = false;
        Victorysound.Play();
        VictoryBGMusic.PlayDelayed(1f);
        Victorysound.volume = 0.1f;
        VictoryBGMusic.volume = 0.05f;
        timeStrInVictory.text = timerString.text;
        int timer = int.Parse(timerString.text[0].ToString()) * 600 + int.Parse(timerString.text[1].ToString()) * 60 +
            int.Parse(timerString.text[3].ToString()) * 10 + int.Parse(timerString.text[4].ToString());

        int totalpoints = int.Parse(PointCnttopleft.text.Substring(8, PointCnttopleft.text.Length-8));
        TotalPntsInVictory.text = totalpoints.ToString();
        

        if (collision.gameObject.CompareTag("Player"))
        {
            LvlCompleteCanvas.SetActive(true);
        }

        if (timer <= threeStars)
        {
            Three.SetActive(true);
            Two.SetActive(false);
            One.SetActive(false);
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().buildIndex.ToString(), 3);
        }
        else if(timer<=twoStars)
        {
            Three.SetActive(false);
            Two.SetActive(true);
            One.SetActive(false);
            if (PlayerPrefs.GetInt(SceneManager.GetActiveScene().buildIndex.ToString()) < 2){
                PlayerPrefs.SetInt(SceneManager.GetActiveScene().buildIndex.ToString(), 2);
            }
        }
        else
        {
            Three.SetActive(false);
            Two.SetActive(false);
            One.SetActive(true);
            if (PlayerPrefs.GetInt(SceneManager.GetActiveScene().buildIndex.ToString()) < 1)
            {
                PlayerPrefs.SetInt(SceneManager.GetActiveScene().buildIndex.ToString(), 1);
            }
        }

        int currLvl = SceneManager.GetActiveScene().buildIndex;
        if (PlayerPrefs.GetInt("HighestLvl") == currLvl-1)
        {
            PlayerPrefs.SetInt("HighestLvl", currLvl);
        }

        Button nextbutton = LvlCompleteCanvas.transform.Find("Next").gameObject.GetComponent<Button>();
        if (currLvl == 9)
        {
            nextbutton.interactable = false;
        }
        

        Time.timeScale = 0f;
    }

}
