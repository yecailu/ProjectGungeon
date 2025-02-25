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


    //��ʼ��Ϸ
    public void PlayGame()
    {       
        ContinuePanel.SetActive(true);

        PlayerDate.Save();

        SceneManager.LoadScene(1);

        PlayerDate.DeletePlayerDateSaveFile();
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
