using QFramework.ProjectGungeon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossF_Laser : MonoBehaviour
{
    public Player player;
    private bool isHurt = false;
    private bool isDetect = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isDetect = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isDetect = false;
        }
    }

    private void LaserHurt()
    {
        if (isDetect)
        {
            player.Hurt(2);
        }
    }


    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
