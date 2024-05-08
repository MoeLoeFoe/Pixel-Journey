using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private Canvas levelBoard,StartMenu,LvlBoardSoon,CharSelect,Settings;
    private int maxLevelCompleted;
    [SerializeField] private Button[] levelSelectBtns;
    private int maxNumOfLvls = 9;
    [SerializeField] private GameObject startWP;
    [SerializeField] private GameObject[] CharPrefabs;
    private GameObject player;
    [SerializeField] private GameObject SFXON, SFXOFF, MUSICON, MUSICOFF;
    [SerializeField] private AudioSource BGMusic,ClickSound;
    private bool sfx=true;
    private void Start()
    {
        levelBoard.enabled = false;
        LvlBoardSoon.enabled = false;
        StartMenu.enabled = true;
        CharSelect.enabled = false;
        Settings.enabled = false;
        SFXOFF.SetActive(false);
        MUSICOFF.SetActive(false);
        int charindex = PlayerPrefs.GetInt("CharSelect", 0);
        player=Instantiate(CharPrefabs[charindex], startWP.transform.position, Quaternion.identity);
        player.GetComponent<SpriteRenderer>().flipX = true;
        BGMusic.volume = 0.05f;
    }
    
    public void LevelSelect()
    {
        if (sfx)
        {
            ClickSound.Play();
        }
        maxLevelCompleted = Mathf.Max(PlayerPrefs.GetInt("HighestLvl"),0);
        for(int i = 0; i < maxNumOfLvls; i++)
        {
            levelSelectBtns[i].interactable = false;
        }
        for(int i = 0; i <= maxLevelCompleted; i++)
        {
            if (i < 9)
            {
                levelSelectBtns[i].interactable = true;

                int stars = PlayerPrefs.GetInt((i + 1).ToString());
                levelSelectBtns[i].transform.GetChild(1).gameObject.SetActive(false);
                levelSelectBtns[i].transform.GetChild(2).gameObject.SetActive(false);
                levelSelectBtns[i].transform.GetChild(3).gameObject.SetActive(false);
                levelSelectBtns[i].transform.GetChild(4).gameObject.SetActive(false);
                if (stars == 1)
                {
                    levelSelectBtns[i].transform.GetChild(1).gameObject.SetActive(true);
                }
                else if (stars == 2)
                {
                    levelSelectBtns[i].transform.GetChild(2).gameObject.SetActive(true);
                }
                else if (stars == 3)
                {
                    levelSelectBtns[i].transform.GetChild(3).gameObject.SetActive(true);
                }
                else
                {
                    levelSelectBtns[i].transform.GetChild(4).gameObject.SetActive(true);
                }
            }
            
        }
        

        levelBoard.enabled = true;
        LvlBoardSoon.enabled = false;
        StartMenu.enabled = false;
        CharSelect.enabled = false;
        Settings.enabled = false;

    }

    public void QuitLVL()
    {
        Application.Quit();
    }
    public void GoLevel()
    {
        if (sfx)
        {
            ClickSound.Play();
        }
        int LevelClicked = int.Parse(EventSystem.current.currentSelectedGameObject.name);
        SceneManager.LoadScene(LevelClicked);
    }

    public void GoSoon()
    {
        if (sfx)
        {
            ClickSound.Play();
        }
        levelBoard.enabled = false;
        LvlBoardSoon.enabled = true;
        StartMenu.enabled = false;
        CharSelect.enabled = false;
        Settings.enabled = false;
    }
    public void GoLvlBoard()
    {
        if (sfx)
        {
            ClickSound.Play();
        }
        levelBoard.enabled = true;
        LvlBoardSoon.enabled = false;
        StartMenu.enabled = false;
        CharSelect.enabled = false;
        Settings.enabled = false;
    }
    
    public void GoCharSelect()
    {
        if (sfx)
        {
            ClickSound.Play();
        }
        CharSelect.enabled = true;
        levelBoard.enabled = false;
        LvlBoardSoon.enabled = false;
        StartMenu.enabled = false;
        Settings.enabled = false;
    }
    
    public void GoHome()
    {
        if (sfx)
        {
            ClickSound.Play();
        }
        levelBoard.enabled = false;
        LvlBoardSoon.enabled = false;
        CharSelect.enabled = false;
        StartMenu.enabled = true;
        Settings.enabled = false;
    }
    public void GoSettings()
    {
        if (sfx)
        {
            ClickSound.Play();
        }
        levelBoard.enabled = false;
        LvlBoardSoon.enabled = false;
        CharSelect.enabled = false;
        StartMenu.enabled = false;
        Settings.enabled = true;
    }
    public void OffSFX()
    {

        SFXOFF.SetActive(true);
        SFXON.SetActive(false);
        sfx = false;
    }
    public void OnSFX()
    {
        ClickSound.Play();
        SFXOFF.SetActive(false);
        SFXON.SetActive(true);
        sfx = true;
    }
    public void OnMusic()
    {
        if (sfx)
        {
            ClickSound.Play();
        }
        MUSICON.SetActive(true);
        MUSICOFF.SetActive(false);
        BGMusic.mute = false;
        
    }
    public void OffMusic()
    {
        if (sfx)
        {
            ClickSound.Play();
        }
        MUSICON.SetActive(false);
        MUSICOFF.SetActive(true);
        BGMusic.mute=true;
    }
    public void Char0()
    {
        if (sfx)
        {
            ClickSound.Play();
        }
        PlayerPrefs.SetInt("CharSelect", 0);
        GoHome();
        GameObject.Destroy(player);
        player = Instantiate(CharPrefabs[0], startWP.transform.position, Quaternion.identity);
        player.GetComponent<SpriteRenderer>().flipX = true;
    }
    public void Char1()
    {
        if (sfx)
        {
            ClickSound.Play();
        }
        PlayerPrefs.SetInt("CharSelect", 1);
        GoHome();
        GameObject.Destroy(player);
        player = Instantiate(CharPrefabs[1], startWP.transform.position, Quaternion.identity);
        player.GetComponent<SpriteRenderer>().flipX = true;
    }
    public void Char2()
    {
        if (sfx)
        {
            ClickSound.Play();
        }
        PlayerPrefs.SetInt("CharSelect", 2);
        GoHome();
        GameObject.Destroy(player);
        player = Instantiate(CharPrefabs[2], startWP.transform.position, Quaternion.identity);
        player.GetComponent<SpriteRenderer>().flipX = true;
    }
    public void Char3()
    {
        if (sfx)
        {
            ClickSound.Play();
        }
        PlayerPrefs.SetInt("CharSelect", 3);
        GoHome();
        GameObject.Destroy(player);
        player = Instantiate(CharPrefabs[3], startWP.transform.position, Quaternion.identity);
        player.GetComponent<SpriteRenderer>().flipX = true;
    }
}
