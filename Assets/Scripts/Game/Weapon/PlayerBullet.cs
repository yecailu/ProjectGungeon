using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public Vector2 Direction;

    public float ShootSpeed = 15;

    private Rigidbody2D mSelfRigidbody2D;
    private void Awake()
    {
        mSelfRigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {

    }



    void FixedUpdate()
    {
        mSelfRigidbody2D.velocity = Direction * ShootSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy)
        {
            enemy.hurt(1);//敌人受伤方法
            Destroy(gameObject);//销毁子弹
        }
        else
        { 
            Destroy(gameObject);
        }
    }
}
