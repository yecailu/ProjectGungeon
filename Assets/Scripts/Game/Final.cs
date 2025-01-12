using QFramework.ProjectGungeon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Final : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Global.NextLevel())
            {
                //重新加载当前场景
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);

            }
            else
            {
                GameUI.Default.GamePass.SetActive(true);
                Time.timeScale = 0;

            }
        }
    }
}
