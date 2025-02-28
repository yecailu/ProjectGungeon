using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace QFramework.ProjectGungeon
{
    public class SettingItem : MonoBehaviour
    {
        public GameObject VolumePanel;
        public GameObject KeyPanel;
        public GameObject MainPanel;


        private void Start()
        {
            MainPanel.transform.Find("BtnRestart").GetComponent<Button>().onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
                Global.ResetData();//重置数据
                SceneManager.LoadScene("SampleScene");//重新加载场景

            });

            MainPanel.transform.Find("BtnRestartMenu").GetComponent<Button>().onClick.AddListener(() =>
            {
                Global.ResetData();//重置数据
                AudioKit.StopMusic();
                SceneManager.LoadScene(0);

            });
        }



        public void OpenVolumePanel()
        {
            VolumePanel.SetActive(true);
        }

        public void CloseVolumePanel()
        {
            VolumePanel.SetActive(false);
        }

        public void OpenKeyPanel()
        {
            KeyPanel.SetActive(true);
        }

        public void CloseKeyPanel()
        {
            KeyPanel.SetActive(false);
        }
    }
}