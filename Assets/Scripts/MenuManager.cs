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

    //��ʼ��Ϸ
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    //������ 
    public void OpenSetting()
    {
        SettingPanel.SetActive(true);
    }

    //�ر�����
    public void CloseSetting()
    {
        SettingPanel.SetActive(false);
    }

    //�˳���Ϸ
    public void ExitGame()
    {
        Application.Quit();
    }

}
