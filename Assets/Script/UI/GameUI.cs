using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public static GameUI Default;//单例

    public GameObject GamePass;//通关界面
    public GameObject GameOver;//失败界面

    public Text HP;

    private void Awake()
    {
        Default = this;
    }

    private void OnDestroy()
    {
        Default = null;

        Global.HPChangedEvent -= UpdateHP;//注销HP事件
    }



    void Start()
    {
        GamePass.transform.Find("BtnRestart").GetComponent<Button>().onClick.AddListener(() =>
        {
            Global.ResetData();//重置数据
            SceneManager.LoadScene("SampleScene");//重新加载场景
            
        });

        GameOver.transform.Find("BtnRestart").GetComponent<Button>().onClick.AddListener(() =>
        {
            Global.ResetData();//重置数据
            SceneManager.LoadScene("SampleScene");

        });

        UpdateHP();
        Global.HPChangedEvent += UpdateHP;//注册HP事件

    }



    void UpdateHP()
    {
        HP.text = "HP:" + Global.HP;
    }


}
