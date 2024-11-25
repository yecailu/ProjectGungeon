using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public Vector2 Direction;

    public Rigidbody2D Rigidbody2D;

    void Start()
    {

    }

    private void FixedUpdate()
    {
        Rigidbody2D.velocity = Direction * 5;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            collision.gameObject.GetComponent<Player>().Hurt(1);//调用玩家受伤方法
            Destroy(gameObject);//销毁子弹
        }
        else
        {
            Destroy(gameObject);//销毁子弹
        }

    }
}
