using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenuManager : MonoBehaviour
{   
    public GameObject MainCamera;

    public GameObject AudioManager;

    public AudioSource BGM;

    public GameObject SettingPanel;

    public GameObject ContinuePanel;

    public static MenuManager Default;

    private void Awake()
    {
        Default = this;

    }

    private void OnDestroy()
    {
        Default = null;

    }

    void Start()
    {
        
        PlayerDate.Load();

        Slider slider = AudioManager.GetComponent<Slider>();

        BGM = MainCamera.GetComponent<AudioSource>();

        slider.value = BGM.volume;
    }


    //开始游戏
    public void PlayGame()
    {       
        ContinuePanel.SetActive(true);

        PlayerDate.Save();

        SceneManager.LoadScene(1);

        PlayerDate.DeletePlayerDateSaveFile();
    }

    //打开设置 
    public void OpenSetting()
    {
        SettingPanel.SetActive(true);
    }

    //关闭设置
    public void CloseSetting()
    {
        SettingPanel.SetActive(false);
    }

    //退出游戏
    public void ExitGame()
    {
        Application.Quit();
    }


    



}
