using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenuManager : MonoBehaviour
{
    public GameObject MainCamera;

    public GameObject AudioManager;

    public AudioSource BGM;

    public GameObject SettingPanel;

    void Start()
    {
        Slider slider = AudioManager.GetComponent<Slider>();

        BGM = MainCamera.GetComponent<AudioSource>();

        slider.value = BGM.volume;
    }


    
    void Update()
    {
        
    }

    //开始游戏
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
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
