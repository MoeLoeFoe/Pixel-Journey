using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Cinemachine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    private float timer=-1f;
    [SerializeField] private GameObject LvlCompleteCanvas,PauseCanvas,MenuButton;
    [SerializeField] private GameObject[] PauseCanvasBtns,MusicSettingsBtns;
    [SerializeField] private GameObject MusicOn, MusicOff, SFXOn, SFXOff;
    [SerializeField] private GameObject ResetlvlText, QuitText, Yeslvl, Nolvl,YesQuit,NoQuit,YesExit,NoExit;
    [SerializeField] private TextMeshProUGUI TimerString;

    [SerializeField] private GameObject[] CharPrefabs;
    [SerializeField] private GameObject startingWayPoint;
    [SerializeField] private CinemachineVirtualCamera VCam;
    [SerializeField] private AudioSource BGMusic,ClickSound;
    private bool sfx=true;

    
    void Awake()
    {
        int charindex = PlayerPrefs.GetInt("CharSelect",0);
        PlayerPrefs.SetString("LastCheckPos", startingWayPoint.transform.position.x.ToString() + "*" + startingWayPoint.transform.position.y.ToString());
        GameObject player = Instantiate(CharPrefabs[charindex], startingWayPoint.gameObject.transform.position, Quaternion.identity);
        VCam.Follow=player.transform;
        
        LvlCompleteCanvas.SetActive(false);
        PauseCanvas.SetActive(false);
        MusicOff.SetActive(false);
        SFXOff.SetActive(false);
        MusicOn.SetActive(true);
        SFXOn.SetActive(true);
        for (int i = 0; i < MusicSettingsBtns.Length; i++)
        {
            MusicSettingsBtns[i].SetActive(false);
        }
        turnSureOff();

        
       



    }
    private void Update()
    {
        timer += Time.deltaTime;
        UpdateTimerString();
        if (LvlCompleteCanvas.active == true)
        {
            BGMusic.mute = true;
        }
        if (BGMusic.mute == false)
        {
            BGMusic.volume = 0.05f;
            ClickSound.volume = 0.1f;
        }
    }

    private void UpdateTimerString()
    {
        float mins = 0, secs = 0;
        float temp = timer;
        while (temp > 0 &&temp>59)
        {
            mins++;
            temp -= 60;
        }
        secs = Mathf.Floor(temp)+1;

        string str="";
        if (mins < 10)
        {
            str += "0" + mins.ToString();
        }
        else
        {
            str+= mins.ToString();
        }
        str += ":";
        if (secs < 10)
        {
            str += "0" + secs.ToString();
        }
        else
        {
            str += secs.ToString();
        }

        TimerString.text = str;
    }
    public void GoNextLvl()
    {
        PlayerPrefs.SetInt("Checkpoint", 0);
        
        if (sfx)
        {
            ClickSound.Play();
        }
        int currLvl = SceneManager.GetActiveScene().buildIndex;
        if (currLvl < 10)
        {
            SceneManager.LoadScene(currLvl + 1);
            LvlCompleteCanvas.SetActive(false);
            Time.timeScale = 1f;
        }
        



    }
    public void RestartLevel()
    {
        PlayerPrefs.SetInt("Checkpoint", 0);
        if (sfx)
        {
            ClickSound.Play();
        }
        int currLvl = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currLvl);
        LvlCompleteCanvas.SetActive(false);
        Time.timeScale = 1f;

    }
    public void HomeScreen()
    {
        PlayerPrefs.SetInt("Checkpoint", 0);
        if (sfx)
        {
            ClickSound.Play();
        }
        SceneManager.LoadScene(0);
        LvlCompleteCanvas.SetActive(false);
        Time.timeScale = 1f;
    }
    private void turnSureOff()
    {
        if (sfx)
        {
            ClickSound.Play();
        }
        ResetlvlText.SetActive(false);
        QuitText.SetActive(false);
        Yeslvl.SetActive(false);
        Nolvl.SetActive(false);
        YesQuit.SetActive(false);
        NoQuit.SetActive(false);
        YesExit.SetActive(false);
        NoExit.SetActive(false);
    }
    public void USureBroLvl()
    {
        if (sfx)
        {
            ClickSound.Play();
        }
        ResetlvlText.SetActive(true);
        QuitText.SetActive(false);
        Yeslvl.SetActive(true);
        Nolvl.SetActive(true);
        YesQuit.SetActive(false);
        NoQuit.SetActive(false);
        YesExit.SetActive(false);
        NoExit.SetActive(false);

        for (int i = 0; i < PauseCanvasBtns.Length; i++)
        {
            PauseCanvasBtns[i].SetActive(false);
        }
        for (int i = 0; i < MusicSettingsBtns.Length; i++)
        {
            MusicSettingsBtns[i].SetActive(false);
        }
    }

    public void USureBroQuit()
    {
        if (sfx)
        {
            ClickSound.Play();
        }
        ResetlvlText.SetActive(false);
        QuitText.SetActive(true);
        Yeslvl.SetActive(false);
        Nolvl.SetActive(false);
        YesQuit.SetActive(true);
        NoQuit.SetActive(true);
        YesExit.SetActive(false);
        NoExit.SetActive(false);

        for (int i = 0; i < PauseCanvasBtns.Length; i++)
        {
            PauseCanvasBtns[i].SetActive(false);
        }
        for (int i = 0; i < MusicSettingsBtns.Length; i++)
        {
            MusicSettingsBtns[i].SetActive(false);
        }
    }

    public void USureBroExit()
    {
        if (sfx)
        {
            ClickSound.Play();
        }
        ResetlvlText.SetActive(false);
        QuitText.SetActive(true);
        Yeslvl.SetActive(false);
        Nolvl.SetActive(false);
        YesQuit.SetActive(false);
        NoQuit.SetActive(false);
        YesExit.SetActive(true);
        NoExit.SetActive(true);

        for (int i = 0; i < PauseCanvasBtns.Length; i++)
        {
            PauseCanvasBtns[i].SetActive(false);
        }
        for (int i = 0; i < MusicSettingsBtns.Length; i++)
        {
            MusicSettingsBtns[i].SetActive(false);
        }
    }
    public void ShowPauseMenu()
    {
        if (sfx)
        {
            ClickSound.Play();
        }
        PauseCanvas.SetActive(true);
        MenuButton.SetActive(false);
        Time.timeScale = 0f;
    }
    public void HidePauseMenu()
    {
        if (sfx)
        {
            ClickSound.Play();
        }
        PauseCanvas.SetActive(false);
        MenuButton.SetActive(true);
        Time.timeScale = 1f;
    }
    public void PauseMenuSettings()
    {
        if (sfx)
        {
            ClickSound.Play();
        }
        for (int i = 0; i < PauseCanvasBtns.Length; i++)
        {
            PauseCanvasBtns[i].SetActive(false);
        }
        for (int i = 0; i < MusicSettingsBtns.Length; i++)
        {
            MusicSettingsBtns[i].SetActive(true);
        }
    }
    public void BackToPauseMenu()
    {
        if (sfx)
        {
            ClickSound.Play();
        }
        for (int i = 0; i < PauseCanvasBtns.Length; i++)
        {
            PauseCanvasBtns[i].SetActive(true);
        }
        for (int i = 0; i < MusicSettingsBtns.Length; i++)
        {
            MusicSettingsBtns[i].SetActive(false);
        }
        turnSureOff();
    }
    public void SetOnMusic()
    {
        if (sfx)
        {
            ClickSound.Play();
        }
        MusicOff.SetActive(false);
        MusicOn.SetActive(true);
        BGMusic.mute = false;
    }
    public void SetOnSFX()
    {
        ClickSound.Play();
        sfx = true;
        SFXOn.SetActive(true);
        SFXOff.SetActive(false);
    }
    public void SetOffMusic()
    {
        if (sfx)
        {
            ClickSound.Play();
        }
        MusicOff.SetActive(true);
        MusicOn.SetActive(false);
        BGMusic.mute = true;
    }
    public void SetOffSFX()
    {
        sfx = false;
        SFXOn.SetActive(false);
        SFXOff.SetActive(true);
    }
    public void QuitGame()
    {
        PlayerPrefs.SetInt("Checkpoint", 0);
        if (sfx)
        {
            ClickSound.Play();
        }
        Application.Quit();
    }
}
