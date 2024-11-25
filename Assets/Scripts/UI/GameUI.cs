using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public static GameUI Default;//����

    public GameObject GamePass;//ͨ�ؽ���
    public GameObject GameOver;//ʧ�ܽ���

    public Text HP;

    private void Awake()
    {
        Default = this;
    }

    private void OnDestroy()
    {
        Default = null;

        Global.HPChangedEvent -= UpdateHP;//ע��HP�¼�
    }



    void Start()
    {
        GamePass.transform.Find("BtnRestart").GetComponent<Button>().onClick.AddListener(() =>
        {
            Global.ResetData();//��������
            SceneManager.LoadScene("SampleScene");//���¼��س���
            
        });

        GameOver.transform.Find("BtnRestart").GetComponent<Button>().onClick.AddListener(() =>
        {
            Global.ResetData();//��������
            SceneManager.LoadScene("SampleScene");

        });

        UpdateHP();
        Global.HPChangedEvent += UpdateHP;//ע��HP�¼�

    }



    void UpdateHP()
    {
        HP.text = "HP:" + Global.HP;
    }


}
